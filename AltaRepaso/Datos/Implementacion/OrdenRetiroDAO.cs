using AltaRepaso.Datos.Interfaz;
using AltaRepaso.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace AltaRepaso.Datos.Implementacion
{
    public class OrdenRetiroDAO : IOrdenRetiroDAO
    {
        public int Crear(OrdenRetiro orden)
        {
            int ordenNro = 0;
            SqlConnection cnn = HelperDAO.GetInstance().GetSqlConnection();
            SqlTransaction t = null;
            try
            {
                cnn.Open();
                t = cnn.BeginTransaction();
                SqlCommand cmdMaestro = new SqlCommand("SP_INSERTAR_ORDEN", cnn, t);
                cmdMaestro.CommandType = CommandType.StoredProcedure;
                cmdMaestro.Parameters.AddWithValue("@responsable", orden.Responsable);

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@nro";
                param.SqlDbType = SqlDbType.Int;
                param.Direction = ParameterDirection.Output;
                cmdMaestro.Parameters.Add(param);

                cmdMaestro.ExecuteNonQuery();

                ordenNro = Convert.ToInt32(param.Value);
                int detallleNro = 1;

                SqlCommand cmdDetalle;
                foreach (DetalleOrden d in orden.lDetalles)
                {
                    cmdDetalle = new SqlCommand("SP_INSERTAR_DETALLES", cnn, t);
                    cmdDetalle.CommandType = CommandType.StoredProcedure;
                    cmdDetalle.Parameters.AddWithValue("@nro_orden", ordenNro);
                    cmdDetalle.Parameters.AddWithValue("@detalle", detallleNro);
                    cmdDetalle.Parameters.AddWithValue("@codigo", d.Material.Codigo);
                    cmdDetalle.Parameters.AddWithValue("@cantidad", d.Cantidad);
                    cmdDetalle.ExecuteNonQuery();
                    detallleNro++;
                }
                t.Commit();
            }
            catch
            {
                if (t != null)
                    t.Rollback();
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
            return ordenNro;
        }

        public DataTable GetDt(string nombreSp)
        {
            return HelperDAO.GetInstance().ConsultarSp(nombreSp);
        }

        public List<Material> GetMateriales()
        {
            List<Material> list = new List<Material>();

            DataTable dt = HelperDAO.GetInstance().ConsultarSp("SP_CONSULTAR_MATERIALES");
            foreach (DataRow dr in dt.Rows)
            {
                int cod = Convert.ToInt32(dr["codigo"]);
                string nom = dr["nombre"].ToString();
                int stock = Convert.ToInt32(dr["stock"]);
                Material m = new Material(cod, nom, stock);
                list.Add(m);
            }

            return list;
        }
    }
}
