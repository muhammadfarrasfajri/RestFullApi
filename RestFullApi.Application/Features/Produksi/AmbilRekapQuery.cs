using MediatR;
using RestFullApi.Application.Interfaces;
using RestFullApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestFullApi.Application.Features.Produksi
{
    public class AmbilRekapQuery : IRequest<IEnumerable<LogProduksi>>
    {
    }

    public class AmbilRekapQueryHandler : IRequestHandler<AmbilRekapQuery, IEnumerable<LogProduksi>>
    {
        private readonly IProduksiReadRepository _readRepo;

        public AmbilRekapQueryHandler(IProduksiReadRepository readRepo)
        {
            _readRepo = readRepo;
        }

        public async Task<IEnumerable<LogProduksi>> Handle(AmbilRekapQuery request, CancellationToken cancellationToken)
        {
            return await _readRepo.GetAllLogAsync();
        }
    }
}
