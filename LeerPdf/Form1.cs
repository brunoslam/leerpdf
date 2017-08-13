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
using System.Data.SqlClient;

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
            DataTable workTable = new DataTable("Persona");
            

            workTable.Columns.Add("Nombre", typeof(String));
            workTable.Columns.Add("Rut", typeof(String));
            workTable.Columns.Add("Ciudad", typeof(String));
            workTable.Columns.Add("Genero", typeof(String));
            workTable.Columns.Add("Direccion", typeof(String));

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
                    string pattern = @"(\s\d*)([a-zA-Z\s]*)(\d{1,2}.\d{3}.\d{3}-\d{1})\s(MUJ|VAR)\s([a-zA-Z0-9,.!? ]*)("+ciudad+@")\s(\d*\s*\s(V|M))";
                    MatchCollection matches = Regex.Matches(arrString[i], pattern);

                    foreach (Match match in matches)
                    { 
                        var nombre = match.Groups[2].Value;
                        var rut = match.Groups[3].Value;
                        var genero = match.Groups[4].Value;
                        var direccion = match.Groups[5].Value;
                        var ciudadP = match.Groups[6].Value;
                        nro++;


                        DataRow workRow;
                        if (nombre == "")
                        {
                            var asdx = "";
                        }

                        workRow = workTable.NewRow();
                        workRow["Nombre"] = nombre;
                        workRow["Rut"] = rut;
                        workRow["Genero"] = genero;
                        workRow["Direccion"] = direccion;
                        workRow["Ciudad"] = ciudad;
                        workTable.Rows.Add(workRow);

                    }
                    //if (match.Success)
                    //{
                      //  Console.WriteLine(match.Value);
                    //}
                }
                
                
            }
            String connString = "Server=tcp:sqllatin.database.windows.net,1433;Initial Catalog=sqllatin;Persist Security Info=False;User ID=latinadmin;Password=Latin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            // connect to SQL
            using (SqlConnection connection =
                    new SqlConnection(connString))
            {
                // make sure to enable triggers
                // more on triggers in next post
                SqlBulkCopy bulkCopy =
                    new SqlBulkCopy
                    (
                    connection,
                    SqlBulkCopyOptions.TableLock |
                    SqlBulkCopyOptions.FireTriggers |
                    SqlBulkCopyOptions.UseInternalTransaction,
                    null
                    );

                // set the destination table name
                bulkCopy.DestinationTableName = "Persona";
                connection.Open();

                // write the data in the "dataTable"
                //bulkCopy.WriteToServer(workTable);
                connection.Close();
            }
            // reset
            //this.dataTable.Clear();
            var asdf = "";

        }
    }
}
