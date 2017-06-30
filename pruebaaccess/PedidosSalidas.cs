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
using System.Text.RegularExpressions;

namespace pruebaaccess
{
    public partial class PedidosSalidas : Form
    {
        public static String cadConex = ConfigurationManager.ConnectionStrings["conAccess"].ConnectionString;

        //public event DataGridViewDataErrorEventHandler DataError;

        //OleDbConnection conect = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:/Users/Public/Documents/Sipe/Datoscopia.mdb");
        OleDbCommand cmd = new OleDbCommand();
        //OleDbConnection conect;
        //OleDbCommand cmd;
        OleDbDataReader reader;
        OleDbDataAdapter adapter;
        BindingSource bs;
        DataTable dt;

        

        DataGridViewComboBoxCell dpbox;

        Double totalpiezas;
        Double totalpesobruto;
        Double totalpesoneto;
        int bandera;

        public PedidosSalidas()
        {            
            //AccesoDatos conect = new AccesoDatos();
            //cmd.Connection = AccesoDatos.conn;
            //conect.AbrirConexion();
            //MessageBox.Show("Abierta");
            InitializeComponent();
            //conect.Open();
            
            
            //cmd = conect.CreateCommand();


            //Obtener la clave y sumar 1
            /*cmd.CommandText = "SELECT TOP 1 IdSAPTPedidos FROM [APT-Pedidos Salidas] ORDER BY IdSAPTPedidos desc;";
            reader = cmd.ExecuteReader();
            reader.Read();
            int val = Convert.ToInt32(reader.GetValue(0));
            String a = (val + 1).ToString();
            textBox1.Text = a;*/

            

            //buscar id de la abla APT-Pedidos Salidas donde la fecha de pedido es el 3 de junio del 2017//
            //cmd.CommandText = "SELECT IdSAPTPedidos FROM [APT-Pedidos Salidas] WHERE (FechaSAPTPedidos = #6/3/2017#);";
            /*cmd.CommandText = "SELECT IdSAPTPedidos FROM [APT-Pedidos Salidas] WHERE (FechaSAPTPedidos = #6/3/2017#);";
            reader = cmd.ExecuteReader();
            reader.Read();
            String a = reader.GetValue(0).ToString();
            textBox1.Text = a;*/

            
            

            //buscar nombre y apellido de la tabla agentes con el id Abraham//
            /*cmd.CommandText = "SELECT Nombre, ApellidoPaterno FROM Agentes WHERE (((IdAgente)=[@name]));";
            cmd.Parameters.AddWithValue("@name", "Abraham");
            reader = cmd.ExecuteReader();
            reader.Read();
            String a = reader.GetValue(0).ToString();
            //String a = conect.State.ToString();
            textBox1.Text = a;*/
        }

        private void detalle_APT_Pedidos_SalidasBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            //this.detalle_APT_Pedidos_SalidasBindingSource.EndEdit();
            //this.tableAdapterManager.UpdateAll(this.datoscopiaDataSet);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.detalle_APT_Pedidos_SalidasDataGridView.DataError +=
            new DataGridViewDataErrorEventHandler(detalle_APT_Pedidos_SalidasDataGridView_DataError);
            using (OleDbConnection conect = new OleDbConnection(PedidosSalidas.cadConex))
            {
                
                        try
                        {
                            conect.Open();
                            cmd = conect.CreateCommand();
                            cmd.CommandText = "SELECT TOP 1 IdSAPTPedidos FROM [APT-Pedidos Salidas] ORDER BY IdSAPTPedidos desc;";
                            reader = cmd.ExecuteReader();
                            reader.Read();
                            String idPedidoSalida = Convert.ToString(reader.GetValue(0));
                            //String a = (val + 1).ToString();
                            textBox1.Text = idPedidoSalida;

                            bandera = 0;
                            

                            BuscarSalidaPedidosEncabezado(idPedidoSalida);
                            BuscarSalidaPedidosDetalle(idPedidoSalida);

                            if (bandera == 0)
                            {
                                textBox2.Text = "";
                                comboBox1.Text = "";
                                comboBox2.Text = "";
                            }
                            comboBox1.Enabled = false;
                            comboBox2.Enabled = false;
                            //detalle_APT_Pedidos_SalidasDataGridView.Enabled = false;
                            detalle_APT_Pedidos_SalidasDataGridView.ReadOnly = true;
                            //buscar id de la abla APT-Pedidos Salidas donde la fecha de pedido es el 3 de junio del 2017//
                            //cmd.CommandText = "SELECT IdSAPTPedidos FROM [APT-Pedidos Salidas] WHERE (FechaSAPTPedidos = #6/3/2017#);";
                            /*cmd.CommandText = "SELECT IdSAPTPedidos FROM [APT-Pedidos Salidas] WHERE (FechaSAPTPedidos = #6/3/2017#);";
                            reader = cmd.ExecuteReader();
                            reader.Read();
                            String a = reader.GetValue(0).ToString();
                            textBox1.Text = a;*/
                            /*String query = "SELECT subproductos.idsubproducto, subproductos.nombresubproducto, [Detalle APT-Pedidos Salidas].idlotecaducidadpedidos, "+
                                "[Detalle APT-Pedidos Salidas].fechacaducidadsaptpedidos, [Detalle APT-Pedidos Salidas].piezassaptpedidos,"+
                                "[Tipos de cajas].descripcióntipocaja, [Detalle APT-Pedidos Salidas].númerodecajassaptpedidos,"+
                                "[Detalle APT-Pedidos Salidas].pesobrutosaptpedidos, Cstr([Tipos de cajas].pesocaja*[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos)," +
                                "Cstr([Detalle APT-Pedidos Salidas].pesobrutosaptpedidos-([Tipos de cajas].pesocaja*[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos)), Cstr(([Detalle APT-Pedidos Salidas].pesobrutosaptpedidos-([Tipos de cajas].pesocaja*[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos))/[Detalle APT-Pedidos Salidas].piezassaptpedidos)" +
                                "from subproductos, [Detalle APT-Pedidos Salidas], [Tipos de cajas] where [Detalle APT-Pedidos Salidas].idsaptpedidos =32095 "+
                                "AND subproductos.idsubproducto = [Detalle APT-Pedidos Salidas].idsubproductosaptpedidos AND [Tipos de cajas].idtipocaja = idtipocajasaptpedidos";*/

                            /*String query = "SELECT subproductos.idsubproducto, subproductos.nombresubproducto, [Detalle APT-Pedidos Salidas].idlotecaducidadpedidos, " +
                                "[Detalle APT-Pedidos Salidas].fechacaducidadsaptpedidos, [Detalle APT-Pedidos Salidas].piezassaptpedidos, [Tipos de cajas].descripcióntipocaja, " +
                                "[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos, [Detalle APT-Pedidos Salidas].pesobrutosaptpedidos, " +
                                "[Tipos de cajas].pesocaja*[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos AS PesoCajas, " +
                                "[Detalle APT-Pedidos Salidas].pesobrutosaptpedidos-PesoCajas AS PesoNeto, "+
                                "IIf([Detalle APT-Pedidos Salidas].piezassaptpedidos=0, PesoNeto, PesoNeto/[Detalle APT-Pedidos Salidas].piezassaptpedidos) as PesoPromedio " +
                                "from subproductos, [Detalle APT-Pedidos Salidas], [Tipos de cajas] where [Detalle APT-Pedidos Salidas].idsaptpedidos =32095 " +
                                "AND subproductos.idsubproducto = [Detalle APT-Pedidos Salidas].idsubproductosaptpedidos AND [Tipos de cajas].idtipocaja = idtipocajasaptpedidos";*/

                            /*String query = "SELECT subproductos.idsubproducto, subproductos.nombresubproducto, [Detalle APT-Pedidos Salidas].idlotecaducidadpedidos, " +
                                "[Detalle APT-Pedidos Salidas].fechacaducidadsaptpedidos, [Detalle APT-Pedidos Salidas].piezassaptpedidos, [Tipos de cajas].descripcióntipocaja, " +
                                "[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos, [Detalle APT-Pedidos Salidas].pesobrutosaptpedidos, " +
                                "[Tipos de cajas].pesocaja*[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos AS PesoCajas, " +
                                "[Detalle APT-Pedidos Salidas].pesobrutosaptpedidos-PesoCajas AS PesoNeto, " +
                                "IIf([Detalle APT-Pedidos Salidas].piezassaptpedidos=0, PesoNeto, PesoNeto/[Detalle APT-Pedidos Salidas].piezassaptpedidos) as PesoPromedio " +
                                "from subproductos, [Detalle APT-Pedidos Salidas], [Tipos de cajas] where [Detalle APT-Pedidos Salidas].idsaptpedidos =" + idPedidoSalida + " " +
                                "AND subproductos.idsubproducto = [Detalle APT-Pedidos Salidas].idsubproductosaptpedidos AND [Tipos de cajas].idtipocaja = idtipocajasaptpedidos";





                            //cmd.Parameters.AddWithValue("@cajas", "0");
                            //poner el parametro de numero de cajas


                            //adapter = new OleDbDataAdapter(cmd.CommandText, conect);
                            adapter = new OleDbDataAdapter(query, conect);
                            //DataSet ds = new DataSet();
                            DataTable dt = new DataTable();
                            //dt = ds.Tables["subproductos"]; ;
                            adapter.Fill(dt);
                            //detalle_APT_Pedidos_SalidasDataGridView.DataSource = ds.Tables[0].Rows[1];
                            //detalle_APT_Pedidos_SalidasBindingSource.DataSource = dt;
                            //detalle_APT_Pedidos_SalidasTableAdapter.Fill(dt);


                            detalle_APT_Pedidos_SalidasDataGridView.DataSource = dt;

                            detalle_APT_Pedidos_SalidasDataGridView.Columns[0].HeaderText = "Clave";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[1].HeaderText = "Producto";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[2].HeaderText = "Lote";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[3].HeaderText = "Caducidad";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[4].HeaderText = "Piezas";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[5].HeaderText = "Tipo Caja";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[6].HeaderText = "Cajas";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[7].HeaderText = "Peso Bruto";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[8].HeaderText = "Peso Cajas";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[9].HeaderText = "Peso Neto";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[10].HeaderText = "Peso Promedio";
                            //detalle_APT_Pedidos_SalidasDataGridView.Data

                            bs = new BindingSource();
                            bs.DataSource = dt;
                            bindingNavigator1.BindingSource = bs;
                            detalle_APT_Pedidos_SalidasDataGridView.DataSource = bs;*/
                        }
                        catch (Exception)
                        {


                        }
                    }
                    
