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
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text.RegularExpressions;

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
            int nro = 0;
            string rutaArchivos = @"C:\Users\BrunoAlonsoPalmaÁvil\OneDrive\Leer PDF excel\Nueva carpeta\Datos comunas\";
            foreach (string file in Directory.EnumerateFiles(rutaArchivos, "*.pdf"))
            {
                string contents = File.ReadAllText(file);
                var nombreArchivo = System.IO.Path.GetFileNameWithoutExtension(file);
                PdfReader reader = new PdfReader(file);

                StringWriter output = new StringWriter();
                string ciudad = "";
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    
                    output.WriteLine(PdfTextExtractor.GetTextFromPage(reader, i, new SimpleTextExtractionStrategy()));
                    if (i == 1)
                    {


                        ciudad = output.ToString().Split('\n')[3].Substring(13);
                        var provincia = output.ToString().Split('\n')[7];
                        //^(\s\d)([a-zA-Z\s]*)(\d{1,2}.\d{3}.\d{3}-\d{1})\s(MUJ|VAR)\s([a-zA-Z0-9,.!? ]*)(ALTO HOSPICIO)(\s\d*\sM|V)$ 
                        //^(\s\d)([a-zA-Z\s]*)(\d{1,2}.\d{3}.\d{3}-\d{1})\s(MUJ|VAR)\s([a-zA-Z0-9,.!? ]*.+?)(ALTO HOSPICIO)
                        //^(\s\d)([a-zA-Z\s]*)(\d{1,2}.\d{3}.\d{3}-\d{1})\s(MUJ|VAR)\s([a-zA-Z0-9,.!? ]*)(ALTO HOSPICIO)\s(\d*\s{1}V{1})$
                        //^(\s\d)([a-zA-Z\s]*)(\d{1,2}.\d{3}.\d{3}-\d{1})\s(MUJ|VAR)\s([a-zA-Z0-9,.!? ]*)(ALTO HOSPICIO)\s(\d*\s*\s(V|M))$


                    }

                }
                string resultado = output.ToString();

                string[] arrString = output.ToString().Split('\n');
                for(var i = 0; i < arrString.Length; i++)
                {
                    Regex regex = new Regex(@"\d{1,2}.\d{3}.\d{3}-\d{1}");
                    //Match match = regex.Match(arrString[i]);
                    string pattern = @"(\s\d)([a-zA-Z\s]*)(\d{1,2}.\d{3}.\d{3}-\d{1})\s(MUJ|VAR)\s([a-zA-Z0-9,.!? ]*)("+ciudad+@")\s(\d*\s*\s(V|M))";
                    MatchCollection matches = Regex.Matches(arrString[i], pattern);

                    foreach (Match match in matches)
                    { 
                        var nombre = match.Groups[2].Value;
                        var rut = match.Groups[3].Value;
                        var genero = match.Groups[4].Value;
                        var direccion = match.Groups[5].Value;
                        var ciudadP = match.Groups[6].Value;
                        nro++;
                    }
                    //if (match.Success)
                    //{
                      //  Console.WriteLine(match.Value);
                    //}
                }
                
                
            }
            var asdf = "";

        }
    }
}
