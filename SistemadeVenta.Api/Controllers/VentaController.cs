using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemadeVenta.DLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemadeVenta.Api.Utilidad;

namespace SistemadeVenta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _ventaServicio;

        public VentaController(IVentaService ventaServicio)
        {
            _ventaServicio = ventaServicio;
        }

        [HttpPost]
        [Route("Registrar")]
        public async Task<IActionResult>Registrar([FromBody] VentaDTO Venta)
        { 
            var rsp=new Response<VentaDTO>();

            try 
            {
                rsp.Status = true;
                rsp.Value=await _ventaServicio.Registrar(Venta);
            }
            catch (Exception ex)
            {
                rsp.Status=false;
                rsp.msg = ex.Message;
            }
        }
        [HttpGet]
        [Route("Reporte")]
        public async Task<IActionResult> Registrar([FromBody] VentaDTO Venta)
        {
            var rsp = new Response<VentaDTO>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _ventaServicio.Registrar(Venta);
            }
            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.msg = ex.Message;
            }
        }
    }
}