                    //reader.Close();  1111111
                    //conect.Close();   2222222222
                
                   
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            using (OleDbConnection conect = new OleDbConnection(PedidosSalidas.cadConex))
            {
                if (e.KeyChar == Convert.ToChar(Keys.Enter))
                {
                    button2.Enabled = false;
                    Regex Val = new Regex(@"^[+-]?\d+(\\d+)?$");
                    //Regex Val = new Regex(@"^[+-]?\d+(\.\d+)?$");
                    if(Val.IsMatch(textBox1.Text))
                    //if (textBox1.Text != "".Trim())
                    {
                        try
                        {
                            bandera = 0;
                            String idPedidoSalida = textBox1.Text;

                            BuscarSalidaPedidosEncabezado(idPedidoSalida);
                            BuscarSalidaPedidosDetalle(idPedidoSalida);

                            if (bandera == 0)
                            {
                                textBox2.Text = "";
                                comboBox1.Text = "";
                                comboBox2.Text = "";
                            }
                            comboBox1.Enabled = false;
                            comboBox2.Enabled = false;
                            //detalle_APT_Pedidos_SalidasDataGridView.Enabled = false;
                            detalle_APT_Pedidos_SalidasDataGridView.ReadOnly = true;
                            //buscar id de la abla APT-Pedidos Salidas donde la fecha de pedido es el 3 de junio del 2017//
                            //cmd.CommandText = "SELECT IdSAPTPedidos FROM [APT-Pedidos Salidas] WHERE (FechaSAPTPedidos = #6/3/2017#);";
                            /*cmd.CommandText = "SELECT IdSAPTPedidos FROM [APT-Pedidos Salidas] WHERE (FechaSAPTPedidos = #6/3/2017#);";
                            reader = cmd.ExecuteReader();
                            reader.Read();
                            String a = reader.GetValue(0).ToString();
                            textBox1.Text = a;*/
                            /*String query = "SELECT subproductos.idsubproducto, subproductos.nombresubproducto, [Detalle APT-Pedidos Salidas].idlotecaducidadpedidos, "+
                                "[Detalle APT-Pedidos Salidas].fechacaducidadsaptpedidos, [Detalle APT-Pedidos Salidas].piezassaptpedidos,"+
                                "[Tipos de cajas].descripcióntipocaja, [Detalle APT-Pedidos Salidas].númerodecajassaptpedidos,"+
                                "[Detalle APT-Pedidos Salidas].pesobrutosaptpedidos, Cstr([Tipos de cajas].pesocaja*[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos)," +
                                "Cstr([Detalle APT-Pedidos Salidas].pesobrutosaptpedidos-([Tipos de cajas].pesocaja*[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos)), Cstr(([Detalle APT-Pedidos Salidas].pesobrutosaptpedidos-([Tipos de cajas].pesocaja*[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos))/[Detalle APT-Pedidos Salidas].piezassaptpedidos)" +
                                "from subproductos, [Detalle APT-Pedidos Salidas], [Tipos de cajas] where [Detalle APT-Pedidos Salidas].idsaptpedidos =32095 "+
                                "AND subproductos.idsubproducto = [Detalle APT-Pedidos Salidas].idsubproductosaptpedidos AND [Tipos de cajas].idtipocaja = idtipocajasaptpedidos";*/

                            /*String query = "SELECT subproductos.idsubproducto, subproductos.nombresubproducto, [Detalle APT-Pedidos Salidas].idlotecaducidadpedidos, " +
                                "[Detalle APT-Pedidos Salidas].fechacaducidadsaptpedidos, [Detalle APT-Pedidos Salidas].piezassaptpedidos, [Tipos de cajas].descripcióntipocaja, " +
                                "[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos, [Detalle APT-Pedidos Salidas].pesobrutosaptpedidos, " +
                                "[Tipos de cajas].pesocaja*[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos AS PesoCajas, " +
                                "[Detalle APT-Pedidos Salidas].pesobrutosaptpedidos-PesoCajas AS PesoNeto, "+
                                "IIf([Detalle APT-Pedidos Salidas].piezassaptpedidos=0, PesoNeto, PesoNeto/[Detalle APT-Pedidos Salidas].piezassaptpedidos) as PesoPromedio " +
                                "from subproductos, [Detalle APT-Pedidos Salidas], [Tipos de cajas] where [Detalle APT-Pedidos Salidas].idsaptpedidos =32095 " +
                                "AND subproductos.idsubproducto = [Detalle APT-Pedidos Salidas].idsubproductosaptpedidos AND [Tipos de cajas].idtipocaja = idtipocajasaptpedidos";*/

                            /*String query = "SELECT subproductos.idsubproducto, subproductos.nombresubproducto, [Detalle APT-Pedidos Salidas].idlotecaducidadpedidos, " +
                                "[Detalle APT-Pedidos Salidas].fechacaducidadsaptpedidos, [Detalle APT-Pedidos Salidas].piezassaptpedidos, [Tipos de cajas].descripcióntipocaja, " +
                                "[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos, [Detalle APT-Pedidos Salidas].pesobrutosaptpedidos, " +
                                "[Tipos de cajas].pesocaja*[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos AS PesoCajas, " +
                                "[Detalle APT-Pedidos Salidas].pesobrutosaptpedidos-PesoCajas AS PesoNeto, " +
                                "IIf([Detalle APT-Pedidos Salidas].piezassaptpedidos=0, PesoNeto, PesoNeto/[Detalle APT-Pedidos Salidas].piezassaptpedidos) as PesoPromedio " +
                                "from subproductos, [Detalle APT-Pedidos Salidas], [Tipos de cajas] where [Detalle APT-Pedidos Salidas].idsaptpedidos =" + idPedidoSalida + " " +
                                "AND subproductos.idsubproducto = [Detalle APT-Pedidos Salidas].idsubproductosaptpedidos AND [Tipos de cajas].idtipocaja = idtipocajasaptpedidos";





                            //cmd.Parameters.AddWithValue("@cajas", "0");
                            //poner el parametro de numero de cajas


                            //adapter = new OleDbDataAdapter(cmd.CommandText, conect);
                            adapter = new OleDbDataAdapter(query, conect);
                            //DataSet ds = new DataSet();
                            DataTable dt = new DataTable();
                            //dt = ds.Tables["subproductos"]; ;
                            adapter.Fill(dt);
                            //detalle_APT_Pedidos_SalidasDataGridView.DataSource = ds.Tables[0].Rows[1];
                            //detalle_APT_Pedidos_SalidasBindingSource.DataSource = dt;
                            //detalle_APT_Pedidos_SalidasTableAdapter.Fill(dt);


                            detalle_APT_Pedidos_SalidasDataGridView.DataSource = dt;

                            detalle_APT_Pedidos_SalidasDataGridView.Columns[0].HeaderText = "Clave";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[1].HeaderText = "Producto";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[2].HeaderText = "Lote";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[3].HeaderText = "Caducidad";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[4].HeaderText = "Piezas";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[5].HeaderText = "Tipo Caja";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[6].HeaderText = "Cajas";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[7].HeaderText = "Peso Bruto";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[8].HeaderText = "Peso Cajas";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[9].HeaderText = "Peso Neto";
                            detalle_APT_Pedidos_SalidasDataGridView.Columns[10].HeaderText = "Peso Promedio";
                            //detalle_APT_Pedidos_SalidasDataGridView.Data

                            bs = new BindingSource();
                            bs.DataSource = dt;
                            bindingNavigator1.BindingSource = bs;
                            detalle_APT_Pedidos_SalidasDataGridView.DataSource = bs;*/
                        }
                        catch (Exception)
                        {


                        }
                    }
                    else
                    {
                        MessageBox.Show("Ingrese datos válidos");
                    }
                    //reader.Close();  1111111
                    //conect.Close();   2222222222
                } 
            }
            
        }

