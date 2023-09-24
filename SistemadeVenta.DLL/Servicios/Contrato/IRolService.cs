using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.DTO;

namespace SistemadeVenta.DLL.Servicios.Contrato
{
    public interface IRolService
    {
        //Este metodo devuelve una lista que le voy a poner al metodo Lista()
        Task<List<RolDTO>> Lista();
    }
}
