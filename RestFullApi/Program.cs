using RestFullApi.Application.Interfaces;
using RestFullApi.Hubs;
using RestFullApi.Infrastructure.Repositories;
using RestFullApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Mendaftarkan MediatR agar Controller bisa menggunakannya
builder.Services.AddSignalR();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RestFullApi.Application.Features.Produksi.CatatProduksiCommand).Assembly));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProduksiWriteRepository, ProduksiWriteRepository>();
builder.Services.AddScoped<IProduksiReadRepository, ProduksiReadRepository>();
builder.Services.AddScoped<IProduksiReadByIdRepository, ProduksiReadByIdRepository>();
builder.Services.AddScoped<IDasborNotificationService, SignalRNotificationService>();

// Tambahkan kebijakan CORS ini
builder.Services.AddCors(options =>
{
    options.AddPolicy("IzinkanSignalR", builder =>
    {
        builder.SetIsOriginAllowed(_ => true) // Izinkan akses dari mana saja untuk testing lokal
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials(); // SignalR wajib menggunakan ini
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