        public void BuscarSalidaPedidosDetalle(String DidPedidoSalida)
        {
            using (OleDbConnection conect = new OleDbConnection(PedidosSalidas.cadConex))
            {
                //buscar id de la abla APT-Pedidos Salidas donde la fecha de pedido es el 3 de junio del 2017//
                //cmd.CommandText = "SELECT IdSAPTPedidos FROM [APT-Pedidos Salidas] WHERE (FechaSAPTPedidos = #6/3/2017#);";
                /*cmd.CommandText = "SELECT IdSAPTPedidos FROM [APT-Pedidos Salidas] WHERE (FechaSAPTPedidos = #6/3/2017#);";
                reader = cmd.ExecuteReader();
                reader.Read();
                String a = reader.GetValue(0).ToString();
                textBox1.Text = a;*/
                /*String query = "SELECT subproductos.idsubproducto, subproductos.nombresubproducto, [Detalle APT-Pedidos Salidas].idlotecaducidadpedidos, "+
                    "[Detalle APT-Pedidos Salidas].fechacaducidadsaptpedidos, [Detalle APT-Pedidos Salidas].piezassaptpedidos,"+
                    "[Tipos de cajas].descripcióntipocaja, [Detalle APT-Pedidos Salidas].númerodecajassaptpedidos,"+
                    "[Detalle APT-Pedidos Salidas].pesobrutosaptpedidos, Cstr([Tipos de cajas].pesocaja*[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos)," +
                    "Cstr([Detalle APT-Pedidos Salidas].pesobrutosaptpedidos-([Tipos de cajas].pesocaja*[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos)), Cstr(([Detalle APT-Pedidos Salidas].pesobrutosaptpedidos-([Tipos de cajas].pesocaja*[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos))/[Detalle APT-Pedidos Salidas].piezassaptpedidos)" +
                    "from subproductos, [Detalle APT-Pedidos Salidas], [Tipos de cajas] where [Detalle APT-Pedidos Salidas].idsaptpedidos =32095 "+
                    "AND subproductos.idsubproducto = [Detalle APT-Pedidos Salidas].idsubproductosaptpedidos AND [Tipos de cajas].idtipocaja = idtipocajasaptpedidos";*/

                /*String query = "SELECT subproductos.idsubproducto, subproductos.nombresubproducto, [Detalle APT-Pedidos Salidas].idlotecaducidadpedidos, " +
                    "[Detalle APT-Pedidos Salidas].fechacaducidadsaptpedidos, [Detalle APT-Pedidos Salidas].piezassaptpedidos, [Tipos de cajas].descripcióntipocaja, " +
                    "[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos, [Detalle APT-Pedidos Salidas].pesobrutosaptpedidos, " +
                    "[Tipos de cajas].pesocaja*[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos AS PesoCajas, " +
                    "[Detalle APT-Pedidos Salidas].pesobrutosaptpedidos-PesoCajas AS PesoNeto, "+
                    "IIf([Detalle APT-Pedidos Salidas].piezassaptpedidos=0, PesoNeto, PesoNeto/[Detalle APT-Pedidos Salidas].piezassaptpedidos) as PesoPromedio " +
                    "from subproductos, [Detalle APT-Pedidos Salidas], [Tipos de cajas] where [Detalle APT-Pedidos Salidas].idsaptpedidos =32095 " +
                    "AND subproductos.idsubproducto = [Detalle APT-Pedidos Salidas].idsubproductosaptpedidos AND [Tipos de cajas].idtipocaja = idtipocajasaptpedidos";*/

                String query = "SELECT subproductos.idsubproducto, subproductos.nombresubproducto, [Detalle APT-Pedidos Salidas].idlotecaducidadpedidos, " +
                    "[Detalle APT-Pedidos Salidas].fechacaducidadsaptpedidos, [Detalle APT-Pedidos Salidas].piezassaptpedidos, [Tipos de cajas].descripcióntipocaja, " +
                    "[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos, [Detalle APT-Pedidos Salidas].pesobrutosaptpedidos, " +
                    "[Tipos de cajas].pesocaja*[Detalle APT-Pedidos Salidas].númerodecajassaptpedidos AS PesoCajas, " +
                    "[Detalle APT-Pedidos Salidas].pesobrutosaptpedidos-PesoCajas AS PesoNeto, " +
                    "IIf([Detalle APT-Pedidos Salidas].piezassaptpedidos=0, PesoNeto, PesoNeto/[Detalle APT-Pedidos Salidas].piezassaptpedidos) as PesoPromedio " +
                    "from subproductos, [Detalle APT-Pedidos Salidas], [Tipos de cajas] where [Detalle APT-Pedidos Salidas].idsaptpedidos =" + DidPedidoSalida + " " +
                    "AND subproductos.idsubproducto = [Detalle APT-Pedidos Salidas].idsubproductosaptpedidos AND [Tipos de cajas].idtipocaja = idtipocajasaptpedidos";





                //cmd.Parameters.AddWithValue("@cajas", "0");
                //poner el parametro de numero de cajas


                //adapter = new OleDbDataAdapter(cmd.CommandText, conect);
                adapter = new OleDbDataAdapter(query, conect);
                //DataSet ds = new DataSet();
                dt = new DataTable();
                //dt = ds.Tables["subproductos"]; ;
                adapter.Fill(dt);
                //detalle_APT_Pedidos_SalidasDataGridView.DataSource = ds.Tables[0].Rows[1];
                //detalle_APT_Pedidos_SalidasBindingSource.DataSource = dt;
                //detalle_APT_Pedidos_SalidasTableAdapter.Fill(dt);


                detalle_APT_Pedidos_SalidasDataGridView.DataSource = dt;

                detalle_APT_Pedidos_SalidasDataGridView.Columns[0].HeaderText = "Clave";
                detalle_APT_Pedidos_SalidasDataGridView.Columns[1].HeaderText = "Producto";
                detalle_APT_Pedidos_SalidasDataGridView.Columns[2].HeaderText = "Lote";
                detalle_APT_Pedidos_SalidasDataGridView.Columns[3].HeaderText = "Caducidad";
                detalle_APT_Pedidos_SalidasDataGridView.Columns[4].HeaderText = "Piezas";
                //cmb = new DataGridViewComboBoxColumn();
                //cmb.Name = "Tipo Caja";


                detalle_APT_Pedidos_SalidasDataGridView.Columns[5].HeaderText = "Tipo Caja";
                detalle_APT_Pedidos_SalidasDataGridView.Columns[6].HeaderText = "Cajas";
                detalle_APT_Pedidos_SalidasDataGridView.Columns[7].HeaderText = "Peso Bruto";
                detalle_APT_Pedidos_SalidasDataGridView.Columns[8].HeaderText = "Peso Cajas";
                detalle_APT_Pedidos_SalidasDataGridView.Columns[9].HeaderText = "Peso Neto";
                detalle_APT_Pedidos_SalidasDataGridView.Columns[10].HeaderText = "Peso Promedio";
                //detalle_APT_Pedidos_SalidasDataGridView.Data
                

                bs = new BindingSource();
                bs.DataSource = dt;
                bindingNavigator1.BindingSource = bs;
                detalle_APT_Pedidos_SalidasDataGridView.DataSource = bs;

                totalpiezas = 0;
                totalpesobruto = 0;
                totalpesoneto = 0;


                foreach (DataGridViewRow fila in detalle_APT_Pedidos_SalidasDataGridView.Rows)
                {
                    int cont = detalle_APT_Pedidos_SalidasDataGridView.Rows.Count;
                    if (fila.Index == cont - 1)
                    {
                        break;
                    }
                    detalle_APT_Pedidos_SalidasDataGridView.CurrentCell = detalle_APT_Pedidos_SalidasDataGridView.Rows[fila.Index].Cells[0];
                    Double piezasacum = Convert.ToDouble(detalle_APT_Pedidos_SalidasDataGridView.Rows[fila.Index].Cells[4].Value);
                    totalpiezas = totalpiezas + piezasacum;

                    Double pesobrutoacum = Convert.ToDouble(detalle_APT_Pedidos_SalidasDataGridView.Rows[fila.Index].Cells[7].Value);
                    totalpesobruto = totalpesobruto + pesobrutoacum;

                    Double pesonetoacum = Convert.ToDouble(detalle_APT_Pedidos_SalidasDataGridView.Rows[fila.Index].Cells[9].Value);
                    totalpesoneto = totalpesoneto + pesonetoacum;
                }
                textBox3.Text = totalpiezas.ToString();
                textBox5.Text = totalpesobruto.ToString();
                textBox7.Text = totalpesoneto.ToString();


            }
            
        }

