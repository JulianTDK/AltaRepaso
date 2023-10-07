using AltaRepaso.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltaRepaso.Datos.Interfaz
{
    public interface IOrdenRetiroDAO
    {
        List<Material> GetMateriales();
        DataTable GetDt(string nombreSp);
        int Crear(OrdenRetiro orden);
    }

}
