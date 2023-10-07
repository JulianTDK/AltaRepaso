using AltaRepaso.Datos.Implementacion;
using AltaRepaso.Datos.Interfaz;
using AltaRepaso.Entidades;
using AltaRepaso.Servicios.Interfaz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltaRepaso.Servicios.Implementacion
{
    public class Servicio : IServicio
    {
        private IOrdenRetiroDAO dao;
        public Servicio()
        {
            dao = new OrdenRetiroDAO();
        }

        public int CrearOrdenRetiro(OrdenRetiro orden)
        {
            return dao.Crear(orden);
        }

        public DataTable TraerDt(string nombreSp)
        {
            return dao.GetDt(nombreSp);
        }

        public List<Material> TraerMateriales()
        {
            return dao.GetMateriales();
        }
    }
}
