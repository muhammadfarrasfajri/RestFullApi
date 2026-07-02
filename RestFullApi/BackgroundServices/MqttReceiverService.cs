using MediatR;
using MQTTnet;
using MQTTnet.Client;
using System.Text.Json;
using RestFullApi.Application.Features.Produksi; // Sesuaikan namespace Command milikmu

namespace RestFullApi.BackgroundServices
{
    // Wajib mewarisi BackgroundService bawaan .NET
    public class MqttReceiverService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private IMqttClient _mqttClient;

        public MqttReceiverService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var mqttFactory = new MqttFactory();
            _mqttClient = mqttFactory.CreateMqttClient();

            // Konfigurasi koneksi ke broker. 
            // Untuk testing, kita pakai broker publik gratis dari HiveMQ. Nanti bisa diganti dengan IP Mosquitto lokalmu.
            var mqttOptions = new MqttClientOptionsBuilder()
                .WithClientId($"DotNet_API_{Guid.NewGuid()}")
                .WithTcpServer("broker.hivemq.com", 1883)
                .Build();

            // 1. Tentukan apa yang terjadi saat pesan/data sensor masuk
            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                var payload = e.ApplicationMessage.ConvertPayloadToString();
                Console.WriteLine($"[MQTT] Data masuk dari topik {e.ApplicationMessage.Topic}: {payload}");

                try
                {
                    // Ubah JSON string dari sensor menjadi objek Command
                    var command = JsonSerializer.Deserialize<CatatProduksiCommand>(payload, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (command != null)
                    {
                        // Pinjam MediatR untuk mengeksekusi Command
                        using var scope = _scopeFactory.CreateScope();
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                        // Eksekusi! (Ini otomatis akan simpan ke DB & pancarkan ke SignalR)
                        await mediator.Send(command, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[MQTT] Gagal memproses data: {ex.Message}");
                }
            };

            // 2. Tentukan apa yang terjadi jika koneksi terputus (Auto-Reconnect)
            _mqttClient.DisconnectedAsync += async e =>
            {
                Console.WriteLine("[MQTT] Terputus dari broker. Mencoba nyambung lagi dalam 5 detik...");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                try { await _mqttClient.ConnectAsync(mqttOptions, stoppingToken); } catch { }
            };

            // 3. Eksekusi koneksi dan berlangganan ke topik spesifik
            try
            {
                await _mqttClient.ConnectAsync(mqttOptions, stoppingToken);
                Console.WriteLine("[MQTT] Berhasil tersambung ke broker HiveMQ!");

                // Daftarkan topik yang ingin didengarkan
                await _mqttClient.SubscribeAsync("pabrik/mesin/produksi", cancellationToken: stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MQTT] Gagal koneksi awal: {ex.Message}");
            }
        }
    }
}