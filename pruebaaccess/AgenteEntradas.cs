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
        DataTable dt1;
        DataTable dt2;


        DataGridViewComboBoxCell dpbox;
        DataGridViewComboBoxCell dpbox2;

        Double totalpiezas;
        Double totalpesobruto;
        Double totalpesoneto;
        Double totalcajasprod;
        Double totalpesocajas;
        Double totalcajasvacias;
        Double totalcajas;
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
            this.dataGridView1.DataError += new DataGridViewDataErrorEventHandler(dataGridView1_DataError);

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
                    dt1 = new DataTable();
                    //LLENA EL ADAPTADOR CON LOS RESULTADOS DE LA CONSULTA
                    adapter.Fill(dt1);

                    //LLENA EL GRID CON LOS RESULTADOS DE LA COLUMNA//
                    dataGridView1.DataSource = dt1;

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
                    bs.DataSource = dt1;
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
                    dt2 = new DataTable();
                    adapter.Fill(dt2);

                    //LLENA EL GRID CON LOS RESULTADOS DE LA COLUMNA//
                    dataGridView2.DataSource = dt2;

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
                    textBox3.Enabled = true;
                    textBox3.Text = "";
                    textBox4.Text = "0";
                    textBox5.Text = "0";
                    textBox6.Text = "0";
                    textBox7.Text = "0";
                    textBox8.Text = "0.00";
                    textBox9.Text = "0.00";
                    textBox10.Text = "0.00";

                    //OBTIENE LA FECHA ACTUAL//
                    DateTime hoy = DateTime.Today;

                    String fechahoy = hoy.ToShortDateString();
                    textBox2.Text = fechahoy;
                    textBox1.Text = ObtenerClaveNueva().ToString();

                    reader.Close();

                    //LIMPIA EL DATATABLE//
                    dt1.Clear();
                    dt2.Clear();

                    dataGridView1.ReadOnly = false;
                    dataGridView2.ReadOnly = false;


                    cmd.CommandText = "SELECT nombrecompleto From agentes;";
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(Convert.ToString(reader.GetValue(0)));
                    }
                    reader.Close();

                    cmd.CommandText = "SELECT nombreregión From regiones;";
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox2.Items.Add(Convert.ToString(reader.GetValue(0)));
                    }
                    reader.Close();

                    

                    //BUSCA LOS USUARIO QUE ENTREGAN PARA PONER EN LOS COMBOBOX//
                    cmd.CommandText = "SELECT sysvusuario.nombreusuario from sysvusuario";
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox3.Items.Add(Convert.ToString(reader.GetValue(0)));
                    }
                    reader.Close();
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
                String observacionesE = textBox3.Text.Trim();

                //try 
                //{
                //  if (UsuarioEntregoSAPTPedidos != "" && UsuarioEntregoSAPTPedidos != "[Selecciona]" && UsuarioRecibioSAPTPedidos != "" && UsuarioRecibioSAPTPedidos != "[Selecciona]")
                //{
                cmd.CommandText = "insert into [APT - Entradas](IdEntradasAPT, FechaEntrada, IdAgente, IdRegión, IdElaboróEntrada, ObservacionesEntrada) "
                +"values (@claveencabezado, @fechaencabezado, @idagente, @idregion, @usuarioelabora, @observacionesE)";
                //cmd.CommandText = "SELECT Nombre, ApellidoPaterno FROM Agentes WHERE (((IdAgente)=[@name]));";
                cmd.Parameters.AddWithValue("@claveencabezado", idencabezado);
                cmd.Parameters.AddWithValue("@fechaencabezado", FechaSAPTPedidos);
                cmd.Parameters.AddWithValue("@usuariorecibio", idAgente);
                cmd.Parameters.AddWithValue("@usuarioentrego", idregion);
                cmd.Parameters.AddWithValue("@usuarioentrego", nombreusuario);
                cmd.Parameters.AddWithValue("@observacionesE", observacionesE);
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
                    //String IdLoteCaducidadPedidos = fila.Cells[2].Value.ToString().Trim();
                    //String FechaCad = fila.Cells[3].Value.ToString().Trim();
                    //String FechaCaducidadSAPTPedidos = FechaCad.Split(' ')[0];
                    int PiezasSAPTPedidos = Convert.ToInt32(fila.Cells[6].Value.ToString().Trim());
                    String TipoCaja = Convert.ToString(dataGridView1.Rows[fila2].Cells[7].Value).Trim();
                    //VA AL MÉTODO PARA OBTENER EL ID DEL TIPO DE CAJA//
                    IdTipoCajaSAPTPedidos = EvaluarIdTipoCaja(TipoCaja);

                    //MessageBox.Show("HOLA " + IdTipoCajaSAPTPedidos);
                    int NumeroDeCajasSAPTPedidos = Convert.ToInt32(fila.Cells[8].Value.ToString().Trim());
                    Double pesobruto = Convert.ToDouble(fila.Cells[9].Value.ToString().Trim());
                    Double pesocajas = Convert.ToDouble(fila.Cells[10].Value.ToString().Trim());
                    String observ = Convert.ToString(fila.Cells[13].Value.ToString().Trim());



                    //REALIZA LA CONSULTA PARA INSERTAR LOS DETALLES DE LA ENTRADA//
                    cmd.CommandText = "insert into [Detalle APT-Entradas](IdEntradasAPT, IdSubProducto, KilosEntradas, IdTipoCaja, "
                    + "NúmeroDeCajasEntradas, PiezasEntradas, Tara, ObservacionesEntradas) values(@detalleclave, @detalleclavepro, @detallepesobruto, "
                    + "@detalleidcaja, @detallenumcaja, @detallepiezas, @detalletara, @detalleobser)";
                    cmd.Parameters.AddWithValue("@detalleclave", idencabezado);
                    cmd.Parameters.AddWithValue("@detalleclavepro", IdSubProductoSAPTPedidos);
                    cmd.Parameters.AddWithValue("@detallepesobruto", pesobruto);
                    cmd.Parameters.AddWithValue("@detalleidcaja", IdTipoCajaSAPTPedidos);
                    cmd.Parameters.AddWithValue("@detallenumcaja", NumeroDeCajasSAPTPedidos);
                    cmd.Parameters.AddWithValue("@detallepiezas", PiezasSAPTPedidos);
                    cmd.Parameters.AddWithValue("@detalletara", pesocajas);
                    cmd.Parameters.AddWithValue("@detalleobser", observ);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();


                    



                    //SI EXISTE, ACTUALIZA LA COLUMNA DE EMPAQUEENTRADASKG PARA TENER LAS EXISTENCIAS AL DÍA//
                    cmd.CommandText = "update [detalle existencias apt] set entradasagenteskg = entradasagenteskg + @pesobruto where " +
                        "idexistenciaapt = @idExist AND idsubproducto = @idsubprod";
                    cmd.Parameters.AddWithValue("@pesobruto", pesobruto);
                    cmd.Parameters.AddWithValue("@idExist", idExistencia);
                    cmd.Parameters.AddWithValue("@idsubprod", IdSubProductoSAPTPedidos);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                }

                foreach (DataGridViewRow fila2 in dataGridView2.Rows)
                {
                    int cont2 = dataGridView2.Rows.Count;
                    if (fila2.Index == cont2 - 1)
                    {
                        break;
                    }

                    int IdTipoCajaV = 0;
                    int fila3 = fila2.Index;
                    int col = dataGridView2.CurrentCell.ColumnIndex;

                    //String IdSubProductoSAPTPedidos = fila2.Cells[0].Value.ToString().Trim();
                    //String IdLoteCaducidadPedidos = fila.Cells[2].Value.ToString().Trim();
                    //String FechaCad = fila.Cells[3].Value.ToString().Trim();
                    //String FechaCaducidadSAPTPedidos = FechaCad.Split(' ')[0];                    
                    String TipoCaja = Convert.ToString(dataGridView2.Rows[fila3].Cells[0].Value).Trim();
                    IdTipoCajaV = EvaluarIdTipoCaja(TipoCaja);
                    int PzCajasV = Convert.ToInt32(fila2.Cells[1].Value.ToString().Trim());


                    cmd.CommandText = "INSERT INTO [Detalle APT - CajasEntradas](IdEntradasAPT, IdTipoCaja, NúmeroDeCajasEntradas) "
                            + "values(@detclavecajasv, @dettipocajav, @detnumcajasv)";
                    cmd.Parameters.AddWithValue("@detclavecajasv",idencabezado);
                    cmd.Parameters.AddWithValue("@dettipocajav", IdTipoCajaV);
                    cmd.Parameters.AddWithValue("@detnumcajasv", PzCajasV);
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
                    if (dataGridView1.CurrentCell.ColumnIndex == 13)
                    {
                        ///EDITANDO                      
                        totalpiezas = 0;
                        totalpesobruto = 0;
                        totalpesoneto = 0;
                        totalcajasprod = 0;
                        totalpesocajas = 0;
                        //totalcajasvacias = 0;

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

                            Double piezasacum = Convert.ToDouble(dataGridView1.Rows[item.Index].Cells[6].Value);
                            totalpiezas = totalpiezas + piezasacum;

                            Double cajasacum = Convert.ToDouble(dataGridView1.Rows[item.Index].Cells[8].Value);
                            totalcajasprod = totalcajasprod + cajasacum;

                            Double pesobrutoacum = Convert.ToDouble(dataGridView1.Rows[item.Index].Cells[9].Value);
                            totalpesobruto = totalpesobruto + pesobrutoacum;

                            Double pesocajasacum = Convert.ToDouble(dataGridView1.Rows[item.Index].Cells[10].Value);
                            totalpesocajas = totalpesocajas + pesocajasacum;

                            Double pesonetoacum = Convert.ToDouble(dataGridView1.Rows[item.Index].Cells[11].Value);
                            totalpesoneto = totalpesoneto + pesonetoacum;

                        }
                        //foreach (DataGridViewRow f in dataGridView2.Rows)
                        //{
                        //    int cont2 = dataGridView2.Rows.Count;
                        //    if (f.Index == cont2 - 1)
                        //    {
                        //        break;
                        //    }
                        //    dataGridView2.CurrentCell = dataGridView2.Rows[f.Index].Cells[0];
                        //    Double cajasvaciasaum = Convert.ToDouble(dataGridView2.Rows[f.Index].Cells[1].Value);
                        //    totalcajasvacias = totalcajasvacias + cajasvaciasaum;

                        //}
                        //MUESTRA LOS TOTALES PARCIALES//
                        //////Double totalcajas = totalcajasprod + totalcajasvacias;
                        ////textBox6.Text = totalcajasvacias.ToString();
                        //textBox7.Text = totalcajas.ToString();
                        textBox4.Text = totalpiezas.ToString();
                        textBox5.Text = totalcajasprod.ToString();
                        textBox8.Text = totalpesobruto.ToString();
                        textBox9.Text = totalpesocajas.ToString();
                        textBox10.Text = totalpesoneto.ToString();
                        
                        



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

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //SI LA TECLA ES ENTER//
                if (e.KeyCode == Keys.Enter)
                {
                    //SendKeys.Send("{RIGHT}");
                    //SendKeys.Send("{UP}");                    

                    totalcajasvacias = 0;
                    //VERIFICA QUE SE ENCUENTRE EN LA ULTIMA COLUMNA//
                    if (dataGridView1.CurrentCell.ColumnIndex == 2)
                    {
                        foreach (DataGridViewRow f in dataGridView2.Rows)
                        {
                            int cont2 = dataGridView2.Rows.Count;
                            if (f.Index == cont2 - 1)
                            {
                                break;
                            }
                            dataGridView2.CurrentCell = dataGridView2.Rows[f.Index].Cells[0];
                            Double cajasvaciasaum = Convert.ToDouble(dataGridView2.Rows[f.Index].Cells[1].Value);
                            totalcajasvacias = totalcajasvacias + cajasvaciasaum;

                        }
                        totalcajas = totalcajasprod + totalcajasvacias;
                        textBox6.Text = totalcajasvacias.ToString();
                        textBox7.Text = totalcajas.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        



        //MÉTODO AL TERMINAR DE EDITAR UNA CELDA//
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {//>>>>>>>>>>>>>>
            try
            {
                dataGridView1.Columns[1].ReadOnly = true;

                using (OleDbConnection conect = new OleDbConnection(AgenteEntradas.cadConex))
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

                    //if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Lote")
                    //{
                    //    String lote = dataGridView1.CurrentCell.Value.ToString().Trim();
                    //    Regex Val = new Regex(@"^[0-9a-zA-Z]+$");
                    //    if (Val.IsMatch(lote))
                    //    {
                    //        //SendKeys.Send("{RIGHT}");
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("Ingrese un lote válido");
                    //        SendKeys.Send("{UP}");
                    //    }
                    //}

                    //if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Caducidad")
                    //{
                    //    String fcaducidad = dataGridView1.CurrentCell.Value.ToString().Trim();
                    //    if (fcaducidad != "")
                    //    {

                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("Ingrese una formato válido");
                    //        SendKeys.Send("{UP}");
                    //    }
                    //}

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

        public void ObtenerUnidadCapacidad()
        {
            cmd.CommandText = "SELECT unidades.descripcion, subproductos.cantidadenvase FROM Subproductos, Unidades WHERE SubProductos.IdSubProducto = "PT-05-01" AND SubProductos.IdUnidad = Unidades.IdUnidad;";
        }



        //MÉTODO AL TERMINAR DE EDITAR UNA CELDA//
        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {//>>>>>>>>>>>>>>
            try
            {
                dataGridView2.Columns[1].ReadOnly = true;

                using (OleDbConnection conect = new OleDbConnection(AgenteEntradas.cadConex))
                {
                    SendKeys.Send("{UP}");
                    SendKeys.Send("{RIGHT}");

                    //SI LA COLUMNA TIENE DE ENCABEZADO "CLAVE"//
                    /*if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Clave")
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
                    }*/

                    //if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Lote")
                    //{
                    //    String lote = dataGridView1.CurrentCell.Value.ToString().Trim();
                    //    Regex Val = new Regex(@"^[0-9a-zA-Z]+$");
                    //    if (Val.IsMatch(lote))
                    //    {
                    //        //SendKeys.Send("{RIGHT}");
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("Ingrese un lote válido");
                    //        SendKeys.Send("{UP}");
                    //    }
                    //}

                    //if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Caducidad")
                    //{
                    //    String fcaducidad = dataGridView1.CurrentCell.Value.ToString().Trim();
                    //    if (fcaducidad != "")
                    //    {

                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("Ingrese una formato válido");
                    //        SendKeys.Send("{UP}");
                    //    }
                    //}

                    /*if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Piezas")
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
                    }*/

                    if (dataGridView2.Columns[e.ColumnIndex].HeaderText == "Cajas")
                    {
                        String cajas = dataGridView2.CurrentCell.Value.ToString().Trim();
                        Regex Val = new Regex(@"^\d+$");
                        if (Val.IsMatch(cajas))
                        {
                            Double pesocaja2 = EvaluarPesoTipoCaja(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString());
                            Double tara = Convert.ToDouble(cajas) * pesocaja2;
                            dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value = tara;
                        }
                        else
                        {
                            MessageBox.Show("Ingrese un número de cajas válido");
                            SendKeys.Send("{UP}");
                        }
                    }

                    /*if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Peso Bruto")
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
                        }*/

                 }
            }catch (Exception ex)
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
                using (OleDbConnection conect = new OleDbConnection(AgenteEntradas.cadConex))
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
        private void dataGridView2_CellBeginEdit(object sender, DataGridViewCellEventArgs e)
        {
            //String valor = Convert.ToString(dataGridView1.CurrentCell.RowIndex);
            //String valor2 = Convert.ToString(dataGridView1.CurrentCell.ColumnIndex);
            //MessageBox.Show(valor + ", "+ valor2);
            try
            {
                dataGridView1.Columns[1].ReadOnly = true;
                using (OleDbConnection conect = new OleDbConnection(AgenteEntradas.cadConex))
                {
                    if (e.ColumnIndex > -1)
                    {
                        dpbox2 = new DataGridViewComboBoxCell();
                        //SE LLAMA AL MÉTODO OBTENERTIPOSCAJAS PARA LLENAR EL COMBOBOX DEL GRID//
                        if (dataGridView2.Columns[e.ColumnIndex].Name.Contains("descripcióntipocaja"))
                        {
                            dataGridView2[e.ColumnIndex, e.RowIndex] = dpbox2;
                            dpbox2.DataSource = obtenerTiposCajas();

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
