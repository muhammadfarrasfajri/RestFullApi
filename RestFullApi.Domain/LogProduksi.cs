using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestFullApi.Domain
{
    public class LogProduksi
    {
        public Guid Id { get; set; }
        public required string IdMesin { get; set; }
        public int Jumlah { get; set; }
        public DateTime WaktuDeteksi { get; set; }
    }

    public class RekapProduksiMenitan
    {
        public DateTime RentangWaktu { get; set; }
        public string IdMesin { get; set; } = string.Empty;
        public int TotalProduksi { get; set; }
        public int FrekuensiSensor { get; set; }
    }
}
