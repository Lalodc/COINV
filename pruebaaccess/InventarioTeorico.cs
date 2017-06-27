﻿using System;
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
                    /*String query = "SELECT [Detalle Existencias APT].idexistenciaAPT, [detalle existencias APT].idsubproducto, "+
                        "subproductos.nombresubproducto, NZ([existenciainicialkg],0)+"+
                        "NZ([entradascompraskg],0)+NZ([entradasempaquekg],0)+NZ([entradasagenteskg],0)+NZ([entradaspedidoskg],0)-NZ([salidasempaquekg],0)"+
                        "-NZ([salidasagenteskg],0)-NZ([salidaspedidoskg],0) as InventarioTeorico from [detalle existencias APT], [existencias APT],"+
                        " subproductos WHErE  [Detalle Existencias APT].idexistenciaAPT = [existencias APT].idexistenciaAPT"+
                        " AND [Detalle Existencias APT].idsubproducto = subproductos.idsubproducto order by"+
                        " [detalle existencias APT].idexistenciaapt ASC, [detalle existencias apt].idsubproducto ASC";*/

                    /*String query = "SELECT [Detalle Existencias APT].IdExistenciaAPT, [Detalle Existencias APT].IdSubProducto, " +
                        "SubProductos.NombreSubProducto, Nz([Detalle Existencias APT].[ExistenciaInicialKg],0)" +
                        "+Nz([Detalle Existencias APT].[EntradasComprasKg],0)+Nz([Detalle Existencias APT].[EntradasEmpaqueKg],0)" +
                        "-Nz([Detalle Existencias APT].[SalidasEmpaqueKg],0)+Nz([Detalle Existencias APT].[EntradasAgentesKg],0)" +
                        "-Nz([Detalle Existencias APT].[SalidasAgentesKg],0)+Nz([Detalle Existencias APT].[EntradasPedidosKg],0)" +
                        "-Nz([Detalle Existencias APT].[SalidasPedidosKg],0) AS Invet FROM ([Detalle Existencias APT] " +
                        "INNER JOIN [Existencias APT] ON [Detalle Existencias APT].IdExistenciaAPT = [Existencias APT].IdExistenciaAPT) " +
                        "INNER JOIN SubProductos ON [Detalle Existencias APT].IdSubProducto = SubProductos.IdSubProducto " +
                        "ORDER BY [Detalle Existencias APT].IdExistenciaAPT, [Detalle Existencias APT].IdSubProducto;";*/

                    String query = "SELECT [Detalle Existencias APT].IdDetalleExistencia, [Detalle Existencias APT].IdExistenciaAPT, [Detalle Existencias APT].IdSubProducto, " +
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
                    //"ORDER BY [Detalle Existencias APT].IdExistenciaAPT, [Detalle Existencias APT].IdSubProducto;";




                    adapter = new OleDbDataAdapter(query, conect);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;

                    dataGridView1.Columns[0].HeaderText = "Clave existencia";
                    dataGridView1.Columns[1].HeaderText = "Clave producto";
                    dataGridView1.Columns[2].HeaderText = "Nombre producto";
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
