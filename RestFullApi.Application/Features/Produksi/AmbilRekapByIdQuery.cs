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
    public class AmbilRekapByIdQuery : IRequest<LogProduksi>
    {
        public Guid Id { get; set; }
    }

    public class AmbilRekapByIdQueryHandler : IRequestHandler<AmbilRekapByIdQuery, LogProduksi>
    {
        private readonly IProduksiReadByIdRepository _readRepo;

        public AmbilRekapByIdQueryHandler(IProduksiReadByIdRepository readRepo)
        {
            _readRepo = readRepo;
        }

        public async Task<LogProduksi> Handle(AmbilRekapByIdQuery request, CancellationToken cancellationToken)
        {
            // Ambil nilainya menggunakan 'request.Id'
            return await _readRepo.GetByIdLogAsync(request.Id);
        }
    }
}
