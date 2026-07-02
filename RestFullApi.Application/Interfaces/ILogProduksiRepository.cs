using RestFullApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestFullApi.Application.Interfaces
{

    // Kontrak Khusus Write (Command)
    public interface IProduksiWriteRepository
    {
        Task<int> InsertLogAsync(LogProduksi log);
    }

    public interface IProduksiReadMenitRepository
    {
        Task<IEnumerable<RekapProduksiMenitan>> GetRekapMenitanAsync();
    }
}
