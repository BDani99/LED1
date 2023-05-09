using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Data.OleDb;


namespace Main
{
    public partial class Form1 : Form
    {
        /* 
         * Arduino műveletek
         *   'A' = Berakás
         *   'B' = Kivétel
         *   'C' = Ellenőrzés
         *   'D' = Leltározás
         */

        // Globál változók létrehozása
        static SerialPort _serialPort;
        int i = 0;
        String item = ""; //Textboxból beolvasott item
        String Adat; //Adatbázis lekérdezés
        DataTable dt; //Beolvasott adatbázis
        bool ok = false;
        bool ok2 = false;
        String lednums = ""; //Az összes polcon lévő item sorszáma

        OleDbConnection con = new OleDbConnection();
        OleDbCommand command;


        public Form1()
        {
            InitializeComponent();

            // Soros porthoz csatlakozás
            _serialPort = new SerialPort();
            _serialPort.PortName = "COM3";
            _serialPort.BaudRate = 9600;

            // Adatbázis csatolása
            con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Benkő Dániel\source\repos\Main\Main\Database1.accdb");
            
            // Az adatok feldolgozása adatbázisból
            con.Open();
            command = new OleDbCommand();
            command.Connection = con;
            Adat = "SELECT LED, Serial FROM Table1";
            command.CommandText = Adat;
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            dt = new DataTable();
            adapter.Fill(dt);
            dataGridView2.DataSource = dt;
            con.Close();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            // A Berakás művelet
            i = -1;
            item = textBox1.Text;
            ok = false;
            ok2 = false;
            string[] serial = new String[dt.Rows.Count];

            // Az üres helyek keresése és az azonos értékek figyelmen kívül hagyása
            while (ok == false && i < dt.Rows.Count - 1 && ok2 != true)
            {
                i++;
                serial[i] = dt.Rows[i][1].ToString();
                if ((!(serial.Contains(item)) && (item != "")))
                {
                    if (dt.Rows[i][1].ToString() == "0")
                    {
                        _serialPort.Close();
                        _serialPort.Open();
                        _serialPort.Write("A" + i.ToString());
                        dt.Rows[i][1] = item;
                        ok = true;
                    }

                }
                else
                {
                    ok2 = true;
                }

            }
            //Ha a temék már megtalálható a polcon
            if (ok2 == true)
            {
                if (item == "") {
                    MessageBox.Show(item + "Nem megfelelő formátum!");
                }
                else
                {
                    MessageBox.Show(item + " nevű termék már behelyezésre került a " + (i + 1) + ". Helyre");
                }
            }
            // Ha a polc televan
            if (!ok && i == dt.Rows.Count-1) { MessageBox.Show("HIBA!!!44!!!NÉGY!!!Nincs több hely a polcon!"); }
            textBox1.Clear();
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // A kivétel művelet
            item =textBox1.Text;
            i = 0;
            ok = false;
            
            // A keresett item kivétele
            while (i < dt.Rows.Count) {
                if (dt.Rows[i][1].ToString() == item)
                {
                    _serialPort.Close();
                    try
                    {
                        _serialPort.Open();
                        _serialPort.Write("B" + i.ToString());
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("{0} Exception caught.", exc);
                    }
                    MessageBox.Show("A keresett cucc a(z) " + (i+1) + ". polcon található");
                    ok = true;
                    dt.Rows[i][1] = "0";

                }
                i++;
                
            }
            if (!ok) { MessageBox.Show("Az elem nem talalható!");
            }
            textBox1.Clear();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Database_Click(object sender, EventArgs e)
        {
           
            Form2 form2 = new Form2();
            form2.Show();
            
        }

        private void művetelekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            leltar.Hide();
            Ell.Hide();
            label1.Show();
            textBox1.Show();
            button_1.Show();
            button2.Show();
            Database.Show();
            dataGridView2.Hide();
        }

        private void leltárEllenőrzésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            leltar.Visible = true;
            Ell.Visible = true;
            textBox1.Hide();
            button_1.Hide();
            button2.Hide();
            Database.Hide();
            dataGridView2.Visible = true;
        }

        private void leltar_Click(object sender, EventArgs e)
        {
            i = 0;
            _serialPort.Close();
            _serialPort.Open();
            lednums = "D";
            while (i < dt.Rows.Count)
            {
                //Nem üres helyek string-be fűzése ','-vel elválasztva.
                if (dt.Rows[i][1].ToString() != "0") {
                    lednums += i.ToString() + ",";
                }
                i++;
            }
            _serialPort.Write(lednums);
        }

        private void Ell_Click(object sender, EventArgs e)
        {
            _serialPort.Close();
            try
            {
                _serialPort.Open();
                _serialPort.Write("C");
            }
            catch (Exception exc)
            {
                Console.WriteLine("{4} Exception caught.", exc);
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'database1DataSet.Table1' table. You can move, or remove it, as needed.
            this.table1TableAdapter.Fill(this.database1DataSet.Table1);

        }

        
    }
}
