using AltaRepaso.Entidades;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltaRepaso.Servicios.Interfaz
{
    public interface IServicio
    {
        List<Material> TraerMateriales();
        DataTable TraerDt(string nombreSp);
        int CrearOrdenRetiro(OrdenRetiro orden);
    }
}
