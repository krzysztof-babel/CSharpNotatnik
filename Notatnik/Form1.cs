using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notatnik
{
    public partial class Form1 : Form
    {
        bool tekstZmieniony = false;
        //constructor
        public Form1()
        {
            InitializeComponent();
        }

        private void Notatnik_Load(object sender, EventArgs e)
        {

        }

        private void ądWydrukuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        
        #region Metoda Zamknij
        private void zamknijToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //close application
            Close();
        }
        #endregion

        private void plikToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #region Metoda zapiszJako
        private void zapiszJakoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string nazwaPliku = openFileDialog1.FileName;
            if (nazwaPliku.Length > 0) saveFileDialog1.FileName = nazwaPliku;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                nazwaPliku = saveFileDialog1.FileName;
                ZapiszDoPlikuTekstowego(nazwaPliku, textBox1.Lines);
                int ostatniSlash = nazwaPliku.LastIndexOf('\\');
                toolStripStatusLabel1.Text = nazwaPliku.Substring(ostatniSlash + 1, nazwaPliku.Length - ostatniSlash - 1);
                tekstZmieniony = false;
            }
        }

        public void ZapiszDoPlikuTekstowego(string nazwaPliku, string[] tekst)
        {
            using (StreamWriter sw = new StreamWriter(nazwaPliku))
            {
                foreach (string wiersz in tekst)
                {
                    sw.WriteLine(wiersz);
                }
            }
        }
        #endregion

        public void otworzToolStripMenuItem(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string nazwaPliku = openFileDialog1.FileName;
                textBox1.Lines = CzytajPlikTekstowy(nazwaPliku);
                int ostatniSlash = nazwaPliku.LastIndexOf('\\');
                toolStripStatusLabel1.Text = nazwaPliku.Substring(ostatniSlash + 1, nazwaPliku.Length - 1);
                toolStripStatusLabel1.Text = nazwaPliku.Substring(ostatniSlash + 1, nazwaPliku.Length - ostatniSlash - 1);
                tekstZmieniony = false;
            }
        }

        public static string[] CzytajPlikTekstowy(string nazwaPliku)
        {
            List<string> tekst = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(nazwaPliku))
                {
                    string wiersz;
                    while ((wiersz = sr.ReadLine()) != null)
                        tekst.Add(wiersz);
                }
                return tekst.ToArray();
            }
            catch (Exception e)
            {

                MessageBox.Show("Błąd odczytu pliku" + nazwaPliku + "\nOpis wyjątku: " + e.Message, "Notatnik - Błąd przy wczytywaniu pliku", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        #region FormClosing
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!tekstZmieniony) return;
            {
                DialogResult dr = MessageBox.Show("Czy zapisać zmiany w edytowanym dokumencie", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);

                switch (dr)
                {
                    case DialogResult.Yes:
                        zapiszJakoToolStripMenuItem_Click
                            (null, null); break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    default: e.Cancel = true;
                        break;

                    /*case DialogResult.None:
                        break;
                    case DialogResult.OK:
                        break;
                    case DialogResult.Cancel:
                        break;
                    case DialogResult.Abort:
                        break;
                    case DialogResult.Retry:
                        break;
                    case DialogResult.Ignore:
                        break;
                    case DialogResult.Yes:
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        break;*/
                }
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            tekstZmieniony = true;
        }
        #endregion

        private void ustawieniaStronyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void drukujToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #region drukowanie

        private StreamReader sr = null;
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font czcionka = textBox1.Font;
            int wysokoscWiersza = (int) czcionka.GetHeight(e.Graphics);
            int iloscLinii = e.MarginBounds.Height / wysokoscWiersza;

            //pierwsza strona - dzielenie linii (dbamy o zachowanie całych wyrazów)
            if (sr == null)
            {
                string tekst = "";
                foreach  (string wiersz in textBox1.Lines)
                {
                    float szerokosc = e.Graphics.MeasureString(wiersz, czcionka).Width;
                    if (szerokosc < e.MarginBounds.Width)
                    {
                        tekst += wiersz + "\n";
                    }
                    else
                    {
                        float sredniaSzerokoscLitery = szerokosc / wiersz.Length;
                        int ileLiterWWierszu = (int)(e.MarginBounds.Width / sredniaSzerokoscLitery);
                        string skracanyWiersz = wiersz;
                        do
                        {
                            int ostatniaSpacja = skracanyWiersz.Substring(0, ileLiterWWierszu).LastIndexOf(' ');
                            int iloscLiter = ostatniaSpacja != -1 ? Math.Min(ostatniaSpacja, ileLiterWWierszu) : ileLiterWWierszu;
                            tekst += skracanyWiersz.Substring(0, iloscLiter).TrimStart(' '); //pozostala czesc wiersza
                        } while (skracanyWiersz.Length > ileLiterWWierszu);

                        tekst += skracanyWiersz + "\n"; //ostatnia czesc
                    } //if-else
                } //foreach
                sr = new StreamReader(tekst);
            } //if (sr == null)

            e.HasMorePages = true;
            for (int i = 0; i < iloscLinii; i++)
            {
                string wiersz = sr.ReadLine();
                if (wiersz == null)
                {
                    e.HasMorePages = false;
                    sr = null;
                    break;
                }
                e.Graphics.DrawString(wiersz, czcionka, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top + i);
            }
        }
        #endregion
    } //class
} //namespace
