using MediatR;
using RestFullApi.Application.Interfaces;
using RestFullApi.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RestFullApi.Application.Features.Produksi
{
    // Kontrak Query (IRequest)
    public class AmbilRekapMenitanQuery : IRequest<IEnumerable<RekapProduksiMenitan>>
    {
    }

    // Eksekutor Query (IRequestHandler)
    public class AmbilRekapMenitanQueryHandler : IRequestHandler<AmbilRekapMenitanQuery, IEnumerable<RekapProduksiMenitan>>
    {
        private readonly IProduksiReadMenitRepository _readRepo;

        public AmbilRekapMenitanQueryHandler(IProduksiReadMenitRepository readRepo)
        {
            _readRepo = readRepo;
        }

        public async Task<IEnumerable<RekapProduksiMenitan>> Handle(AmbilRekapMenitanQuery request, CancellationToken cancellationToken)
        {
            // Pastikan method GetRekapMenitanAsync() sudah kamu tambahkan di dalam IProduksiReadRepository
            return await _readRepo.GetRekapMenitanAsync();
        }
    }
}