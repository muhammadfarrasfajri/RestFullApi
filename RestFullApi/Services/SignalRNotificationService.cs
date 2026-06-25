using Microsoft.AspNetCore.SignalR;
using RestFullApi.Application.Interfaces;
using RestFullApi.Domain;
using RestFullApi.Hubs;

namespace RestFullApi.Services
{
    // Kelas ini mengimplementasikan kontrak dari lapisan Application
    public class SignalRNotificationService : IDasborNotificationService
    {
        private readonly IHubContext<DasborProduksiHub> _hubContext;

        public SignalRNotificationService(IHubContext<DasborProduksiHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task KirimLogBaruAsync(LogProduksi log, CancellationToken cancellationToken)
        {
            // Di sinilah SignalR benar-benar dieksekusi
            await _hubContext.Clients.All.SendAsync("TerimaLogBaru", log, cancellationToken);
        }
    }
}