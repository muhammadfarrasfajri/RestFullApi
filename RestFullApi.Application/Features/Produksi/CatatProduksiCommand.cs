using MediatR;
using RestFullApi.Application.Interfaces;
using RestFullApi.Domain;

namespace RestFullApi.Application.Features.Produksi
{
    public class CatatProduksiCommand : IRequest<int>
    {
        public required string IdMesin { get; set; }
        public int Jumlah { get; set; }
    }

    public class CatatProduksiCommandHandler : IRequestHandler<CatatProduksiCommand, int>
    {
        // Panggil kontrak Write-nya, bukan koneksi database-nya
        private readonly IProduksiWriteRepository _writeRepo;

        private readonly IDasborNotificationService _notificationService;

        public CatatProduksiCommandHandler(IProduksiWriteRepository writeRepo, IDasborNotificationService notificationService)
        {
            _writeRepo = writeRepo;
            _notificationService = notificationService;
        }

        public async Task<int> Handle(CatatProduksiCommand request, CancellationToken cancellationToken)
        {
            var log = new LogProduksi
            {
                Id = Guid.NewGuid(),
                IdMesin = request.IdMesin,
                Jumlah = request.Jumlah,
                WaktuDeteksi = DateTime.UtcNow
            };

            var result = await _writeRepo.InsertLogAsync(log);

            await _notificationService.KirimLogBaruAsync(log, cancellationToken);

            // Handler hanya mendelegasikan tugas ke Infrastructure
            return result;
        }
    }
}