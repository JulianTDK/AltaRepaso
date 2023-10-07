using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace AltaRepaso.Datos
{
    public class HelperDAO
    {
        private static HelperDAO instance;
        private SqlConnection cnn;
        private HelperDAO()
        {
            cnn = new SqlConnection(Properties.Resources.CadenaConexion);
        }

        public static HelperDAO GetInstance()
        {
            if (instance == null)
            {
                instance = new HelperDAO();
            }
            return instance;
        }
        public SqlConnection GetSqlConnection()
        {
            return this.cnn;
        }

        public DataTable ConsultarSp(string nombreSP)
        {
            cnn.Open();
            SqlCommand cmd = new SqlCommand(nombreSP, cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            cnn.Close();
            return dt;
        }
    }
}
