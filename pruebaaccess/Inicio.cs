using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace pruebaaccess
{
    public partial class Inicio : Form
    {
        //OleDbConnection conect = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:/Users/Public/Documents/Sipe/Datoscopia.mdb");
        //OleDbCommand cmd = new OleDbCommand();
        public static String cadConex = ConfigurationManager.ConnectionStrings["conAccess"].ConnectionString;

        OleDbCommand cmd = new OleDbCommand();
        //OleDbConnection conect;
        //OleDbCommand cmd;
        OleDbDataReader reader;
        OleDbDataAdapter adapter;
        BindingSource bs;
        DataTable dt1;
        DataTable dt2;

        public Inicio()
        {
            InitializeComponent();
        }

        private void salidasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PedidosSalidas frmPedidosSalidas = new PedidosSalidas();
            frmPedidosSalidas.Show();
            this.Hide();
        }

        private void mostrarInventarioTeóricoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InventarioTeorico frmInventarioTeorico = new InventarioTeorico();
            frmInventarioTeorico.Show();
            this.Hide();
        }

        private void entradasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmpaqueEntradas frmEmpaqueEntradas = new EmpaqueEntradas();
            frmEmpaqueEntradas.Show();
            this.Hide();
        }

        private void salidasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            EmpaqueSalidas frmEmpaqueSalidas = new EmpaqueSalidas();
            frmEmpaqueSalidas.Show();
            this.Hide();
        }

        private void entradasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AgenteEntradas frmAgenteEntradas = new AgenteEntradas();
            frmAgenteEntradas.Show();
            this.Hide();
        }

        private void salidasToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            AgenteSalidas frmAgenteSalidas = new AgenteSalidas();
            frmAgenteSalidas.Show();
            this.Hide();
        }

        private void insertarExistenciasDelMesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OleDbConnection conect = new OleDbConnection(PedidosSalidas.cadConex))
            {
                try
                {
                    conect.Open();
                    cmd = conect.CreateCommand();
                    String val = obtenerTopDeMesAnterior(conect);
                    dt2 = new DataTable();
                    dt2 = obtenerProductosDePT(conect);

                    DateTime hoy = DateTime.Today;

                    int idExistencia;
                    String fechahoy = hoy.ToShortDateString();
                    //String id = hoy.Year + "" + fechahoy.Split('')[0];
                    if (hoy.Month >= 1 && hoy.Month <= 9)
                    {
                        idExistencia = Convert.ToInt32(hoy.Year + "0" + hoy.Month);
                    }
                    else
                    {
                        idExistencia = Convert.ToInt32(hoy.Year + "" + hoy.Month);
                    }

                    DialogResult dialog = MessageBox.Show("Desea insertar las existencias del mes?", "Guardar existencias", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        insertarEncabezadoExistencia(idExistencia, fechahoy);

                        if (dt2.Rows.Count > 0)
                        {
                            foreach (DataRow item in dt2.Rows)
                            {
                                String idprod = item["idsubproducto"].ToString();
                                Double punitario = Convert.ToDouble(item["preciounitario"]);
                                Double inventF = obtenerInventarioFisicoAnterior(val, idprod);
                                if (inventF >= 0)
                                {
                                    cmd.CommandText = "insert into [detalle existencias apt](idexistenciaapt, idsubproducto, preciounitario, existenciainicialkg) " +
                                        "values(@idexistencia, @idsubprod, @punit, @existini);";
                                    cmd.Parameters.AddWithValue("@idexistencia", idExistencia);
                                    cmd.Parameters.AddWithValue("@idsubprod", idprod);
                                    cmd.Parameters.AddWithValue("@punit", punitario);
                                    cmd.Parameters.AddWithValue("@existini", inventF);
                                    cmd.ExecuteNonQuery();
                                    cmd.Parameters.Clear();
                                    //insertarDetalleExistencia(idExistencia, idprod,  punitario, inventF);
                                }
                                else
                                {
                                    cmd.CommandText = "insert into [detalle existencias apt](idexistenciaapt, idsubproducto, preciounitario) " +
                                        "values(@idexistencia, @idsubprod, @punit);";
                                    cmd.Parameters.AddWithValue("@idexistencia", idExistencia);
                                    cmd.Parameters.AddWithValue("@idsubprod", idprod);
                                    cmd.Parameters.AddWithValue("@punit", punitario);
                                    cmd.ExecuteNonQuery();
                                    cmd.Parameters.Clear();
                                }
                            }
                        }
                        cmd = new OleDbCommand("COMMIT", conect);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Existencias del mes insertadas con éxito");
                    }else if(dialog == DialogResult.No){
                        cmd = new OleDbCommand("ROLLBACK", conect);
                        cmd.ExecuteNonQuery();
                    }
                    

                    //cmd.CommandText = "select top 1 idexistenciaapt from [detalle existencias apt] order by idexistenciaapt desc;";
                    //reader = cmd.ExecuteReader();
                    //reader.Read();
                    //String val = Convert.ToString(reader.GetValue(0));






                    //DateTime hoy = DateTime.Today;

                    //int idExistencia;
                    //String fechahoy = hoy.ToShortDateString();
                    ////String id = hoy.Year + "" + fechahoy.Split('')[0];
                    //if (hoy.Month >= 1 && hoy.Month <= 9)
                    //{
                    //    idExistencia = Convert.ToInt32(hoy.Year + "0" + hoy.Month);
                    //}
                    //else
                    //{
                    //    idExistencia = Convert.ToInt32(hoy.Year + "" + hoy.Month);
                    //}
                    
                    
                    //insertarEncabezadoExistencia(idExistencia,fechahoy);
                    //insertarDetalleExistencia(conect, idExistencia);
                    //MessageBox.Show("listo");
                    

                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: "+ex.Message);                    
                }
            }
        }


        /**/
        public String obtenerTopDeMesAnterior(OleDbConnection conect)
        {
            try
            {
                cmd = new OleDbCommand("BEGIN TRANSACTION", conect);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "select top 1 idexistenciaapt from [detalle existencias apt] order by idexistenciaapt desc;";
                reader = cmd.ExecuteReader();
                reader.Read();
                String id = Convert.ToString(reader.GetValue(0));
                reader.Close();
                return id;
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        /**/
        public DataTable obtenerProductosDePT(OleDbConnection conect)
        {
            try
            {
                String query = "SELECT [subproductos].idsubproducto, [subproductos].preciounitario FROM subproductos " +
                "WHERE (((subproductos.idcategoría)=9 Or (subproductos.idcategoría)=12));";


                adapter = new OleDbDataAdapter(query, conect);
                dt1 = new DataTable();
                adapter.Fill(dt1);
                return dt1;
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        /**/
        public Double obtenerInventarioFisicoAnterior(String idExist, String idproduct)
        {
            try
            {
                cmd.CommandText = "select inventariofisicokg from [Detalle existencias apt] where idExistenciaapt = @idExist AND idsubproducto = @idsubprod;";
                cmd.Parameters.AddWithValue("@idExist", idExist);
                cmd.Parameters.AddWithValue("@idsubprod", idproduct);
                reader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                int count = Convert.ToInt32(reader.Read());
                if (count == 1)
                {
                    Double inventFis = Convert.ToDouble(reader.GetValue(0));
                    reader.Close();
                    return inventFis;
                }
                else
                {
                    reader.Close();
                    return -1;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            
            //Double inventFis = Convert.ToDouble(reader.GetValue(0));
           
            //return count;
        }




        public void insertarEncabezadoExistencia(int idExistencia, String fechahoy)
        {
            try
            {
                cmd.CommandText = "insert into [existencias APT](idExistenciaAPT, fechaexistenciaapt) values(@idExistencia, @fechaExist);";
                cmd.Parameters.AddWithValue("@idExistencia", idExistencia);
                cmd.Parameters.AddWithValue("@fechaExist", fechahoy);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        //private void inventarioToolStripMenuItem_Click(object sender, EventArgs e)
        //{

        //}

        //public void insertarDetalleExistencia(OleDbConnection conect, int idExistencia)
        //{

            //String query = "SELECT [subproductos].idsubproducto, [subproductos].preciounitario FROM subproductos " +
            //    "WHERE (((subproductos.idcategoría)=9 Or (subproductos.idcategoría)=12));";


            //adapter = new OleDbDataAdapter(query,conect);
            //dt1 = new DataTable();
            //adapter.Fill(dt1);

            //if (dt1.Rows.Count > 0)
            //{
            //    foreach (DataRow item in dt1.Rows)
            //    {
            //        String idprod = item["idsubproducto"].ToString();
            //        Double punit = Convert.ToInt32(item["preciounitario"]);

            //        cmd.CommandText = "insert into [Detalle Existencias APT](idexistenciaapt, idsubproducto, preciounitario) " +
            //            "values(@idexistencia, @idsubprod, @punit)";
            //        cmd.Parameters.AddWithValue("@idexistencia", idExistencia);
            //        cmd.Parameters.AddWithValue("@idsubprod", idprod);
            //        cmd.Parameters.AddWithValue("@punit", punit);
            //        cmd.ExecuteNonQuery();
            //        cmd.Parameters.Clear();


            //    }
            //}
            

        //}
    }
}
