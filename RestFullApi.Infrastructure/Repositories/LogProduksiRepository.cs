using Dapper;
using Npgsql;
using RestFullApi.Application.Interfaces;
using RestFullApi.Domain;
using Microsoft.Extensions.Configuration;

namespace RestFullApi.Infrastructure.Repositories
{
    // 1. REPOSITORI UNTUK MENULIS DATA (COMMAND)
    public class ProduksiWriteRepository : IProduksiWriteRepository
    {
        private readonly string _connString;

        // Menggunakan ?? "" untuk mencegah warning kuning dari .NET 8
        public ProduksiWriteRepository(IConfiguration config)
            => _connString = config.GetConnectionString("DefaultConnection") ?? "";

        public async Task<int> InsertLogAsync(LogProduksi log)
        {
            using var connection = new NpgsqlConnection(_connString);

            // A. Simpan data mentah dari MQTT
            var sqlInsert = "INSERT INTO log_produksi (id, id_mesin, jumlah, waktu_deteksi) VALUES (@Id, @IdMesin, @Jumlah, @WaktuDeteksi)";
            var result = await connection.ExecuteAsync(sqlInsert, log);

            return result;
        }
    }

    // 2. REPOSITORI UNTUK MEMBACA DATA REKAP (QUERY)
    public class ProduksiReadMenitRepository : IProduksiReadMenitRepository
    {
        private readonly string _connectionString;

        public ProduksiReadMenitRepository(IConfiguration configuration)
            => _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";

        public async Task<IEnumerable<RekapProduksiMenitan>> GetRekapMenitanAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);

            // KUNCI RAHASIA: Kita hitung langsung dari tabel mentah 'log_produksi' 
            // menggunakan fungsi time_bucket dari TimescaleDB. Dijamin instan!
            var sql = @"
                SELECT 
                    time_bucket('1 minute', waktu_deteksi) AS RentangWaktu, 
                    id_mesin AS IdMesin, 
                    SUM(jumlah) AS TotalProduksi, 
                    COUNT(id) AS FrekuensiSensor 
                FROM log_produksi 
                GROUP BY RentangWaktu, id_mesin
                ORDER BY RentangWaktu DESC 
                LIMIT 50";

            return await connection.QueryAsync<RekapProduksiMenitan>(sql);
        }
    }
}