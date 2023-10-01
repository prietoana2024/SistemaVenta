using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemadeVenta.DLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemadeVenta.Api.Utilidad;

namespace SistemadeVenta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IRolService _rolServicio;

        public RolController(IRolService rolServicio)
        {
            _rolServicio = rolServicio;
        }
        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Lista() {
            var rsp =new Response<List<RolDTO>>();

            try 
            {
                rsp.Status = true;
                rsp.Value=await _rolServicio.Lista();
            }

            catch (Exception ex) 
            { 
                rsp.msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }

    }
}
