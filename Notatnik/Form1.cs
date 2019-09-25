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

        private void ustawieniaStronyToolStripMenuItem_Click(object sender, EventArgs e)
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
                tekstZmieniony = false;
            }
        }

        private string[] CzytajPlikTekstowy(string nazwaPliku)
        {
            throw new NotImplementedException();
        }
    }
}
