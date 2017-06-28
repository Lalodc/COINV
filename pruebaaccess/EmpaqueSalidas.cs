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
    public partial class EmpaqueSalidas : Form
    {
        //VARIABLES GLOBALES//
        public static String cadConex = ConfigurationManager.ConnectionStrings["conAccess"].ConnectionString;

        OleDbCommand cmd = new OleDbCommand();
        OleDbDataReader reader;
        OleDbDataAdapter adapter;
        BindingSource bs;
        DataTable dt;

        DataGridViewComboBoxCell dpbox;

        Double totalpiezas;
        Double totalpesobruto;
        Double totalpesoneto;
        Double totalcajas;
        Double totalpesocajas;
        int bandera;

        public EmpaqueSalidas()
        {
            InitializeComponent();
        }

        private void dataGridView1BindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            //VALIDAR//
            this.Validate();
        }

        private void EmpaqueSalidas_Load(object sender, EventArgs e)
        {
            //CONTROLAR ERRORES//
            //this.dataGridView1.DataError += new DataGridViewDataErrorEventHandler(dataGridView1_DataError);

            //BUSCAR LA ÚLTIMA SALIDA AL ACCEDER AL FORMULARIO//
            using (OleDbConnection conect = new OleDbConnection(EmpaqueSalidas.cadConex))
            {

                try
                {
                    conect.Open();
                    cmd = conect.CreateCommand();
                    //BUSCA LA ÚLTIMA SALIDA//
                    cmd.CommandText = "SELECT TOP 1 IdSAPT FROM [APT-Empaque Salidas] ORDER BY IdSAPT desc;";
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    String idEmpaqueSalida = Convert.ToString(reader.GetValue(0));
                    textBox1.Text = idEmpaqueSalida;

                    bandera = 0;

                    BuscarSalidaEmpaqueEncabezado(idEmpaqueSalida);
                    BuscarSalidaEmpaqueDetalle(idEmpaqueSalida);

                    if (bandera == 0)
                    {
                        textBox2.Text = "";
                        comboBox1.Text = "";
                        comboBox2.Text = "";
                    }
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                    textBox5.Enabled = false;
                    textBox6.Enabled = false;
                    textBox7.Enabled = false;

                    dataGridView1.ReadOnly = true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    throw;

                }
            }
        }


        //REALIZA LA BUSQUEDA DE LAS SALIDAS A EMPAQUE AL PRESIONAR ENTER//
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            using (OleDbConnection conect = new OleDbConnection(EmpaqueSalidas.cadConex))
            {
                if (e.KeyChar == Convert.ToChar(Keys.Enter))
                {
                    button2.Enabled = false;
                    Regex Val = new Regex(@"^[+-]?\d+(\\d+)?$");
                    if (Val.IsMatch(textBox1.Text))
                    {
                        try
                        {
                            bandera = 0;
                            String idEmpaqueSalida = textBox1.Text;

                            BuscarSalidaEmpaqueEncabezado(idEmpaqueSalida);
                            BuscarSalidaEmpaqueDetalle(idEmpaqueSalida);

                            if (bandera == 0)
                            {
                                textBox2.Text = "";
                                comboBox1.Text = "";
                                comboBox2.Text = "";
                            }
                            comboBox1.Enabled = false;
                            comboBox2.Enabled = false;
                            textBox3.Enabled = false;
                            textBox4.Enabled = false;
                            textBox5.Enabled = false;
                            textBox6.Enabled = false;
                            textBox7.Enabled = false;

                            dataGridView1.ReadOnly = true;

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                            throw;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ingrese datos válidos");
                    }
                }
            }
        }


        //BUSCAR ENCABEZADO DE LA SALIDA//
        public void BuscarSalidaEmpaqueEncabezado(String EidEmpaqueSalida)
        {
            try
            {
                using (OleDbConnection conect = new OleDbConnection(EmpaqueSalidas.cadConex))
                {

                    cmd = conect.CreateCommand();
                    //BUSCA LA SALIDA SEGUN LA CLAVE INGRESADA EN EL TEXTBOX//
                    cmd.CommandText = "SELECT [APT-Empaque Salidas].fechaSAPT, [APT-Empaque Salidas].usuarioentregósapt, [APT-Empaque Salidas].usuariorecibiósapt "
                    +"from [APT-Empaque Salidas] where [APT-Empaque Salidas].idsapt =" + EidEmpaqueSalida + ";";
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
                MessageBox.Show("error " + e.Message);
                throw;
            }
        }


        //BUSCAR DETALLE DE LA SALIDA//
        public void BuscarSalidaEmpaqueDetalle(String DidEmpaqueSalida)
        {
            try
            {
                using (OleDbConnection conect = new OleDbConnection(EmpaqueSalidas.cadConex))
                {
                    //buscar id de la abla APT-Empaque Salidas donde la fecha de pedido es el 3 de junio del 2017//
                    //BUSCAR EL DETALLE DE LA SALIDAS PARA MOSTRARLO EN EL GRID//

                    String query = "SELECT subproductos.idsubproducto, subproductos.nombresubproducto, [Detalle APT-Empaque Salidas].idlotecaducidad, "
                        + "[Detalle APT-Empaque Salidas].fechacaducidadSAPT, [Detalle APT-Empaque Salidas].piezasSAPT, [Tipos de cajas].descripcióntipocaja, "
                        + "[Detalle APT-Empaque Salidas].númerodecajasSAPT, [Detalle APT-Empaque Salidas].pesobrutoSAPT, "
                        + "[Tipos de cajas].pesocaja*[Detalle APT-Empaque Salidas].númerodecajasSAPT AS PesoCajas, "
                        + "[Detalle APT-Empaque Salidas].pesobrutoSAPT-PesoCajas AS PesoNeto, "
                        + "IIf([Detalle APT-Empaque Salidas].piezasSAPT=0, PesoNeto, PesoNeto/[Detalle APT-Empaque Salidas].piezasSAPT) as PesoPromedio "
                        + "from subproductos, [Detalle APT-Empaque Salidas], [Tipos de cajas] where [Detalle APT-Empaque Salidas].idSAPT = " + DidEmpaqueSalida + " "
                        + "AND subproductos.idsubproducto = [Detalle APT-Empaque Salidas].idsubproductoSAPT AND [Tipos de cajas].idtipocaja = [Detalle APT-Empaque Salidas].idtipocajaSAPT";                  
                    
                    adapter = new OleDbDataAdapter(query, conect);
                    dt = new DataTable();
                    //LLENA EL ADAPTADOR CON LOS RESULTADOS DE LA CONSULTA
                    adapter.Fill(dt);

                    //LLENA EL GRID CON LOS RESULTADOS DE LA COLUMNA//
                    dataGridView1.DataSource = dt;

                    //ASIGNA NOMBRE A LOS ENCABEZADOS DE CADA COLUMNA//
                    dataGridView1.Columns[0].HeaderText = "Clave";
                    dataGridView1.Columns[1].HeaderText = "Producto";
                    dataGridView1.Columns[2].HeaderText = "Lote";
                    dataGridView1.Columns[3].HeaderText = "Caducidad";
                    dataGridView1.Columns[4].HeaderText = "Piezas";
                    dataGridView1.Columns[5].HeaderText = "Tipo Caja";
                    dataGridView1.Columns[6].HeaderText = "Cajas";
                    dataGridView1.Columns[7].HeaderText = "Peso Bruto";
                    dataGridView1.Columns[8].HeaderText = "Peso Cajas";
                    dataGridView1.Columns[9].HeaderText = "Peso Neto";
                    dataGridView1.Columns[10].HeaderText = "Peso Promedio";

                    //LLENA EL BINDINGNAVIGATOR//
                    bs = new BindingSource();
                    bs.DataSource = dt;
                    bindingNavigator1.BindingSource = bs;
                    dataGridView1.DataSource = bs;

                    totalpiezas = 0;
                    totalpesobruto = 0;
                    totalpesoneto = 0;
                    totalcajas = 0;
                    totalpesocajas = 0;


                    foreach (DataGridViewRow fila in dataGridView1.Rows)
                    {
                        int cont = dataGridView1.Rows.Count;
                        if (fila.Index == cont - 1)
                        {
                            break;
                        }
                        //VA GENERANDO LOS CAMPOS CALCULABLES//
                        dataGridView1.CurrentCell = dataGridView1.Rows[fila.Index].Cells[0];
                        Double piezasacum = Convert.ToDouble(dataGridView1.Rows[fila.Index].Cells[4].Value);
                        totalpiezas = totalpiezas + piezasacum;

                        Double cajasacum = Convert.ToDouble(dataGridView1.Rows[fila.Index].Cells[6].Value);
                        totalcajas = totalcajas + cajasacum;

                        Double pesobrutoacum = Convert.ToDouble(dataGridView1.Rows[fila.Index].Cells[7].Value);
                        totalpesobruto = totalpesobruto + pesobrutoacum;

                        Double pesocajasacum = Convert.ToDouble(dataGridView1.Rows[fila.Index].Cells[8].Value);
                        totalpesocajas = totalpesocajas + pesocajasacum;

                        Double pesonetoacum = Convert.ToDouble(dataGridView1.Rows[fila.Index].Cells[9].Value);
                        totalpesoneto = totalpesoneto + pesonetoacum;
                    }
                    textBox3.Text = totalpiezas.ToString();
                    textBox4.Text = totalcajas.ToString();
                    textBox5.Text = totalpesobruto.ToString();
                    textBox6.Text = totalpesocajas.ToString();
                    textBox7.Text = totalpesoneto.ToString();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                throw;
            }
        }



        //BOTON NUEVO, LIMPIA EL FORM PARA LLENAR UNA NUEVA SALIDA//
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (OleDbConnection conect = new OleDbConnection(EmpaqueSalidas.cadConex))
                {
                    conect.Open();
                    cmd = conect.CreateCommand();
                    //LIMPIA LOS CAMPOS//
                    button2.Enabled = true;
                    textBox2.Text = "";
                    comboBox1.Text = "[Selecciona]";
                    comboBox2.Text = "[Selecciona]";
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    textBox3.Text = "0";
                    textBox4.Text = "0";
                    textBox5.Text = "0.00";
                    textBox6.Text = "0.00";
                    textBox7.Text = "0.00";

                    //OBTIENE LA FECHA ACTUAL//
                    DateTime hoy = DateTime.Today;

                    String fechahoy = hoy.ToShortDateString();
                    textBox2.Text = fechahoy;
                    textBox1.Text = ObtenerClaveNueva().ToString();

                    reader.Close();

                    //LIMPIA EL DATATABLE//
                    dt.Clear();

                    dataGridView1.ReadOnly = false;

                    //BUSCA LOS USUARIO QUE ENTREGAN PARA PONER EN LOS COMBOBOX//
                    cmd.CommandText = "SELECT sysvusuario.nombreusuario from sysvusuario";
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(Convert.ToString(reader.GetValue(0)));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                throw;
            }

        }

        //BUSCA LA ÚLTIMA SALIDA E INCREMENTA 1, QUE SERÁ LA NUEVA//
        public int ObtenerClaveNueva()
        {
            try
            {
                cmd.CommandText = "SELECT TOP 1 IdSAPT FROM [APT-Empaque Salidas] ORDER BY IdSAPT desc;";
                reader = cmd.ExecuteReader();
                reader.Read();
                int val = Convert.ToInt32(reader.GetValue(0));
                val = val + 1;
                return val;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                throw;
            }

        }


        //OBTIENE LOS TIPOS DE CAJA PARA AGREGARLOS AL COMBOBOX DEL GRID//
        public List<String> obtenerTiposCajas()
        {
            try
            {
                using (OleDbConnection conect = new OleDbConnection(EmpaqueSalidas.cadConex))
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
                MessageBox.Show("Error: " + e.Message);
                throw;
            }
        }



        //BOTON GUARDAR, GUARDA UNA NUEVA SALIDA//
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (OleDbConnection conect = new OleDbConnection(EmpaqueSalidas.cadConex))
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
                            guardarSalidaEmpaque(conect);
                            //PREGUNTA SI ESTA SEGURO QUE DESEA GUARDAR//
                            DialogResult dialog = MessageBox.Show("Desea guardar?", "Guardar", MessageBoxButtons.YesNo);
                            //SI LA RESPUESTA ES SI, HACE UN COMMIT//
                            if (dialog == DialogResult.Yes)
                            {
                                cmd = new OleDbCommand("COMMIT", conect);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Insertado con éxito");
                                comboBox1.Enabled = false;
                                comboBox2.Enabled = false;

                                dataGridView1.ReadOnly = true;


                            }//SI LA RESPUESTA ES NO, HACE UN ROLLBACK Y NO INSERTA NADA//
                            else if (dialog == DialogResult.No)
                            {
                                cmd = new OleDbCommand("ROLLBACK", conect);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ingrese datos válidos en la grid");
                        }
                        
                    }


                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }




        //MÉTODO GUARDAR, INICIA LA TRANSACCIÓN PARA GUARDAR LOS DATOS DE LA SALIDA//
        public void guardarSalidaEmpaque(OleDbConnection conect)
        {
            try
            {
                conect.Open();
                //INICIA LA TRANSACCIÓN PARA GUARDAR LA SALIDA//
                cmd = new OleDbCommand("BEGIN TRANSACTION", conect);
                cmd.ExecuteNonQuery();

                DateTime hoy = DateTime.Today;

                int idExistencia;
                String fechahoy = hoy.ToShortDateString();
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

                //try 
                //{
                  //  if (UsuarioEntregoSAPTPedidos != "" && UsuarioEntregoSAPTPedidos != "[Selecciona]" && UsuarioRecibioSAPTPedidos != "" && UsuarioRecibioSAPTPedidos != "[Selecciona]")
                    //{
                        cmd.CommandText = "insert into [APT-Empaque Salidas](IdSAPT, FechaSAPT, UsuarioRecibióSAPT, usuarioentregóSAPT) values (@claveencabezado, @fechaencabezado, @usuariorecibio, @usuarioentrego)";
                        //cmd.CommandText = "SELECT Nombre, ApellidoPaterno FROM Agentes WHERE (((IdAgente)=[@name]));";
                        cmd.Parameters.AddWithValue("@claveencabezado", idencabezado);
                        cmd.Parameters.AddWithValue("@fechaencabezado", FechaSAPTPedidos);
                        cmd.Parameters.AddWithValue("@usuariorecibio", UsuarioRecibioSAPTPedidos);
                        cmd.Parameters.AddWithValue("@usuarioentrego", UsuarioEntregoSAPTPedidos);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        
                    //}
                //}
                //catch (Exception)
                //{
                //    MessageBox.Show("Ingrese datos válidos en el encabezado");
                //    throw;
                //}
                



                ////EDITANDO AQUI/////
                //Regex Val = new Regex(@"a-zA-ZñÑ\s{2,50}");
                //if (Val.IsMatch(UsuarioEntregoSAPTPedidos) && Val.IsMatch(UsuarioRecibioSAPTPedidos))
                //{

                //REALIZA LA CONSULTA PARA INSERTAR EL ENCABEZADO DE LA SALIDA//
                //cmd.CommandText = "insert into [APT-Empaque Salidas](IdSAPT, FechaSAPT, UsuarioRecibióSAPT, usuarioentregóSAPT) values (@claveencabezado, @fechaencabezado, @usuariorecibio, @usuarioentrego)";
                ////cmd.CommandText = "SELECT Nombre, ApellidoPaterno FROM Agentes WHERE (((IdAgente)=[@name]));";
                //cmd.Parameters.AddWithValue("@claveencabezado", idencabezado);
                //cmd.Parameters.AddWithValue("@fechaencabezado", FechaSAPTPedidos);
                //cmd.Parameters.AddWithValue("@usuariorecibio", UsuarioRecibioSAPTPedidos);
                //cmd.Parameters.AddWithValue("@usuarioentrego", UsuarioEntregoSAPTPedidos);
                //cmd.ExecuteNonQuery();
                //cmd.Parameters.Clear();
                //}
                //else
                //{
                //  MessageBox.Show("Ingrese datos validos en el encabezado");
                //}


                //RECORRE EL GRID E IGUALA LOS DATOS EN VARIABLES//
                foreach (DataGridViewRow fila in dataGridView1.Rows)
                {
                    int cont = dataGridView1.Rows.Count;
                    if (fila.Index == cont - 1)
                    {
                        break;
                    }
                    int IdTipoCajaSAPTPedidos = 0;
                    int fila2 = fila.Index;
                    int col = dataGridView1.CurrentCell.ColumnIndex;

                    String IdSubProductoSAPTPedidos = fila.Cells[0].Value.ToString().Trim();
                    String IdLoteCaducidadPedidos = fila.Cells[2].Value.ToString().Trim();
                    String FechaCad = fila.Cells[3].Value.ToString().Trim();
                    String FechaCaducidadSAPTPedidos = FechaCad.Split(' ')[0];
                    int PiezasSAPTPedidos = Convert.ToInt32(fila.Cells[4].Value.ToString().Trim());
                    String TipoCaja = Convert.ToString(dataGridView1.Rows[fila2].Cells[5].Value).Trim();
                    //VA AL MÉTODO PARA OBTENER EL ID DEL TIPO DE CAJA//
                    IdTipoCajaSAPTPedidos = EvaluarIdTipoCaja(TipoCaja);

                    //MessageBox.Show("HOLA " + IdTipoCajaSAPTPedidos);
                    int NumeroDeCajasSAPTPedidos = Convert.ToInt32(fila.Cells[6].Value.ToString().Trim());
                    Double pesobruto = Convert.ToDouble(fila.Cells[7].Value.ToString().Trim());
                    Double pesocajas = Convert.ToDouble(fila.Cells[8].Value.ToString().Trim());



                    //REALIZA LA CONSULTA PARA INSERTAR LOS DETALLES DE LA ENTRADA//
                    cmd.CommandText = "insert into [Detalle APT-Empaque Salidas](IdSAPT, idsubproductoSAPT, pesobrutoSAPT, idtipocajaSAPT, "
                    + "númerodecajasSAPT, piezasSAPT, taraSAPT, idlotecaducidad, fechacaducidadSAPT) values(@detalleclave, @detalleclavepro, @detallepesobruto, "
                    + "@detalleidcaja, @detallenumcaja, @detallepiezas, @detalletara, @detalleidlote, @detallefechacad)";
                    cmd.Parameters.AddWithValue("@detalleclave", idencabezado);
                    cmd.Parameters.AddWithValue("@detalleclavepro", IdSubProductoSAPTPedidos);
                    cmd.Parameters.AddWithValue("@detallepesobruto", pesobruto);
                    cmd.Parameters.AddWithValue("@detalleidcaja", IdTipoCajaSAPTPedidos);
                    cmd.Parameters.AddWithValue("@detallenumcaja", NumeroDeCajasSAPTPedidos);
                    cmd.Parameters.AddWithValue("@detallepiezas", PiezasSAPTPedidos);
                    cmd.Parameters.AddWithValue("@detalletara", pesocajas);
                    cmd.Parameters.AddWithValue("@detalleidlote", IdLoteCaducidadPedidos);
                    cmd.Parameters.AddWithValue("@detallefechacad", FechaCaducidadSAPTPedidos);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    //SI EXISTE, ACTUALIZA LA COLUMNA DE EMPAQUEENTRADASKG PARA TENER LAS EXISTENCIAS AL DÍA//
                    cmd.CommandText = "update [detalle existencias apt] set salidasempaquekg = salidasempaquekg + @pesobruto where " +
                        "idexistenciaapt = @idExist AND idsubproducto = @idsubprod";
                    cmd.Parameters.AddWithValue("@pesobruto", pesobruto);
                    cmd.Parameters.AddWithValue("@idExist", idExistencia);
                    cmd.Parameters.AddWithValue("@idsubprod", IdSubProductoSAPTPedidos);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Existe un error con esta clave");
                throw;
            }
        }
        ///ME QUEDÉ AQUÍ///
        private bool celdasNullEnDataGridView()
        {
            bool bVacia = false;
            if (dataGridView1.Rows.Count > 1)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    int cont = dataGridView1.Rows.Count;
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


        //MÉTODO QUE EVALÚA EL TIPO DE CAJA Y RETORNA SU ID//
        public int EvaluarIdTipoCaja(String TipoCaja)
        {
            int idcaja = 0;
            if (TipoCaja.Contains("Sin caja")){
                idcaja = 1;
            }else if (TipoCaja.Contains("Chica pvc")){
                idcaja = 2;
            }else if (TipoCaja.Contains("Grande pvc")){
                idcaja = 3;
            }else if (TipoCaja.Contains("Almac.chica pvc")){
                idcaja = 4;
            }else if (TipoCaja.Contains("Almac.grande pvc")){
                idcaja = 5;
            }else if (TipoCaja.Contains("Bote p/vinagre")){
                idcaja = 6;
            }else if (TipoCaja.Contains("~Garrafa p/vinagre")){
                idcaja = 7;
            }
            return idcaja;
        }

        //MÉTODO QUE EVALÚA EL PESO DE LA CAJA Y RETORNA SU PESO//
        public Double EvaluarPesoTipoCaja(String TipoCaja)
        {
            Double pesocaja = 0;
            if (TipoCaja.Contains("Sin caja")){
                pesocaja = 0.0;
            }else if (TipoCaja.Contains("Chica pvc")){
                pesocaja = 1.2;
            }else if (TipoCaja.Contains("Grande pvc")){
                pesocaja = 1.7;
            }else if (TipoCaja.Contains("Almac.chica pvc")){
                pesocaja = 2.2;
            }else if (TipoCaja.Contains("Almac.grande pvc")){
                pesocaja = 3.8;
            }else if (TipoCaja.Contains("Bote p/vinagre")){
                pesocaja = 0;
            }else if (TipoCaja.Contains("~Garrafa p/vinagre")){
                pesocaja = 0;
            }
            return pesocaja;
        }



        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }



        //EVENTO AL PRESIONAR UNA TECLA//
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //SI LA TECLA ES ENTER//
                if (e.KeyCode == Keys.Enter)
                {
                    //SendKeys.Send("{RIGHT}");
                    //SendKeys.Send("{UP}");                    

                    //VERIFICA QUE SE ENCUENTRE EN LA ULTIMA COLUMNA//
                    if (dataGridView1.CurrentCell.ColumnIndex == 10)
                    {
                        ///EDITANDO
                        totalpiezas = 0;
                        totalpesobruto = 0;
                        totalpesoneto = 0;
                        totalcajas = 0;
                        totalpesocajas = 0;
                        //int cont = 0;

                        //RECORRE EL GRID PARA SACAR LOS CAMPOS CALCULADOS//
                        foreach (DataGridViewRow item in dataGridView1.Rows)
                        {
                            int con = dataGridView1.Rows.Count;
                            if (item.Index == con - 1)
                            {
                                break;
                            }
                            //VA REALIZANDO LAS OPERACIONES PARA MOSTRAR LOS TOTALES EN EL ENCABEZADO//
                            dataGridView1.CurrentCell = dataGridView1.Rows[item.Index].Cells[0];
                            Double piezasacum = Convert.ToDouble(dataGridView1.Rows[item.Index].Cells[4].Value);
                            totalpiezas = totalpiezas + piezasacum;

                            Double cajasacum = Convert.ToDouble(dataGridView1.Rows[item.Index].Cells[6].Value);
                            totalcajas = totalcajas + cajasacum;

                            Double pesobrutoacum = Convert.ToDouble(dataGridView1.Rows[item.Index].Cells[7].Value);
                            totalpesobruto = totalpesobruto + pesobrutoacum;

                            Double pesocajasacum = Convert.ToDouble(dataGridView1.Rows[item.Index].Cells[8].Value);
                            totalpesocajas = totalpesocajas + pesocajasacum;

                            Double pesonetoacum = Convert.ToDouble(dataGridView1.Rows[item.Index].Cells[9].Value);
                            totalpesoneto = totalpesoneto + pesonetoacum;




                        }
                        //MUESTRA LOS TOTALES PARCIALES//
                        textBox3.Text = totalpiezas.ToString();
                        textBox4.Text = totalcajas.ToString();
                        textBox5.Text = totalpesobruto.ToString();
                        textBox6.Text = totalpesocajas.ToString();
                        textBox7.Text = totalpesoneto.ToString();



                    }
                    SendKeys.Send("{DOWN}");
                    SendKeys.Send("{LEFT}");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                throw;
            }
        }




        //MÉTODO AL TERMINAR DE EDITAR UNA CELDA//
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.Columns[1].ReadOnly = true;

                using (OleDbConnection conect = new OleDbConnection(EmpaqueSalidas.cadConex))
                {
                    SendKeys.Send("{UP}");
                    SendKeys.Send("{RIGHT}");

                    //SI LA COLUMNA TIENE DE ENCABEZADO "CLAVE"//
                    if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Clave")
                    {
                        int col = dataGridView1.CurrentCell.ColumnIndex;

                        //OBTIENE EL VALOR DE LA CELDA ACTUAL//
                        String iddetalle = dataGridView1.CurrentCell.Value.ToString().Trim();
                        //SE INSTANCIA UNA EXPRESIÓN REGULAR//
                        Regex Val = new Regex(@"([a-zA-Z][a-zA-Z]\-\d{2,3}\-\d{2})$");
                        //SI EL VALOR DE LA CELDA COINCIDE CON LA EXPRESIÓN REGULAR//
                        if (Val.IsMatch(iddetalle))
                        {
                            //ASIGNA VALOR A LA CELDA DE LA COLUMNA DE "NOMBRE DEL PRODUCTO" CON UNA//
                            //CONSULTA AUTOMÁTICA AL INGRESAR EL ID DEL PRODUCTO//
                            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[col + 1].Value = buscarDetalle(iddetalle);
                            SendKeys.Send("{RIGHT}");
                        }
                        else
                        {
                            MessageBox.Show("Ingrese una clave válida");
                            SendKeys.Send("{UP}");
                        }
                    }

                    if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Lote")
                    {
                        String lote = dataGridView1.CurrentCell.Value.ToString().Trim();
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

                    if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Caducidad")
                    {
                        String fcaducidad = dataGridView1.CurrentCell.Value.ToString().Trim();
                        if (fcaducidad != "")
                        {

                        }
                        else
                        {
                            MessageBox.Show("Ingrese una formato válido");
                            SendKeys.Send("{UP}");
                        }
                    }

                    if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Piezas")
                    {
                        String pz = dataGridView1.CurrentCell.Value.ToString().Trim();
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

                    if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Cajas")
                    {
                        String cajas = dataGridView1.CurrentCell.Value.ToString().Trim();
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

                    if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Peso Bruto")
                    {
                        String pesobruto = dataGridView1.CurrentCell.Value.ToString().Trim();
                        Regex Val = new Regex(@"^[0-9]+\.?[0-9]*$");
                        if (Val.IsMatch(pesobruto))
                        {
                            //OBTIENE EL VALOR DE LA CELDA DE PIEZAS DE LA FILA ACTUAL//
                            Double piezas = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex - 3].Value);
                            //OBTIENE EL VALOR DE LA CELDA DE PESONETO DE LA FILA ACTUAL//
                            //Double pesoneto = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                            //?????//
                            //dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value = 0;
                            //dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].Value = pesoneto;
                            SendKeys.Send("{RIGHT}");
                            SendKeys.Send("{RIGHT}");

                            Double cajas = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value);
                            Double pesocaja = EvaluarPesoTipoCaja(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex - 2].Value.ToString());
                            Double tara = cajas * pesocaja;
                            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value = tara;

                            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].Value = Convert.ToDouble(pesobruto) - tara;
                            Double pesonet = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].Value);

                            if (piezas == 0)
                            {
                                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 3].Value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].Value;
                                SendKeys.Send("{RIGHT}");

                            }
                            else if (piezas != 0)
                            {
                                Double pesopromedio = pesonet / piezas;
                                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 3].Value = pesopromedio;
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
                throw;
            }
        }




        //MÉTODO PARA BUSCAR EL NOMBRE DEL PRODUCTO DEL ID INGRESADO EN LA FILA ACTUAL DEL GRID//
        public String buscarDetalle(String idDetalle)
        {
            try
            {
                using (OleDbConnection conect = new OleDbConnection(EmpaqueSalidas.cadConex))
                {
                    cmd = conect.CreateCommand();
                    //CONSULTA PARA OBTENER EL NOMBRE DEL PRODUCTO BUSCADO POR ID//
                    cmd.CommandText = "SELECT subproductos.nombresubproducto from subproductos where idsubproducto = @id";
                    cmd.Parameters.AddWithValue("@id", idDetalle);
                    conect.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    String val = reader.GetValue(0).ToString();
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



        //DATAERROR PARA CONTROLAR LOS ERRORES DE DISEÑO DEL GRID//
        public void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
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



        //MÉTODO AL HACER CLIC EN UNA CELDA DEL GRID//
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            String valor = Convert.ToString(dataGridView1.CurrentCell.RowIndex);
            String valor2 = Convert.ToString(dataGridView1.CurrentCell.ColumnIndex);
            //MessageBox.Show(valor + ", "+ valor2);
        }

        //MÉTODO AL INICIAR A EDITAR UNA CELDA//
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                dataGridView1.Columns[1].ReadOnly = true;
                using (OleDbConnection conect = new OleDbConnection(EmpaqueSalidas.cadConex))
                {
                    if (e.ColumnIndex > -1)
                    {
                        dpbox = new DataGridViewComboBoxCell();
                        //SE LLAMA AL MÉTODO OBTENERTIPOSCAJAS PARA LLENAR EL COMBOBOX DEL GRID//
                        if (dataGridView1.Columns[e.ColumnIndex].Name.Contains("descripcióntipocaja"))
                        {
                            dataGridView1[e.ColumnIndex, e.RowIndex] = dpbox;
                            dpbox.DataSource = obtenerTiposCajas();

                        }

                    }
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
            this.Hide();
        }



    }
}
