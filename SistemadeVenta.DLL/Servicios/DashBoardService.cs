using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SistemadeVenta.DLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Model;
using System.Globalization;

namespace SistemadeVenta.DLL.Servicios
{
    public class DashBoardService:IDashBoardService
    {
         private readonly IVentaRepository _ventaRepositorio;

        private readonly IGenericRepository<Producto> _productoRepositorio;

        private readonly IMapper _mapper;

        public DashBoardService(IVentaRepository ventaRepositorio, 
            IGenericRepository<Producto> productoRepositorio, IMapper mapper)
        {
            _ventaRepositorio = ventaRepositorio;
            _productoRepositorio = productoRepositorio;
            _mapper = mapper;
        }
        //recibe la tabla de ventas, el siguiente
        private IQueryable<Venta> retornarVentas
            (IQueryable<Venta> tablaVenta, int restarCantidadDias)
        {
            //el ? despues de datetime significa que permitira nullos
            DateTime? ultimaFecha = tablaVenta.OrderByDescending(v => v.FechaRegistro).
                Select(v => v.FechaRegistro).First();
            //nos lo ordenará por fecha de registro
            ultimaFecha = ultimaFecha.Value.AddDays(restarCantidadDias);
            //vamos a obtener la ultima fecha encontrada y a esa fecha le vamos a restar los dias
            return tablaVenta.Where(v => v.FechaRegistro.Value.Date >= ultimaFecha.Value.Date);
        }
        //EL SIGUIENTE ES PARA MOSTRAR EN NUESTRO DASHBOARD EL NUMERO DE VENTAS, COMO UN DIGITO
        private async Task<int> TotalVentasUltimaSemana() 
        {
            int total = 0;
            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();
            //validamos que si existan ventas

            if (_ventaQuery.Count() > 0)
            {
                var tablaVenta = retornarVentas(_ventaQuery, -7);
                //vamos a obtener el total de ventas que han sido registradas estos 7 dias
                total = tablaVenta.Count();
            }
            return total;
        }

        //EL SIGUIENTE METODO MOSTRARÁ EL TOTAL DE INGRESOS DE LA ULTIMA SEMANA
        private async Task<string> TotalIngresosUltimaSemana()
        { //todo metodo retorna el tipo de aquí arriba
            decimal resultado = 0;

            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();
            //validamos que si existan ventas
            if (_ventaQuery.Count() > 0)
            {
                var tablaVenta = retornarVentas(_ventaQuery, -7);
                //vamos a obtener el total de ventas que han sido registradas estos 7 dias
                resultado = tablaVenta.Select(v => v.Total).Sum(v=>v.Value);
            }
            return Convert.ToString(resultado, new CultureInfo("es-CO"));
        }
        private async Task<int> TotalProductos()
        { 
  
            IQueryable<Producto> _productoQuery = await _productoRepositorio.Consultar();
            int total = _productoQuery.Count();
            return total;
        }
        private async Task<Dictionary<string, int>>ventasUltimaSemana
            ()
        {
            //variable diccionario oara ingresar es un string
            Dictionary<string, int> resultado = new Dictionary<string, int>();
            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();

            if (_ventaQuery.Count() > 0) 
            {
                var tablaVenta = retornarVentas(_ventaQuery, -7);

                resultado=tablaVenta
                    .GroupBy(v=>v.FechaRegistro.Value.Date)
                    .OrderBy(g=>g.Key)
                    .Select(dv=>new {fecha=dv.Key.ToString("dd/MM/yyyy"),total=dv.Count()})
                    .ToDictionary(keySelector:r=>r.fecha,elementSelector:r=>r.total);
            }
            return resultado;
        }

        public async Task<DashBoardDTO> Resumen()
        {
            DashBoardDTO vmDashboard = new DashBoardDTO();
            try
            {
                vmDashboard.TotalVentas = await TotalVentasUltimaSemana();
                vmDashboard.TotalIngresos = await TotalIngresosUltimaSemana();
                vmDashboard.TotalProductos = await TotalProductos();

                List<VentaSemanaDTO>listaVentaSemana=new List<VentaSemanaDTO>();

                foreach (KeyValuePair<string, int> item in await ventasUltimaSemana()) 
                {
                    listaVentaSemana.Add(new VentaSemanaDTO()
                    {
                        Fecha = item.Key,
                        Total = item.Value
                    });
                }
                vmDashboard.VentasUltimaSemana = listaVentaSemana;
             }
            catch
            {
                throw;
            }
            return vmDashboard;
        }
    }
}
