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


namespace SistemadeVenta.DLL.Servicios
{
    public class RolService:IRolService
    {
        private readonly IGenericRepository<Rol> _rolRepositorio;

        private readonly IMapper _mapper;

        public RolService(IGenericRepository<Rol> rolRepositorio, IMapper mapper)
        {
            _rolRepositorio = rolRepositorio;
            _mapper = mapper;
        }

        public async Task<List<RolDTO>> Lista()
        {
            //nuestra varibale lista de roles va a utilizar el metodo consultar
            try 
            {
                var listaRoles=await _rolRepositorio.Consultar();
                //_mapper.Map va a poder convertir desde nuestra claro rol a nuestro modelo DTO
                //El metodo consultar devulve un yqueryable de tipo rol, asi que debemos devolver a roldto el convertido desde rol, mapper nos convierte, estre los <> ponemos a lo quedeseamos que se convierta
                //Entre los parentesis pasamos el origen
                //Lo que hicimos es convertir de ROL a ROLDTO en forma de lista
                return _mapper.Map<List<RolDTO>>(listaRoles.ToList());
            }
            catch
            {
                throw;
            }
        }
    }
}
