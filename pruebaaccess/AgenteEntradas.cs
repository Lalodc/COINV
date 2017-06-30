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


        private void button3_Click(object sender, EventArgs e)
        {
            Inicio frmInicio = new Inicio();
            frmInicio.Show();
            this.Hide();
        }
        
    }
}
