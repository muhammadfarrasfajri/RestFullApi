using Dapper;
using Npgsql;
using RestFullApi.Application.Interfaces;
using RestFullApi.Domain;
using Microsoft.Extensions.Configuration;

namespace RestFullApi.Infrastructure.Repositories
{
    // Kelas ini "mewarisi" dan wajib mematuhi kontrak dari ILogProduksiRepository

    public class ProduksiWriteRepository : IProduksiWriteRepository
    {
        private readonly string _connString;
        public ProduksiWriteRepository(IConfiguration config) => _connString = config.GetConnectionString("DefaultConnection");

        public async Task<int> InsertLogAsync(LogProduksi log)
        {
            using var connection = new NpgsqlConnection(_connString);
            var sql = "INSERT INTO log_produksi (id, id_mesin, jumlah, waktu_deteksi) VALUES (@Id, @IdMesin, @Jumlah, @WaktuDeteksi)";
            return await connection.ExecuteAsync(sql, log);
        }
    }

    public class ProduksiReadRepository : IProduksiReadRepository
    {
        private readonly string _connectionString;

        public ProduksiReadRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection");
       
        public async Task<IEnumerable<LogProduksi>> GetAllLogAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = "SELECT id, id_mesin AS IdMesin, jumlah, waktu_deteksi AS WaktuDeteksi FROM log_produksi ORDER BY waktu_deteksi DESC LIMIT 100";

            // Dapper secara otomatis membungkus hasil kembalian SQL ke dalam List objek Domain
            return await connection.QueryAsync<LogProduksi>(sql);
        }
    }

    public class ProduksiReadByIdRepository : IProduksiReadByIdRepository
    {
        private readonly string _connectionString;

        // Constructor untuk menangkap string koneksi dari Dependency Injection
        public ProduksiReadByIdRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection");
        
        public async Task<LogProduksi> GetByIdLogAsync(Guid id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = "SELECT id, id_mesin, jumlah, waktu_deteksi FROM log_produksi WHERE id = @id";

            // 2. Gunakan QueryFirstOrDefaultAsync yang dirancang untuk mengambil 1 baris (atau null)
            return await connection.QueryFirstOrDefaultAsync<LogProduksi>(sql, new { id = id });
        }

    }

}