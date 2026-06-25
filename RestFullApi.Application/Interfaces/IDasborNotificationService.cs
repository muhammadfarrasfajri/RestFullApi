using RestFullApi.Domain;

namespace RestFullApi.Application.Interfaces
{
    public interface IDasborNotificationService
    {
        Task KirimLogBaruAsync(LogProduksi log, CancellationToken cancellationToken);
    }
}