using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;


namespace pruebaaccess
{
    class AccesoDatos
    {
        public static OleDbConnection conn = null;

        public void Conexion()
        {
            conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:/Users/Public/Documents/Sipe/Datoscopia.mdb");
        }

        public void AbrirConexion()
        {
            conn.Open();
        }

        public void CerrarConexion()
        {
            conn.Close();
        }


    }
}
