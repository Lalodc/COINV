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
using Microsoft.Office.Interop.Access.Dao;


namespace pruebaaccess
{
    public partial class InventarioTeorico : Form
    {
        public static String cadConex = ConfigurationManager.ConnectionStrings["conAccess"].ConnectionString;        
        OleDbCommand cmd = new OleDbCommand();        
        //OleDbDataReader reader;
        OleDbDataAdapter adapter;
        BindingSource bs;
        DataTable dt;

        public InventarioTeorico()
        {
            InitializeComponent();
        }

        private void InventarioTeorico_Load(object sender, EventArgs e)
        {
            try
            {
                using (OleDbConnection conect = new OleDbConnection(PedidosSalidas.cadConex))
                {
                    

                    String query = "SELECT [Detalle Existencias APT].IdExistenciaAPT, [Detalle Existencias APT].IdSubProducto, " +
                        "SubProductos.NombreSubProducto, ROUND(IIF(IsNull([Detalle Existencias APT].[ExistenciaInicialKg]),0,[Detalle Existencias APT].[ExistenciaInicialKg])" +
                        "+IIF(IsNull([Detalle Existencias APT].[EntradasComprasKg]),0,[Detalle Existencias APT].[EntradasComprasKg])" +
                        "+IIF(IsNull([Detalle Existencias APT].[EntradasEmpaqueKg]),0,[Detalle Existencias APT].[EntradasEmpaqueKg])" +
                        "-IIF(IsNull([Detalle Existencias APT].[SalidasEmpaqueKg]),0,[Detalle Existencias APT].[SalidasEmpaqueKg])" +
                        "+IIF(IsNull([Detalle Existencias APT].[EntradasAgentesKg]),0,[Detalle Existencias APT].[EntradasAgentesKg])" +
                        "-IIF(IsNull([Detalle Existencias APT].[SalidasAgentesKg]),0,[Detalle Existencias APT].[SalidasAgentesKg])" +
                        "+IIF(IsNull([Detalle Existencias APT].[EntradasPedidosKg]),0,[Detalle Existencias APT].[EntradasPedidosKg])" +
                        "-IIF(IsNull([Detalle Existencias APT].[SalidasPedidosKg]),0,[Detalle Existencias APT].[SalidasPedidosKg]),2) AS Invet " +
                        "FROM ([Detalle Existencias APT] INNER JOIN [Existencias APT] ON [Detalle Existencias APT].IdExistenciaAPT = [Existencias APT].IdExistenciaAPT) " +
                        "INNER JOIN SubProductos ON [Detalle Existencias APT].IdSubProducto = SubProductos.IdSubProducto ";

                    //BUENO//
                    /*String query = "SELECT [Detalle Existencias APT].IdDetalleExistencia, [Detalle Existencias APT].IdExistenciaAPT, [Detalle Existencias APT].IdSubProducto, " +
                        "SubProductos.NombreSubProducto, ROUND(IIF(IsNull([Detalle Existencias APT].[ExistenciaInicialKg]),0,[Detalle Existencias APT].[ExistenciaInicialKg])" +
                        "+IIF(IsNull([Detalle Existencias APT].[EntradasComprasKg]),0,[Detalle Existencias APT].[EntradasComprasKg])" +
                        "+IIF(IsNull([Detalle Existencias APT].[EntradasEmpaqueKg]),0,[Detalle Existencias APT].[EntradasEmpaqueKg])" +
                        "-IIF(IsNull([Detalle Existencias APT].[SalidasEmpaqueKg]),0,[Detalle Existencias APT].[SalidasEmpaqueKg])" +
                        "+IIF(IsNull([Detalle Existencias APT].[EntradasAgentesKg]),0,[Detalle Existencias APT].[EntradasAgentesKg])" +
                        "-IIF(IsNull([Detalle Existencias APT].[SalidasAgentesKg]),0,[Detalle Existencias APT].[SalidasAgentesKg])" +
                        "+IIF(IsNull([Detalle Existencias APT].[EntradasPedidosKg]),0,[Detalle Existencias APT].[EntradasPedidosKg])" +
                        "-IIF(IsNull([Detalle Existencias APT].[SalidasPedidosKg]),0,[Detalle Existencias APT].[SalidasPedidosKg]),2) AS Invet " +
                        "FROM ([Detalle Existencias APT] INNER JOIN [Existencias APT] ON [Detalle Existencias APT].IdExistenciaAPT = [Existencias APT].IdExistenciaAPT) " +
                        "INNER JOIN SubProductos ON [Detalle Existencias APT].IdSubProducto = SubProductos.IdSubProducto ";
                    //"ORDER BY [Detalle Existencias APT].IdExistenciaAPT, [Detalle Existencias APT].IdSubProducto;";*/




                    adapter = new OleDbDataAdapter(query, conect);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;

                    this.dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView1.Columns[0].HeaderText = "Clave existencia";
                    this.dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView1.Columns[1].HeaderText = "Clave producto";
                    this.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView1.Columns[2].HeaderText = "Nombre producto";
                    this.dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView1.Columns[3].HeaderText = "Inventario teorico";

                    bs = new BindingSource();
                    bs.DataSource = dt;
                    bindingNavigator1.BindingSource = bs;
                    dataGridView1.DataSource = bs;




                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: "+ ex.Message);
                throw;
            }            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Inicio frmInicio = new Inicio();
            frmInicio.Show();
            this.Close();
        }
    }
}
