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
using Microsoft.EntityFrameworkCore;

namespace SistemadeVenta.DLL.Servicios
{
   public class UsuarioService:IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepositorio;

        private readonly IMapper _mapper;

        public UsuarioService(IGenericRepository<Usuario> usuarioRepositorio, IMapper mapper)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
        }
        public async Task<List<UsuarioDTO>> Lista()
        {
            try 
            {
                var queryUsuario = await _usuarioRepositorio.Consultar();
                var listaUsuarios = queryUsuario.Include(rol => rol.IdRolNavigation).ToList();
                return _mapper.Map<List<UsuarioDTO>>(listaUsuarios);
            }
            catch 
            {
                throw;
            }
        }

        public async Task<SesionDTO> ValidarCredenciales(string correo, string clave)
        {
            try
            {
                var queryUsuario = await _usuarioRepositorio.Consultar(u=>u.Correo==correo && u.Clave==clave);
                if (queryUsuario.FirstOrDefault() == null) 
                {
                    throw new TaskCanceledException("El usuario no existe");

                }
                Usuario devolverUsuario = queryUsuario.Include(rol=>rol.IdRolNavigation).First();
                //este usuario de la linea de arriba o devolver usuario, es del tipo Usuario, asi que debemos pasarlo por mapper y convertirlo de tipo SesionDTO
                return _mapper.Map<SesionDTO>(devolverUsuario);


            }
            catch
            {
                throw;
            }
        }
    
        public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
        {
            try
            {
                //nuestro usuariocreado recibe un usuario, pero no es del tipo dto, asi que para recibirlo en _usuarioRepositorio debemos covertirlo, así lo aceptará el modelo
                var UsuarioCreado = await _usuarioRepositorio.Crear(_mapper.Map<Usuario>(modelo));
                if(UsuarioCreado.IdUsuario==0)
                {
                    throw new TaskCanceledException("Usuario no se pudo crear");
                }
                //con el await obtenemos, si es cero el usuario fallo, sino, continuaria aquí
                var query = await _usuarioRepositorio.Consultar(u => u.IdUsuario == UsuarioCreado.IdUsuario);
                UsuarioCreado=query.Include(rol => rol.IdRolNavigation).First();
                return _mapper.Map<UsuarioDTO>(UsuarioCreado);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(UsuarioDTO modelo)
        {
            try
            {
                var usuarioModelo = _mapper.Map<Usuario>(modelo);
                var usuarioEncontrado=await _usuarioRepositorio.Obtener(u=>u.IdUsuario==usuarioModelo.IdUsuario);
                if (usuarioEncontrado== null) 
                {
                    throw new TaskCanceledException("Usuario no existe");
                }


                usuarioEncontrado.NombreCompleto = usuarioModelo.NombreCompleto;
                usuarioEncontrado.Correo = usuarioModelo.Correo;
                usuarioEncontrado.IdRol = usuarioModelo.IdRol;
                usuarioEncontrado.Clave= usuarioModelo.Clave;
                usuarioEncontrado.EsActivo = usuarioModelo.EsActivo;

                bool respuesta=await _usuarioRepositorio.Editar(usuarioEncontrado);

                if (respuesta == false) 
                {
                    throw new TaskCanceledException("No se pudo editar");
                }
                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var usuarioEncontrado = await _usuarioRepositorio.Obtener(u => u.IdUsuario==id);
                //donde usuario encontrado en la segunda parte no lleva await 

                if (usuarioEncontrado == null) 
                {
                    throw new TaskCanceledException("El usuario no existe");
                }

                bool respuesta = await _usuarioRepositorio.Eliminar(usuarioEncontrado);

                if (respuesta==false) 
                {
                    throw new TaskCanceledException("No se pudo eliminar");
                     
                }
                return respuesta;

            }
            catch
            {
                throw;
            }
        }

      
    }
}