        public void BuscarSalidaPedidosEncabezado(String EidPedidoSalida)
        {
            try
            {
                using (OleDbConnection conect = new OleDbConnection(PedidosSalidas.cadConex))
                {
                    cmd = conect.CreateCommand();
                    cmd.CommandText = "SELECT [APT-Pedidos Salidas].fechasaptpedidos, [APT-Pedidos Salidas].usuarioentregósaptpedidos, [APT-Pedidos Salidas].usuariorecibiósaptpedidos " +
                    "from [APT-Pedidos Salidas] where [APT-Pedidos Salidas].idsaptpedidos =" + EidPedidoSalida + ";";
                    conect.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        textBox2.Text = Convert.ToString(reader[0]);
                        comboBox1.Text = Convert.ToString(reader[1]);
                        comboBox2.Text = Convert.ToString(reader[2]);
                        bandera = 1;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                throw;
            }                      
       }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (OleDbConnection conect = new OleDbConnection(PedidosSalidas.cadConex))
                {
                    conect.Open();
                    cmd = conect.CreateCommand();
                    //detalle_APT_Pedidos_SalidasDataGridView.Columns.Remove(detalle_APT_Pedidos_SalidasDataGridView.Columns[5]);
                    button2.Enabled = true;
                    textBox2.Text = "";
                    comboBox1.Text = "[Selecciona]";
                    comboBox2.Text = "[Selecciona]";
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    textBox3.Text = "0";
                    textBox5.Text = "0.00";
                    textBox7.Text = "0.00";
                    //detalle_APT_Pedidos_SalidasDataGridView.Columns[1].ReadOnly = true;

                    DateTime hoy = DateTime.Today;

                    String fechahoy = hoy.ToShortDateString();
                    textBox2.Text = fechahoy;
                    textBox1.Text = ObtenerClaveNueva().ToString();
                    //reader.Close();   3333333333333

                    //DataGridViewComboBoxColumn comboboxTipoCaja = new DataGridViewComboBoxColumn();

                    //comboboxTipoCaja.Name = "Tipo Caja";
                    //comboboxTipoCaja.Items.Add(obtenerTiposCajas());
                    //detalle_APT_Pedidos_SalidasDataGridView.Columns.Add(comboboxTipoCaja);

                    //if (detalle_APT_Pedidos_SalidasDataGridView.Columns[5].HeaderText == "Tipo Caja")
                    //{
                    //    comboboxTipoCaja.Items.Add(obtenerTiposCajas());
                    //}
                    //if(comboboxTipoCaja.HeaderText == "Tipos Caja"){
                    //    //comboboxTipoCaja.DataSource = obtenerTiposCajas();
                    //    comboboxTipoCaja.Items.Add(obtenerTiposCajas());
                    //}



                    //textbox1.Text = ObtenerClaveNueva();
                    //Obtener la clave y sumar 1
                    //cmd.CommandText = "SELECT TOP 1 IdSAPTPedidos FROM [APT-Pedidos Salidas] ORDER BY IdSAPTPedidos desc;";
                    //reader = cmd.ExecuteReader();
                    //reader.Read();
                    //int val = Convert.ToInt32(reader.GetValue(0));
                    //String a = (val + 1).ToString();
                    //textBox1.Text = a;
                    reader.Close();    //44444444444444444444444                

                    dt.Clear();


                    detalle_APT_Pedidos_SalidasDataGridView.ReadOnly = false;
                    //detalle_APT_Pedidos_SalidasDataGridView.Enabled = true;

                    cmd.CommandText = "SELECT sysvusuario.nombreusuario from sysvusuario";
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(Convert.ToString(reader.GetValue(0)));
                    }

                    //conect.Close();     55555555555555555555555555555555
                    //int col = detalle_APT_Pedidos_SalidasDataGridView.ColumnCount;
                    //int fila = detalle_APT_Pedidos_SalidasDataGridView.RowCount;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: "+ ex.Message);
                throw;
            }         
        }

        public int ObtenerClaveNueva()
        {
            try
            {
                cmd.CommandText = "SELECT TOP 1 IdSAPTPedidos FROM [APT-Pedidos Salidas] ORDER BY IdSAPTPedidos desc;";
                reader = cmd.ExecuteReader();
                reader.Read();
                int val = Convert.ToInt32(reader.GetValue(0));
                //String a = (val + 1).ToString();
                val = val + 1;
                return val;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: "+ e.Message);
                throw;
            }
            
        }

        public List<String> obtenerTiposCajas()
        {
            try
            {
                using (OleDbConnection conect = new OleDbConnection(PedidosSalidas.cadConex))
                {
                    cmd = conect.CreateCommand();
                    cmd.CommandText = "SELECT [Tipos de Cajas].descripcióntipocaja, [Tipos de Cajas].pesocaja from [Tipos de Cajas]";
                    List<String> lista = new List<String>();

                    conect.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(reader.GetValue(0) + " " + reader.GetValue(1));
                    }

                    return lista;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: "+ e.Message);
                throw;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (OleDbConnection conect = new OleDbConnection(PedidosSalidas.cadConex))
                {
                    if (comboBox1.Text == "" || comboBox1.Text == "[Selecciona]" || comboBox2.Text == "" || comboBox2.Text == "[Selecciona]")
                    {
                        MessageBox.Show("Ingrese datos válidos en el encabezado");
                    }
                    else
                    {
                        Boolean EGrid = celdasNullEnDataGridView();
                        if (EGrid == false)
                        {
                            guardarSalidaPedido(conect);
                            DialogResult dialog = MessageBox.Show("Desea guardar?", "Guardar", MessageBoxButtons.YesNo);
                            //using (OleDbConnection conect = new OleDbConnection(PedidosSalidas.cadConex))
                            //{
                            if (dialog == DialogResult.Yes)
                            {
                                //conect.Open();
                                //conect.CreateCommand();
                                cmd = new OleDbCommand("COMMIT", conect);
                                //cmd.CommandText = "COMMIT";
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Insertado con éxito");
                                comboBox1.Enabled = false;
                                comboBox2.Enabled = false;
                                button2.Enabled = false;
                                //detalle_APT_Pedidos_SalidasDataGridView.Enabled = false;
                                detalle_APT_Pedidos_SalidasDataGridView.ReadOnly = true;


                                //foreach (DataGridViewRow fila in detalle_APT_Pedidos_SalidasDataGridView.Rows)
                                //{
                                //    String clave = fila.Cells[0].Value.ToString().Trim();
                                //    //String producto = fila.Cells[1].Value.ToString().Trim();
                                //    String lote = fila.Cells[2].Value.ToString().Trim();
                                //    String caducidad = fila.Cells[3].Value.ToString().Trim();
                                //    int piezas = Convert.ToInt32(fila.Cells[4].Value);
                                //    int tipocaja = Convert.ToInt32(fila.Cells[5].Value);
                                //    int cajas = Convert.ToInt32(fila.Cells[5].Value);
                                //    Double pesobruto = Convert.ToDouble(fila.Cells[6].Value);
                                //    //Double pesocajas;
                                //    //Double pesoneto;
                                //    //Double pesopromedio;
                                //}

                            }
                            else if (dialog == DialogResult.No)
                            {
                                cmd = new OleDbCommand("ROLLBACK", conect);
                                //cmd.CommandText = "ROLLBACK";
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ingrese datos válidos en la grid");
                        }
                    }
                        //}

                
                
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: "+ ex.Message);                
            }   
        }        

        public void guardarSalidaPedido(OleDbConnection conect)
        {
            //using (OleDbConnection conect = new OleDbConnection(PedidosSalidas.cadConex))
            //{
                try
                {
                    conect.Open();
                    cmd = new OleDbCommand("BEGIN TRANSACTION", conect);
                    cmd.ExecuteNonQuery();

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

                    int idencabezado = Convert.ToInt32(textBox1.Text.Trim());
                    String FechaSAPTPedidos = textBox2.Text.Trim();
                    String UsuarioEntregoSAPTPedidos = comboBox1.Text.Trim();
                    String UsuarioRecibioSAPTPedidos = comboBox2.Text.Trim();



                    ////EDITANDO AQUI/////
                    //Regex Val = new Regex(@"a-zA-ZñÑ\s{2,50}");
                    //if (Val.IsMatch(UsuarioEntregoSAPTPedidos) && Val.IsMatch(UsuarioRecibioSAPTPedidos))
                    //{

                        cmd.CommandText = "insert into [APT-Pedidos Salidas](idsaptpedidos, FechaSAPTPedidos, usuariorecibiósaptpedidos, usuarioentregósaptpedidos) values (@claveencabezado, @fechaencabezado, @usuariorecibio, @usuarioentrego)";
                        //cmd.CommandText = "SELECT Nombre, ApellidoPaterno FROM Agentes WHERE (((IdAgente)=[@name]));";
                        cmd.Parameters.AddWithValue("@claveencabezado", idencabezado);
                        cmd.Parameters.AddWithValue("@fechaencabezado", FechaSAPTPedidos);
                        cmd.Parameters.AddWithValue("@usuariorecibio", UsuarioRecibioSAPTPedidos);
                        cmd.Parameters.AddWithValue("@usuarioentrego", UsuarioEntregoSAPTPedidos);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    //}
                    //else
                    //{
                      //  MessageBox.Show("Ingrese datos validos en el encabezado");
                    //}
                    

                    foreach (DataGridViewRow fila in detalle_APT_Pedidos_SalidasDataGridView.Rows)
                    {
                        int cont = detalle_APT_Pedidos_SalidasDataGridView.Rows.Count;
                        if (fila.Index == cont - 1)
                        {
                            break;
                        }
                        int IdTipoCajaSAPTPedidos = 0;
                        int fila2 = fila.Index;
                        int col = detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.ColumnIndex;

                        //DataGridViewTextBoxCell dpbox2 = (DataGridViewTextBoxCell)detalle_APT_Pedidos_SalidasDataGridView[col, fila2];




                        String IdSubProductoSAPTPedidos = fila.Cells[0].Value.ToString().Trim();
                        //String producto = fila.Cells[1].Value.ToString().Trim();
                        String IdLoteCaducidadPedidos = fila.Cells[2].Value.ToString().Trim();

                        String FechaCad = fila.Cells[3].Value.ToString().Trim();
                        String FechaCaducidadSAPTPedidos = FechaCad.Split(' ')[0];
                        int PiezasSAPTPedidos = Convert.ToInt32(fila.Cells[4].Value.ToString().Trim());
                        String TipoCaja = Convert.ToString(detalle_APT_Pedidos_SalidasDataGridView.Rows[fila2].Cells[5].Value).Trim();

                        IdTipoCajaSAPTPedidos = EvaluarIdTipoCaja(TipoCaja);


                        //MessageBox.Show("HOLA " + IdTipoCajaSAPTPedidos);
                        int NumeroDeCajasSAPTPedidos = Convert.ToInt32(fila.Cells[6].Value.ToString().Trim());
                        Double pesobruto = Convert.ToDouble(fila.Cells[7].Value.ToString().Trim());


                        

                        cmd.CommandText = "insert into [Detalle APT-Pedidos Salidas](idsaptpedidos, idsubproductosaptpedidos, pesobrutosaptpedidos, idtipocajasaptpedidos, " + ""
                        + "númerodecajassaptpedidos, piezassaptpedidos, idlotecaducidadpedidos, fechacaducidadsaptpedidos) values(@detalleclave, @detalleclavepro, @detallepesobruto, " + ""
                        + "@detalleidcaja, @detallenumcaja, @detallepiezas, @detalleidlote, @detallefechacad)";
                        cmd.Parameters.AddWithValue("@detalleclave", idencabezado);
                        cmd.Parameters.AddWithValue("@detalleclavepro", IdSubProductoSAPTPedidos);
                        cmd.Parameters.AddWithValue("@detallepesobruto", pesobruto);
                        cmd.Parameters.AddWithValue("@detalleidcaja", IdTipoCajaSAPTPedidos);
                        cmd.Parameters.AddWithValue("@detallenumcaja", NumeroDeCajasSAPTPedidos);
                        cmd.Parameters.AddWithValue("@detallepiezas", PiezasSAPTPedidos);
                        cmd.Parameters.AddWithValue("@detalleidlote", IdLoteCaducidadPedidos);
                        cmd.Parameters.AddWithValue("@detallefechacad", FechaCaducidadSAPTPedidos);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        cmd.CommandText = "update [detalle existencias apt] set salidaspedidoskg = salidaspedidoskg + @pesobruto where " +
                            "idexistenciaapt = @idExist AND idsubproducto = @idsubprod";
                        cmd.Parameters.AddWithValue("@pesobruto",pesobruto);
                        cmd.Parameters.AddWithValue("@idExist", idExistencia);
                        cmd.Parameters.AddWithValue("@idsubprod", IdSubProductoSAPTPedidos);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        /*cmd.CommandText = "update [detalle existencias apt] set salidaspedidoskg = salidaspedidoskg + " + pesobruto + " where "+
                            "idexistenciaapt = " + idExistencia + " AND idsubproducto = "+ IdSubProductoSAPTPedidos;
                        cmd.ExecuteNonQuery();*/
                        
                        

                        //cmd.CommandText = "UPDATE "


                        //MessageBox.Show("Id Tipo Caja de la fila "+IdTipoCajaSAPTPedidos);
                        //Double pesocajas;
                        //Double pesoneto;
                        //Double pesopromedio;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Existe un error con esta clave");
                    throw;

                }
            //}
        }


        private bool celdasNullEnDataGridView()
        {
            bool bVacia = false;
            if (detalle_APT_Pedidos_SalidasDataGridView.Rows.Count > 1)
            {
                foreach (DataGridViewRow row in detalle_APT_Pedidos_SalidasDataGridView.Rows)
                {
                    int cont = detalle_APT_Pedidos_SalidasDataGridView.Rows.Count;
                    if (row.Index == cont - 1)
                    {
                        break;
                    }
                    if
                        (string.IsNullOrEmpty(row.Cells[0].FormattedValue.ToString()) ||
                         string.IsNullOrEmpty(row.Cells[1].FormattedValue.ToString()) ||
                         string.IsNullOrEmpty(row.Cells[2].FormattedValue.ToString()) ||
                         string.IsNullOrEmpty(row.Cells[3].FormattedValue.ToString()) ||
                         string.IsNullOrEmpty(row.Cells[4].FormattedValue.ToString()) ||
                         string.IsNullOrEmpty(row.Cells[5].FormattedValue.ToString()) ||
                         string.IsNullOrEmpty(row.Cells[6].FormattedValue.ToString()) ||
                         string.IsNullOrEmpty(row.Cells[7].FormattedValue.ToString()) ||
                         string.IsNullOrEmpty(row.Cells[8].FormattedValue.ToString()) ||
                         string.IsNullOrEmpty(row.Cells[9].FormattedValue.ToString()) ||
                         string.IsNullOrEmpty(row.Cells[10].FormattedValue.ToString()))
                    { bVacia = true; }
                }
            }
            else
            {
                bVacia = true;
            }

            return bVacia;
        }

        public int EvaluarIdTipoCaja(String TipoCaja)
        {
            int idcaja=0;
            if (TipoCaja.Contains("Sin caja")){
                idcaja = 1;
            }else if(TipoCaja.Contains("Chica pvc")){
                idcaja = 2;
            }else if (TipoCaja.Contains("Grande pvc")){
                idcaja = 3;
            }else if(TipoCaja.Contains("Almac.chica pvc")){
                idcaja = 4;
            }else if(TipoCaja.Contains("Almac.grande pvc")){
                idcaja = 5;
            }else if(TipoCaja.Contains("Bote p/vinagre")){
                idcaja = 6;
            }else if(TipoCaja.Contains("~Garrafa p/vinagre")){
                idcaja = 7;
            }
            return idcaja;
        }

        private void detalle_APT_Pedidos_SalidasDataGridView_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*if (e.KeyChar.Equals(Keys.Enter))
            {
                e.Handled = true;
                int i = detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.ColumnIndex + 1;
                //detalle_APT_Pedidos_SalidasDataGridView.CurrentCell = i;

                    //detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.ColumnIndex + 1;
                //detalle_APT_Pedidos_SalidasDataGridView.CurrentCell = i;
            }*/
            //if (e.KeyChar == Keys.Enter)
            //{
                   
            //}
        }

        private void detalle_APT_Pedidos_SalidasDataGridView_KeyDown(object sender, KeyEventArgs e)
        
        {
            /*if (detalle_APT_Pedidos_SalidasDataGridView.)
            {
                detalle_APT_Pedidos_SalidasDataGridView.CurrentCell
            }*/



            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                
                    //SendKeys.Send("{RIGHT}");
                    //SendKeys.Send("{UP}");

                  //e.SuppressKeyPress = false;

                    if (detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.ColumnIndex == 10)
                    {
                        ///EDITANDO
                        totalpiezas = 0;
                        totalpesobruto = 0;
                        totalpesoneto = 0;
                        //int cont = 0;
                        foreach (DataGridViewRow item in detalle_APT_Pedidos_SalidasDataGridView.Rows)
                        {
                            int con = detalle_APT_Pedidos_SalidasDataGridView.Rows.Count;
                            if (item.Index == con - 1)
                            {
                                break;
                            }
                        
                            detalle_APT_Pedidos_SalidasDataGridView.CurrentCell = detalle_APT_Pedidos_SalidasDataGridView.Rows[item.Index].Cells[0];
                            Double piezasacum = Convert.ToDouble(detalle_APT_Pedidos_SalidasDataGridView.Rows[item.Index].Cells[4].Value);
                            totalpiezas = totalpiezas + piezasacum;

                            Double pesobrutoacum = Convert.ToDouble(detalle_APT_Pedidos_SalidasDataGridView.Rows[item.Index].Cells[7].Value);
                            totalpesobruto = totalpesobruto + pesobrutoacum;

                            Double pesonetoacum = Convert.ToDouble(detalle_APT_Pedidos_SalidasDataGridView.Rows[item.Index].Cells[9].Value);
                            totalpesoneto = totalpesoneto + pesonetoacum;

                        


                        }
                        textBox3.Text = totalpiezas.ToString();
                        textBox5.Text = totalpesobruto.ToString();
                        textBox7.Text = totalpesoneto.ToString();
                    
                    
                    
                    }
                    SendKeys.Send("{DOWN}");
                    SendKeys.Send("{LEFT}");

                
                
                   //if(detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.ColumnIndex == 10)
                   //{
                   //    int fila = detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.RowIndex;                   
                   //    detalle_APT_Pedidos_SalidasDataGridView.CurrentCell = detalle_APT_Pedidos_SalidasDataGridView.Rows[fila].Cells[0];
                   //    int piezasacum = Convert.ToInt32(detalle_APT_Pedidos_SalidasDataGridView.Rows[fila].Cells[4].Value);
                   //    totalpiezas = totalpiezas + piezasacum;
                   
                   //    int pesobrutoacum = Convert.ToInt32(detalle_APT_Pedidos_SalidasDataGridView.Rows[fila].Cells[7].Value);
                   //    totalpesobruto = totalpesobruto + pesobrutoacum;
                   
                   //    int pesonetoacum = Convert.ToInt32(detalle_APT_Pedidos_SalidasDataGridView.Rows[fila].Cells[9].Value);
                   //    totalpesoneto = totalpesoneto + pesonetoacum;

                   
                   

                   //    textBox3.Text = totalpiezas.ToString();
                   //    textBox5.Text = totalpesobruto.ToString();
                   //    textBox7.Text = totalpesoneto.ToString();
                   //    totalpiezas = 0;
                   //    totalpesobruto = 0;
                   //    totalpesoneto = 0;
                   //    SendKeys.Send("{DOWN}");
                   //    SendKeys.Send("{LEFT}");

                   //}
                
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: "+ex.Message);
                throw;
            }

            
            
            

            //detalle_APT_Pedidos_SalidasDataGridView.CurrentCell = data

            /*int col = detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.ColumnIndex;
            int fila = detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.RowIndex;

            if (col < detalle_APT_Pedidos_SalidasDataGridView.ColumnCount - 1)
            {
                col++;
            }
            else
            {
                col = 0;
                fila++;
            }
            if (fila==detalle_APT_Pedidos_SalidasDataGridView.RowCount)
            {
                dt.NewRow();
                detalle_APT_Pedidos_SalidasDataGridView.DataSource = dt;
                //detalle_APT_Pedidos_SalidasDataGridView.Rows.Add();
            }
            //detalle_APT_Pedidos_SalidasDataGridView.CurrentCell = detalle_APT_Pedidos_SalidasDataGridView[col, fila];
            e.Handled = true;*/

           /* e.SuppressKeyPress = true;
            int iColumn = detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.ColumnIndex;
            int iRow = detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.RowIndex;
            if (iColumn == detalle_APT_Pedidos_SalidasDataGridView.ColumnCount - 1)
            {
                if (detalle_APT_Pedidos_SalidasDataGridView.RowCount > (iRow + 1))
                {
                    detalle_APT_Pedidos_SalidasDataGridView.CurrentCell = detalle_APT_Pedidos_SalidasDataGridView[1, iRow + 1];
                }
                else
                {
                    //focus next control
                }
            }
            else
                detalle_APT_Pedidos_SalidasDataGridView.CurrentCell = detalle_APT_Pedidos_SalidasDataGridView[iColumn + 1, iRow];*/

            /*if (e.KeyCode == Keys.Enter && detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.ColumnIndex == 1)
            {
                e.Handled = true;
                DataGridViewCell cell = detalle_APT_Pedidos_SalidasDataGridView.Rows[0].Cells[0];
                detalle_APT_Pedidos_SalidasDataGridView.CurrentCell = cell;
                detalle_APT_Pedidos_SalidasDataGridView.BeginEdit(true);
            }*/
            


        }

        private void detalle_APT_Pedidos_SalidasDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                detalle_APT_Pedidos_SalidasDataGridView.Columns[1].ReadOnly = true;

                using (OleDbConnection conect = new OleDbConnection(PedidosSalidas.cadConex))
                {
                    SendKeys.Send("{UP}");
                    SendKeys.Send("{RIGHT}");

                    if (detalle_APT_Pedidos_SalidasDataGridView.Columns[e.ColumnIndex].HeaderText == "Clave")
                    {
                        //MessageBox.Show("Si es");
                        int col = detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.ColumnIndex;
                        //int fila = detalle_APT_Pedidos_SalidasDataGridView.c

                        String iddetalle = detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.Value.ToString().Trim();
                        Regex Val = new Regex(@"([a-zA-Z][a-zA-Z]\-\d{2,3}\-\d{2})$");
                        if (Val.IsMatch(iddetalle))
                        {

                            //buscarDetalle(iddetalle);
                            //detalle_APT_Pedidos_SalidasDataGridView.Rows[detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.RowIndex].Cells[col + 1].Value = iddetalle;
                            detalle_APT_Pedidos_SalidasDataGridView.Rows[detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.RowIndex].Cells[col + 1].Value = buscarDetalle(iddetalle);
                            //MessageBox.Show("valor:"+ iddetalle);
                            //conect.Close();          7777777777777777777777777777777777
                            //SendKeys.Send("{UP}");
                            SendKeys.Send("{RIGHT}");
                        }
                        else
                        {
                            MessageBox.Show("Ingrese una clave válida");
                            SendKeys.Send("{UP}");
                        }
                    }

                    if (detalle_APT_Pedidos_SalidasDataGridView.Columns[e.ColumnIndex].HeaderText == "Lote")
                    {
                        String lote = detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.Value.ToString().Trim();
                        Regex Val = new Regex(@"^[0-9a-zA-Z]+$");
                        if (Val.IsMatch(lote))
                        {
                            //SendKeys.Send("{RIGHT}");
                        }
                        else
                        {
                            MessageBox.Show("Ingrese un lote válido");
                            SendKeys.Send("{UP}");
                        }
                    }

                    if (detalle_APT_Pedidos_SalidasDataGridView.Columns[e.ColumnIndex].HeaderText == "Caducidad")
                    {
                        //DateTime dateValue;
                        String fcaducidad = detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.Value.ToString().Trim();
                        if (fcaducidad != "")
                        {

                        }
                        else
                        {
                            MessageBox.Show("Ingrese una formato válido");
                            SendKeys.Send("{UP}");
                        }
                    }

                    if (detalle_APT_Pedidos_SalidasDataGridView.Columns[e.ColumnIndex].HeaderText == "Piezas")
                    {
                        String pz = detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.Value.ToString().Trim();
                        Regex Val = new Regex(@"^\d+$");
                        if (Val.IsMatch(pz))
                        {

                        }
                        else
                        {
                            MessageBox.Show("Ingrese un número de piezas válido");
                            SendKeys.Send("{UP}");
                        }
                    }

                    if (detalle_APT_Pedidos_SalidasDataGridView.Columns[e.ColumnIndex].HeaderText == "Cajas")
                    {
                        String cajas = detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.Value.ToString().Trim();
                        Regex Val = new Regex(@"^\d+$");
                        if (Val.IsMatch(cajas))
                        {

                        }
                        else
                        {
                            MessageBox.Show("Ingrese un número de cajas válido");
                            SendKeys.Send("{UP}");
                        }
                    }

                    if (detalle_APT_Pedidos_SalidasDataGridView.Columns[e.ColumnIndex].HeaderText == "Peso Bruto")
                    {
                        String pesobruto = detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.Value.ToString().Trim();
                        Regex Val = new Regex(@"^[0-9]+\.?[0-9]*$");
                        if (Val.IsMatch(pesobruto))
                        {
                            Double piezas = Convert.ToDouble(detalle_APT_Pedidos_SalidasDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex - 3].Value);
                            Double pesoneto = Convert.ToDouble(detalle_APT_Pedidos_SalidasDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                            detalle_APT_Pedidos_SalidasDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value = 0;
                            detalle_APT_Pedidos_SalidasDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].Value = pesoneto;
                            SendKeys.Send("{RIGHT}");
                            SendKeys.Send("{RIGHT}");


                            if (piezas == 0)
                            {
                                detalle_APT_Pedidos_SalidasDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex + 3].Value = pesoneto;
                                SendKeys.Send("{RIGHT}");

                            }
                            else if (piezas != 0)
                            {
                                Double pesopromedio = pesoneto / piezas;
                                detalle_APT_Pedidos_SalidasDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex + 3].Value = pesopromedio;
                                SendKeys.Send("{RIGHT}");

                            }
                        }
                        else
                        {
                            MessageBox.Show("Ingrese un peso bruto válido");
                            SendKeys.Send("{UP}");
                        }

                    }                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                SendKeys.Send("{UP}");
                //SendKeys.Send("{LEFT}");
                //throw;
            }
            
        }

        ////////DATAERROR/////////
        public void detalle_APT_Pedidos_SalidasDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {

            //MessageBox.Show("Error ocurrido: " + anError.Context);
            String error = anError.Context.ToString();

            if (error.Contains("Commit") || error.Contains("Parsing") || error.Contains("CurrentCellChange"))
            {
                MessageBox.Show("Ingrese un formato válido");
            }

            if (anError.Context == DataGridViewDataErrorContexts.Commit)
            {
                MessageBox.Show("Commit error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {
                MessageBox.Show("Cell change");
            }
            if (anError.Context == DataGridViewDataErrorContexts.Parsing)
            {
                MessageBox.Show("Ingrese un formato válido");
            }
            if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
            {
                MessageBox.Show("leave control error");
            }

            if ((anError.Exception) is ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[anError.RowIndex].ErrorText = "an error";
                view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";

                anError.ThrowException = false;
            }
        }

        public String buscarDetalle(String idDetalle)
        {
            try
            {
                using (OleDbConnection conect = new OleDbConnection(PedidosSalidas.cadConex))
                {
                    cmd = conect.CreateCommand();
                    //cmd.CommandText = "SELECT subproductos.nombresubproducto from subproductos where idsubproducto = " + '%' + idDetalle + '%' + ";";
                    cmd.CommandText = "SELECT subproductos.nombresubproducto from subproductos where idsubproducto = @id";
                    cmd.Parameters.AddWithValue("@id", idDetalle);
                    conect.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    String val = reader.GetValue(0).ToString();
                    //conect.Close();
                    //reader.Close();        88888888888888888888888888888888888888
                    cmd.Parameters.Clear();

                    return val;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                throw;
            }            
            
        }

        private void detalle_APT_Pedidos_SalidasDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            String valor = Convert.ToString(detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.RowIndex);
            String valor2 = Convert.ToString(detalle_APT_Pedidos_SalidasDataGridView.CurrentCell.ColumnIndex);
            //MessageBox.Show(valor + ", "+ valor2);
//            if (e.ColumnIndex > -1)
//            {
//                DataGridViewComboBoxCell dpbox = new DataGridViewComboBoxCell();

//                if (detalle_APT_Pedidos_SalidasDataGridView.Columns[e.ColumnIndex].Name.Contains("descripcióntipocaja"))
//                {
//                    detalle_APT_Pedidos_SalidasDataGridView[e.ColumnIndex, e.RowIndex] = dpbox;
//                    dpbox.DataSource = obtenerTiposCajas();
////                    dpbox.ValueMember = "";

//                }

//            }
        }

        private void detalle_APT_Pedidos_SalidasDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                detalle_APT_Pedidos_SalidasDataGridView.Columns[1].ReadOnly = true;
                using (OleDbConnection conect = new OleDbConnection(PedidosSalidas.cadConex))
                {
                    if (e.ColumnIndex > -1)
                    {
                        dpbox = new DataGridViewComboBoxCell();

                        if (detalle_APT_Pedidos_SalidasDataGridView.Columns[e.ColumnIndex].Name.Contains("descripcióntipocaja"))
                        {
                            detalle_APT_Pedidos_SalidasDataGridView[e.ColumnIndex, e.RowIndex] = dpbox;
                            dpbox.DataSource = obtenerTiposCajas();
                            //                    dpbox.ValueMember = "";

                        }

                    }
                    //conect.Close();         99999999999999999999999999999999999999
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                throw;
            }            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Inicio frmInicio = new Inicio();
            frmInicio.Show();
            this.Close();
        }

       
    }
}
