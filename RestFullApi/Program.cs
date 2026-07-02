using RestFullApi.Application.Interfaces;
using RestFullApi.Infrastructure.Hubs;
using RestFullApi.Infrastructure.Repositories;
using RestFullApi.Infrastructure.Services;
using RestFullApi.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RestFullApi.Application.Features.Produksi.CatatProduksiCommand).Assembly));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- PENGATURAN DEPENDENCY INJECTION YANG BERSIH ---
// Kita hanya mendaftarkan alat-alat yang benar-benar kita pakai
builder.Services.AddScoped<IProduksiWriteRepository, ProduksiWriteRepository>();
builder.Services.AddScoped<IProduksiReadMenitRepository, ProduksiReadMenitRepository>();
builder.Services.AddScoped<IDasborNotificationService, DasborNotificationService>();

// Mendaftarkan Satpam MQTT
builder.Services.AddHostedService<MqttReceiverService>();

// Tambahkan kebijakan CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("IzinkanSignalR", policyBuilder =>
    {
        policyBuilder.SetIsOriginAllowed(_ => true)
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("IzinkanSignalR");
app.UseAuthorization();
app.MapControllers();
app.MapHub<DasborProduksiHub>("/produksihub");

app.Run();