using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestFullApi.Application.Features.Produksi;

namespace RestFullApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProduksiController : ControllerBase
    {
        // 1. Controller HANYA meminta IMediator
        private readonly IMediator _mediator;

        public ProduksiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Endpoint POST: api/produksi/catat
        [HttpPost("catat")]
        public async Task<IActionResult> CatatProduksi([FromBody] CatatProduksiCommand command)
        {
            try
            {
                // Controller melempar Command (Write) ke MediatR
                await _mediator.Send(command);

                return Ok(new
                {
                    Status = "Sukses",
                    Pesan = $"Berhasil mencatat {command.Jumlah} barang dari mesin {command.IdMesin}."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Pesan = "Terjadi kesalahan server", Error = ex.Message });
            }
        }

        [HttpGet("rekap-menit")]
        public async Task<IActionResult> GetRekapMenitan()
        {
            // UBAH BARIS INI: Gunakan AmbilRekapMenitanQuery, bukan GetRekapProduksiMenitanQuery
            var hasil = await _mediator.Send(new AmbilRekapMenitanQuery());
            return Ok(hasil);
        }
    }
}