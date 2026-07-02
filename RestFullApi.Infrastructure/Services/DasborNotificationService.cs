using Microsoft.AspNetCore.SignalR;
using RestFullApi.Application.Interfaces;
using RestFullApi.Domain;
using System.Threading;
using System.Threading.Tasks;

// UBAH BARIS INI: Arahkan ke rumah barunya
using RestFullApi.Infrastructure.Hubs;

namespace RestFullApi.Infrastructure.Services
{
    public class DasborNotificationService : IDasborNotificationService
    {
        private readonly IHubContext<DasborProduksiHub> _hubContext;

        public DasborNotificationService(IHubContext<DasborProduksiHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task KirimLogBaruAsync(LogProduksi log, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.SendAsync("TerimaLogBaru", cancellationToken: cancellationToken);
        }
    }
}