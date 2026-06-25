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

        // Endpoint GET: api/produksi/rekap
        [HttpGet("rekap")]
        public async Task<IActionResult> AmbilRekap()
        {
            // Controller melempar Query (Read All) ke MediatR
            var data = await _mediator.Send(new AmbilRekapQuery());
            return Ok(data);
        }

        // Endpoint GET: api/produksi/rekap/{id}
        [HttpGet("rekap/{id}")]
        public async Task<IActionResult> AmbilRekapByid(Guid id)
        {
            // Controller melempar Query (Read By Id) beserta parameter ID-nya ke MediatR
            var data = await _mediator.Send(new AmbilRekapByIdQuery { Id = id });

            if (data == null)
            {
                return NotFound(new { Pesan = "Data log produksi tidak ditemukan." });
            }

            return Ok(data);
        }
    }
}