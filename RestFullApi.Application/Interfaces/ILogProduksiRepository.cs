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

    // Kontrak Khusus Read (Query)
    public interface IProduksiReadRepository
    {
        Task<IEnumerable<LogProduksi>> GetAllLogAsync();
    }

    public interface IProduksiReadByIdRepository
    {
        Task<LogProduksi> GetByIdLogAsync(Guid id);
    }
}
