using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace LeerPdf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnProcesar_Click(object sender, EventArgs e)
        {
            string rutaArchivos = @"C:\Users\BrunoAlonsoPalmaÁvil\OneDrive\Leer PDF excel\Nueva carpeta\Datos comunas\";
            foreach (string file in Directory.EnumerateFiles(rutaArchivos, "*.pdf"))
            {
                string contents = File.ReadAllText(file);


                break;
            }


        }
    }
}
