using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.DTO;

namespace SistemadeVenta.DLL.Servicios.Contrato
{
    public interface IVentaService
    {
        Task<VentaDTO> Registrar(VentaDTO modelo);
        Task<List<VentaDTO>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin);
        Task<List<ReporteDTO>> Reporte(string fechaInicio, string fechaFin);
    }
}
