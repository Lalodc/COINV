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
    public partial class AgenteEntradas : Form
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
        Double totalcajasprod;
        Double totalpesocajas;
        Double totalcajasvacias;
        int bandera;

        public AgenteEntradas()
        {
            InitializeComponent();
        }

        private void dataGridView1BindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            //VALIDAR//
            this.Validate();
        }

        private void AgenteEntradas_Load(object sender, EventArgs e)
        {
            //CONTROLAR ERRORES//
            //this.dataGridView1.DataError += new DataGridViewDataErrorEventHandler(dataGridView1_DataError);

            //BUSCAR LA ÚLTIMA SALIDA AL ACCEDER AL FORMULARIO//
            using (OleDbConnection conect = new OleDbConnection(AgenteEntradas.cadConex))
            {

                try
                {
                    conect.Open();
                    cmd = conect.CreateCommand();
                    //BUSCA LA ÚLTIMA SALIDA//
                    cmd.CommandText = "SELECT TOP 1 IdEntradasAPT FROM [APT - Entradas] ORDER BY IdEntradasAPT desc;";
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    String idAgenteEntrada = Convert.ToString(reader.GetValue(0));
                    textBox1.Text = idAgenteEntrada;

                    bandera = 0;

                    BuscarEntradaAgenteEncabezado(idAgenteEntrada);
                    BuscarEntradaAgenteDetalle(idAgenteEntrada);

                    if (bandera == 0)
                    {//duda, checar buscar uno y despues nada
                        textBox2.Text = "";
                        comboBox1.Text = "";
                        comboBox2.Text = "";
                    }
                    //Group1
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    textBox3.Enabled = false;
                    //Group2
                    textBox4.Enabled = false;
                    textBox5.Enabled = false;
                    textBox6.Enabled = false;
                    textBox7.Enabled = false;
                    //Group3
                    textBox8.Enabled = false;
                    textBox9.Enabled = false;
                    textBox10.Enabled = false;

                    dataGridView1.ReadOnly = true;
                    dataGridView2.ReadOnly = true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);                   
                }
            }
        }


        //REALIZA LA BUSQUEDA DE LAS SALIDAS A EMPAQUE AL PRESIONAR ENTER//
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            using (OleDbConnection conect = new OleDbConnection(AgenteEntradas.cadConex))
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
                            String idAgenteEntrada = textBox1.Text;

                            BuscarEntradaAgenteEncabezado(idAgenteEntrada);
                            BuscarEntradaAgenteDetalle(idAgenteEntrada);

                            if (bandera == 0)
                            {
                                textBox2.Text = "";
                                comboBox1.Text = "";
                                comboBox2.Text = "";
                                comboBox3.Text = "";
                            }
                            //Group1
                            comboBox1.Enabled = false;
                            comboBox2.Enabled = false;
                            comboBox3.Enabled = false;
                            textBox3.Enabled = false;
                            //Group2
                            textBox4.Enabled = false;
                            textBox5.Enabled = false;
                            textBox6.Enabled = false;
                            textBox7.Enabled = false;
                            //Group3
                            textBox8.Enabled = false;
                            textBox9.Enabled = false;
                            textBox10.Enabled = false;

                            dataGridView1.ReadOnly = true;
                            dataGridView2.ReadOnly = true;

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);                            
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
        public void BuscarEntradaAgenteEncabezado(String EidAgenteEntrada)
        {
            try
            {
                using (OleDbConnection conect = new OleDbConnection(AgenteEntradas.cadConex))
                {

                    cmd = conect.CreateCommand();
                    //BUSCA LA SALIDA SEGUN LA CLAVE INGRESADA EN EL TEXTBOX//
                    cmd.CommandText = "SELECT fechaentrada, nombrecompleto, nombreregión, usuario, observacionesentrada FROM ((([apt - entradas] "
                    +"INNER JOIN agentes ON [apt - entradas].idagente = agentes.idagente) INNER JOIN regiones ON [apt - entradas].idregión = regiones.idregión) "
                    +"INNER JOIN sysvusuario ON [apt - entradas].idelaboróentrada = sysvusuario.usuario)  WHERE [apt - entradas].idEntradasAPT = @idAgenteEntrada";
                    cmd.Parameters.AddWithValue("idAgenteEntrada", EidAgenteEntrada);
                    /*cmd.CommandText = "SELECT [APT-Empaque Salidas].fechaSAPT, [APT-Empaque Salidas].usuarioentregósapt, [APT-Empaque Salidas].usuariorecibiósapt "
                    + "from [APT-Empaque Salidas] where [APT-Empaque Salidas].idsapt =" + EidAgenteEntrada + ";";*/
                    conect.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        textBox2.Text = Convert.ToString(reader[0]);
                        comboBox1.Text = Convert.ToString(reader[1]);
                        comboBox2.Text = Convert.ToString(reader[2]);
                        comboBox3.Text = Convert.ToString(reader[3]);
                        textBox3.Text = Convert.ToString(reader[4]);
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
        public void BuscarEntradaAgenteDetalle(String DidAgenteEntrada)
        {
            try
            {
                using (OleDbConnection conect = new OleDbConnection(AgenteEntradas.cadConex))
                {
                    //buscar id de la abla APT-Empaque Salidas donde la fecha de pedido es el 3 de junio del 2017//
                    //BUSCAR EL DETALLE DE LA SALIDAS PARA MOSTRARLO EN EL GRID//

                    String query = "SELECT subproductos.idsubproducto, subproductos.nombresubproducto, unidades.descripcion, "
                    +"subproductos.cantidadenvase, [detalle apt - entradas].idlotecaducidad, [detalle apt - entradas].fechacaducidad, [detalle apt - entradas].piezasentradas, "
                    +"[Tipos de cajas].descripcióntipocaja, [detalle apt - entradas].númerodecajasentradas, [detalle apt - entradas].kilosentradas,  "
                    +"[Tipos de cajas].pesocaja*[detalle apt - entradas].númerodecajasentradas AS PesoCajas, [detalle apt - entradas].kilosentradas-PesoCajas AS PesoNeto, "
                    + "IIf([detalle apt - entradas].piezasentradas=0, PesoNeto, PesoNeto/[detalle apt - entradas].piezasentradas) as PesoPromedio, [detalle apt - entradas].observacionesentradas from "
                    +"subproductos, unidades, [detalle apt - entradas], [Tipos de cajas] WHERE [detalle apt - entradas].IdEntradasAPT = " + DidAgenteEntrada + " "
                    +"AND subproductos.idsubproducto = [detalle apt - entradas].idsubproducto AND [Tipos de cajas].idtipocaja = [detalle apt - entradas].IdTipoCaja "
                    +"AND Unidades.IdUnidad = subproductos.idUnidad";

                    /*String query = "SELECT subproductos.idsubproducto, subproductos.nombresubproducto, [Detalle APT-Empaque Salidas].idlotecaducidad, "
                        + "[Detalle APT-Empaque Salidas].fechacaducidadSAPT, [Detalle APT-Empaque Salidas].piezasSAPT, [Tipos de cajas].descripcióntipocaja, "
                        + "[Detalle APT-Empaque Salidas].númerodecajasSAPT, [Detalle APT-Empaque Salidas].pesobrutoSAPT, "
                        + "[Tipos de cajas].pesocaja*[Detalle APT-Empaque Salidas].númerodecajasSAPT AS PesoCajas, "
                        + "[Detalle APT-Empaque Salidas].pesobrutoSAPT-PesoCajas AS PesoNeto, "
                        + "IIf([Detalle APT-Empaque Salidas].piezasSAPT=0, PesoNeto, PesoNeto/[Detalle APT-Empaque Salidas].piezasSAPT) as PesoPromedio "
                        + "from subproductos, [Detalle APT-Empaque Salidas], [Tipos de cajas] where [Detalle APT-Empaque Salidas].idSAPT = " + DidAgenteEntrada + " "
                        + "AND subproductos.idsubproducto = [Detalle APT-Empaque Salidas].idsubproductoSAPT AND [Tipos de cajas].idtipocaja = [Detalle APT-Empaque Salidas].idtipocajaSAPT";*/

                    adapter = new OleDbDataAdapter(query, conect);
                    dt = new DataTable();
                    //LLENA EL ADAPTADOR CON LOS RESULTADOS DE LA CONSULTA
                    adapter.Fill(dt);

                    //LLENA EL GRID CON LOS RESULTADOS DE LA COLUMNA//
                    dataGridView1.DataSource = dt;

                    //ASIGNA NOMBRE A LOS ENCABEZADOS DE CADA COLUMNA//
                    dataGridView1.Columns[0].HeaderText = "Clave";
                    dataGridView1.Columns[1].HeaderText = "Producto";
                    dataGridView1.Columns[2].HeaderText = "Unidad";
                    dataGridView1.Columns[3].HeaderText = "Cap Empaque";
                    dataGridView1.Columns[4].HeaderText = "Lote";
                    dataGridView1.Columns[5].HeaderText = "Caducidad";
                    dataGridView1.Columns[6].HeaderText = "Piezas";
                    dataGridView1.Columns[7].HeaderText = "Tipo Caja";
                    dataGridView1.Columns[8].HeaderText = "Cajas";
                    dataGridView1.Columns[9].HeaderText = "Peso Bruto";
                    dataGridView1.Columns[10].HeaderText = "Peso Cajas";
                    dataGridView1.Columns[11].HeaderText = "Peso Neto";
                    dataGridView1.Columns[12].HeaderText = "Peso Promedio";
                    dataGridView1.Columns[13].HeaderText = "Observaciones";

                    //LLENA EL BINDINGNAVIGATOR//
                    bs = new BindingSource();
                    bs.DataSource = dt;
                    bindingNavigator1.BindingSource = bs;
                    dataGridView1.DataSource = bs;

                    LlenarGridDeCajasVacias(DidAgenteEntrada);

                    totalpiezas = 0;
                    totalpesobruto = 0;
                    totalpesoneto = 0;
                    totalcajasprod = 0;
                    totalpesocajas = 0;
                    totalcajasvacias = 0;


                    foreach (DataGridViewRow fila in dataGridView1.Rows)
                    {
                        int cont = dataGridView1.Rows.Count;
                        if (fila.Index == cont - 1)
                        {
                            break;
                        }
                        //VA GENERANDO LOS CAMPOS CALCULABLES//
                        dataGridView1.CurrentCell = dataGridView1.Rows[fila.Index].Cells[0];
                        Double piezasacum = Convert.ToDouble(dataGridView1.Rows[fila.Index].Cells[6].Value);
                        totalpiezas = totalpiezas + piezasacum;

                        Double cajasacum = Convert.ToDouble(dataGridView1.Rows[fila.Index].Cells[8].Value);
                        totalcajasprod = totalcajasprod + cajasacum;

                        Double pesobrutoacum = Convert.ToDouble(dataGridView1.Rows[fila.Index].Cells[9].Value);
                        totalpesobruto = totalpesobruto + pesobrutoacum;

                        Double pesocajasacum = Convert.ToDouble(dataGridView1.Rows[fila.Index].Cells[10].Value);
                        totalpesocajas = totalpesocajas + pesocajasacum;

                        Double pesonetoacum = Convert.ToDouble(dataGridView1.Rows[fila.Index].Cells[11].Value);
                        totalpesoneto = totalpesoneto + pesonetoacum;
                    }

                    foreach (DataGridViewRow f in dataGridView2.Rows)
                    {
                        int cont2 = dataGridView2.Rows.Count;
                        if (f.Index == cont2-1)
                        {
                            break;
                        }
                        dataGridView2.CurrentCell = dataGridView2.Rows[f.Index].Cells[0];
                        Double cajasvaciasaum = Convert.ToDouble(dataGridView2.Rows[f.Index].Cells[1].Value);
                        totalcajasvacias = totalcajasvacias + cajasvaciasaum;

                    }
                    Double totalcajas = totalcajasprod + totalcajasvacias;
                    textBox6.Text = totalcajasvacias.ToString();
                    textBox7.Text = totalcajas.ToString();
                    textBox4.Text = totalpiezas.ToString();
                    textBox5.Text = totalcajasprod.ToString();
                    textBox8.Text = totalpesobruto.ToString();
                    textBox9.Text = totalpesocajas.ToString();
                    textBox10.Text = totalpesoneto.ToString();
                    
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                throw;
            }
        }

        public void LlenarGridDeCajasVacias(String DidAgenteEntrada)
        {
            try
            {
                using (OleDbConnection conect = new OleDbConnection(AgenteEntradas.cadConex))
                {
                    String query = "SELECT [Tipos de cajas].descripcióntipocaja, [detalle apt - cajasentradas].númerodecajasentradas, "
                        +"[Tipos de cajas].pesocaja*[detalle apt - cajasentradas].númerodecajasentradas as PesoCajas FROM [Tipos de Cajas], [Detalle APT - CajasEntradas] "
                        +"WHERE [Tipos de cajas].idtipocaja = [detalle apt - cajasentradas].IdTipoCaja AND [detalle apt - cajasentradas].identradasapt = " + DidAgenteEntrada +";";

                    adapter = new OleDbDataAdapter(query, conect);
                    dt = new DataTable();
                    adapter.Fill(dt);

                    //LLENA EL GRID CON LOS RESULTADOS DE LA COLUMNA//
                    dataGridView2.DataSource = dt;

                    dataGridView2.Columns[0].HeaderText = "Tipo Caja";
                    dataGridView2.Columns[1].HeaderText = "Cajas";
                    dataGridView2.Columns[2].HeaderText = "Peso Cajas";
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
                using (OleDbConnection conect = new OleDbConnection(AgenteEntradas.cadConex))
                {
                    conect.Open();
                    cmd = conect.CreateCommand();
                    //LIMPIA LOS CAMPOS//
                    button2.Enabled = true;
                    textBox2.Text = "";
                    comboBox1.Text = "[Selecciona]";
                    comboBox2.Text = "[Selecciona]";
                    comboBox3.Text = "[Selecciona]";
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    comboBox3.Enabled = true;
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
                    dataGridView2.ReadOnly = false;


                    cmd.CommandText = "SELECT nombrecompleto From agentes;";
                    reader = cmd.ExecuteReader();
                    while (true)
                    {
                        comboBox1.Items.Add(Convert.ToString(reader.GetValue(0)));
                    }

                    cmd.CommandText = "SELECT nombreregión From regiones;";
                    reader = cmd.ExecuteReader();
                    while (true)
                    {
                        comboBox2.Items.Add(Convert.ToString(reader.GetValue(0)));
                    }

                    

                    //BUSCA LOS USUARIO QUE ENTREGAN PARA PONER EN LOS COMBOBOX//
                    cmd.CommandText = "SELECT sysvusuario.nombreusuario from sysvusuario";
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox3.Items.Add(Convert.ToString(reader.GetValue(0)));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        //BUSCA LA ÚLTIMA SALIDA E INCREMENTA 1, QUE SERÁ LA NUEVA//
        public int ObtenerClaveNueva()
        {
            try
            {
                cmd.CommandText = "SELECT TOP 1 IdEntradasAPT FROM [APT - Entradas] ORDER BY IdEntradasAPT desc;";
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
                using (OleDbConnection conect = new OleDbConnection(AgenteEntradas.cadConex))
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
                using (OleDbConnection conect = new OleDbConnection(AgenteEntradas.cadConex))
                {

                    if (comboBox1.Text == "" || comboBox1.Text == "[Selecciona]" || comboBox2.Text == "" || comboBox2.Text == "[Selecciona]" || comboBox3.Text == "" || comboBox3.Text == "[Selecciona]")
                    {
                        MessageBox.Show("Ingrese datos válidos en el encabezado");
                    }
                    else
                    {
                        Boolean EGrid = celdasNullEnDataGridView();
                        if (EGrid == false)
                        {
                            guardarEntradaAgente(conect);
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
                                comboBox3.Enabled = false;
                                button2.Enabled = false;
                                dataGridView1.ReadOnly = true;
                                dataGridView2.ReadOnly = true;


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
        public void guardarEntradaAgente(OleDbConnection conect)
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
                String nombreagente = comboBox1.Text.Trim();
                String idAgente = obtenerIdAgente(nombreagente);
                String nombreregion = comboBox2.Text.Trim();
                int idregion = Convert.ToInt32(obtenerIdregion(nombreregion));
                String nombreusuario = comboBox3.Text.Trim();

                //try 
                //{
                //  if (UsuarioEntregoSAPTPedidos != "" && UsuarioEntregoSAPTPedidos != "[Selecciona]" && UsuarioRecibioSAPTPedidos != "" && UsuarioRecibioSAPTPedidos != "[Selecciona]")
                //{
                cmd.CommandText = "insert into [APT - Entradas](IdEntradasAPT, FechaEntrada, IdAgente, IdRegión, IdElaboróEntrada) values (@claveencabezado, @fechaencabezado, @idagente, @idregion, @usuarioelabora)";
                //cmd.CommandText = "SELECT Nombre, ApellidoPaterno FROM Agentes WHERE (((IdAgente)=[@name]));";
                cmd.Parameters.AddWithValue("@claveencabezado", idencabezado);
                cmd.Parameters.AddWithValue("@fechaencabezado", FechaSAPTPedidos);
                cmd.Parameters.AddWithValue("@usuariorecibio", idAgente);
                cmd.Parameters.AddWithValue("@usuarioentrego", idregion);
                cmd.Parameters.AddWithValue("@usuarioentrego", nombreusuario);
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

                //ME QUEDE AQUI///////UHHHHH////
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

        public String obtenerIdAgente(String nombreAgente)
        {
            try
            {
                cmd.CommandText = "SELECT idAgente from agentes where nombrecompleto = @nombreagente";
                cmd.Parameters.AddWithValue("@nombreagente",nombreAgente);
                reader = cmd.ExecuteReader();
                reader.Read();
                String val = Convert.ToString(reader.GetValue(0));
                return val;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                throw;
            }
        }

        public String obtenerIdregion(String nombreregion)
        {
            try
            {
                cmd.CommandText = "SELECT idregión from regiones where nombreregión = @nombreregion";
                cmd.Parameters.AddWithValue("@nombreagente", nombreregion);
                reader = cmd.ExecuteReader();
                reader.Read();
                String val = Convert.ToString(reader.GetValue(0));
                return val;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
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
