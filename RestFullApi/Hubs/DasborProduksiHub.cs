using Microsoft.AspNetCore.SignalR;

namespace RestFullApi.Hubs
{
    // Wajib mewarisi kelas Hub dari SignalR
    public class DasborProduksiHub : Hub
    {
        // Untuk saat ini, kita biarkan kosong.
        // Mengapa kosong? Karena dalam kasus IoT, dasbor (klien) hanya bertugas 
        // "mendengarkan", sedangkan server (API-mu) yang akan aktif "berteriak" mengirim data.
    }
}