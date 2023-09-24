using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Model;

namespace SistemaVenta.DAL.Repositorios
{//agregar las herencias
    public class VentaRepository: GenericRepository<Venta>,IVentaRepository
    {
        private readonly DbventaContext _dbContext;

        public VentaRepository(DbventaContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Venta> Registrar(Venta modelo)
        {//CREAMOS UNA VARIABLE
            Venta ventaGenerada=new();
            //si dentro de la logica ocurre un error la linea siguiente tiene que reestablecer todo al principio
            using (var transaction= _dbContext.Database.BeginTransaction()) {
                try {
                    foreach (DetalleVenta dv in modelo.DetalleVenta)
                    {
                        Producto producto_encontrado = _dbContext.Productos.Where(p => p.IdProducto == dv.IdProducto).First();
                        producto_encontrado.Stock = producto_encontrado.Stock - dv.Cantidad;
                        _dbContext.Productos.Update(producto_encontrado);
                    }await _dbContext.SaveChangesAsync();
                    NumeroDocumento correlativo=_dbContext.NumeroDocumentos.First();
                    correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
                    correlativo.FechaRegistro=DateTime.Now;
                    _dbContext.NumeroDocumentos.Update(correlativo);
                    await _dbContext.SaveChangesAsync();
                    //0001
                    int CantidadDigitos = 4;
                    string ceros = string.Concat(Enumerable.Repeat("0", CantidadDigitos));
                    string numeroVenta=ceros+correlativo.UltimoNumero.ToString();
                    numeroVenta =numeroVenta.Substring(numeroVenta.Length-CantidadDigitos, CantidadDigitos);
                    modelo.NumeroDocumento = numeroVenta;

                    await _dbContext.AddAsync(modelo);
                    await _dbContext.SaveChangesAsync();

                    ventaGenerada = modelo;
                    //la transaccion puede finalizar sin nigun problema
                    transaction.Commit();
                }    
                catch 
                {
                    //devolvera todo como estaba antes
                    transaction.Rollback();
                    //devuelve el error
                    throw;
                }
                return ventaGenerada;
            }
          //  throw new NotImplementedException();
        }

        
    }
}
