using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace SistemaVenta.DAL.Repositorios
{
    public class GenericRepository<TModelo>:IGenericRepository<TModelo> where TModelo : class
    {
        //variable de lectura de la base de datos:
        private readonly DbventaContext _dbcontext;
       
        //contructor recibe el contecto y almacena el valor en dbcontext
        public GenericRepository(DbventaContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        //al precionar control . sobre la clase IGenericRepository me va a crear todos los metodos
       
        public async Task<TModelo> Obtener(Expression<Func<TModelo, bool>> filtro)
        {//NECESITAMOS DEVOLVER EL MODELO CON EL CUAL ESTA SIENDO CONSULTADO, AWAIT PORQUE SON METODOS ASINCRONOS
            try 
            { 
                TModelo modelo = await _dbcontext.Set<TModelo>().Where(filtro).FirstOrDefaultAsync();
                return modelo;
            }
            catch { throw; }
           // throw new NotImplementedException();
        }
        public async Task<TModelo> Crear(TModelo modelo)
        {
            //USAMOS LA BASE DE DATOS Y ESTABLECEMOS CON QUE MODELO VAMOS A ESTAR UTILIZANDO, PASAMOS EL MODELO QUE ESTEMOS RECIBIENDO
            try
            {
                _dbcontext.Set<TModelo>().Add(modelo);
                //guardar cambios de manera asincrona asi:
                await _dbcontext.SaveChangesAsync();
                return modelo;
            }
            catch { throw; }
            //throw new NotImplementedException();
        }

        public async Task<bool> Editar(TModelo modelo)
        {//llamamos la base de datos, establecemos que modelo vamos a utilizar
            try
            {
                _dbcontext.Set<TModelo>().Update(modelo);
                await _dbcontext.SaveChangesAsync();
                return true;

            }
            catch { throw; }
          //  throw new NotImplementedException();
        }

        public async Task<bool> Eliminar(TModelo modelo)
        {
            try
            {
                _dbcontext.Set<TModelo>().Remove(modelo);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { throw; }
          //  throw new NotImplementedException();
        }
        public async Task<IQueryable<TModelo>> Consultar(Expression<Func<TModelo, bool>> filtro = null)
        {//UNA CONSULTA QUE SERA EJECUTADA, DEVUELVE LA CONSULTA Y QUIEN LO LLAME, LO EJECUTA, VALIDAMOS SI INGRESO ALGO EN EL FILTRO PARA BUSCAR O DEVOLVER EL MODELO
            try
            {
                IQueryable<TModelo>queryModelo=filtro==null? _dbcontext.Set<TModelo>():_dbcontext.Set<TModelo>().Where(filtro);
                return queryModelo;
            }
            catch { throw; }
            //throw new NotImplementedException();
        }

    }
}
