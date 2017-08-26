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

using iTextSharp.text.pdf;
using iTextSharp;
using System.Diagnostics;

namespace LeerPdf
{
    public partial class TiempoTranscurrido : Form
    {
        public TiempoTranscurrido()
        {
            InitializeComponent();
            btnProcesar.PerformClick();
        }

        private void btnProcesar_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int nro = 0;
            DataTable workTable = new DataTable("Persona");
            DataTable personaTable = new DataTable("Persona");

            personaTable.Columns.Add("Nombre", typeof(String));
            personaTable.Columns.Add("Rut", typeof(String));

            personaTable.Columns.Add("Genero", typeof(String));
            personaTable.Columns.Add("Direccion", typeof(String));
            personaTable.Columns.Add("Circunscripcion", typeof(String));
            personaTable.Columns.Add("Region", typeof(String));
            personaTable.Columns.Add("Provincia", typeof(String));
            personaTable.Columns.Add("Ciudad", typeof(String));

            string rutaArchivos = @"C:\Users\BrunoAlonsoPalmaÁvil\Desktop\Escritorio\Leer PDF excel\Nueva carpeta\Datos comunas\asd\";
            foreach (string file in Directory.EnumerateFiles(rutaArchivos, "*.pdf"))
            {
                Application.DoEvents();
                //string contents = File.ReadAllText(file);
                var nombreArchivo = System.IO.Path.GetFileNameWithoutExtension(file);
                PdfReader reader = new PdfReader(file);

                StringWriter output = null;
                StringWriter outputBytesPage = null;

                string ciudad = "";
                string region = "";
                string provincia = "";
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {

                    ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();

                    PdfObject obj = reader.GetPdfObject(i);

                    //var asd = ExtractTextFromPDFBytes(reader.GetPageContent(i));

                    byte[] contentBytes = iTextSharp.text.pdf.parser.ContentByteUtils.GetContentBytesForPage(reader, i);

                    //outputBytes.WriteLine(System.Text.Encoding.UTF8.GetString(contentBytes));
                    outputBytesPage = new StringWriter();
                    outputBytesPage.WriteLine(System.Text.Encoding.UTF8.GetString(contentBytes));
                    // Bt\\n[A-Z a-z \s\d-\\/()'Ñ.\[\]]*scStr
                    // BT\\n[A-Z a-z \s\d-\\\/()'Ñ.\[\]]*sc[A-Z a-z \s\d-\\\/()'Ñ.\[\]]*ET
                    // BT\\n[A-Z a-z \s\d-\\\/()'Ñ.\[\]�]*sc
                    if (i == 1)
                    {
                        output = new StringWriter();
                        output.WriteLine(PdfTextExtractor.GetTextFromPage(reader, i, new SimpleTextExtractionStrategy()));
                        try
                        {
                            ciudad = output.ToString().Split('\n')[3].Substring(13);
                            region = output.ToString().Split('\n')[7];
                            provincia = output.ToString().Split('\n')[4];
                            //^(\s\d)([a-zA-Z\s]*)(\d{1,2}.\d{3}.\d{3}-\d{1})\s(MUJ|VAR)\s([a-zA-Z0-9,.!? ]*)(ALTO HOSPICIO)(\s\d*\sM|V)$ 
                            //^(\s\d)([a-zA-Z\s]*)(\d{1,2}.\d{3}.\d{3}-\d{1})\s(MUJ|VAR)\s([a-zA-Z0-9,.!? ]*.+?)(ALTO HOSPICIO)
                            //^(\s\d)([a-zA-Z\s]*)(\d{1,2}.\d{3}.\d{3}-\d{1})\s(MUJ|VAR)\s([a-zA-Z0-9,.!? ]*)(ALTO HOSPICIO)\s(\d*\s{1}V{1})$
                            //^(\s\d)([a-zA-Z\s]*)(\d{1,2}.\d{3}.\d{3}-\d{1})\s(MUJ|VAR)\s([a-zA-Z0-9,.!? ]*)(ALTO HOSPICIO)\s(\d*\s*\s(V|M))$
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    string resultadoPaginaBytes = outputBytesPage.ToString();
                    //(?=BT).*?(?<=sc\\n)
                    //(?=BT).*?(?<=sc\\n)
                    Regex regexPag = new Regex("(?=BT).*?(?<=sc\\n)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    Match matchpag = regexPag.Match(resultadoPaginaBytes);
                    if (matchpag.Success)
                    {
                        //.WriteLine(matchpag.Value);
                    }
                    string pattern = "(?=BT).{200,}?(?<=sc\\n)";
                    MatchCollection matches = Regex.Matches(resultadoPaginaBytes, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                    foreach (Match match in matches)
                    {
                        if (!match.ToString().Contains("CIRCUNSCRIPCION") && !match.ToString().Contains("VICIO ELEC"))
                        {
                            //var nombre = match.ToString();

                            string pattern2 = "(?=BT).*?(?<=ET\\n)";
                            DataRow personaRow = personaTable.NewRow();
                            personaRow["Region"] = region.Substring(0, region.IndexOf(":") - 1);
                            personaRow["Provincia"] = provincia;
                            personaRow["Ciudad"] = ciudad;
                            MatchCollection matches2 = Regex.Matches(match.ToString(), pattern2, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            int contador = 0;
                            foreach (Match match2 in matches2)
                            {
                                contador++;
                                string pattern3 = @"\((.*?)\)";
                                MatchCollection matches3 = Regex.Matches(match2.ToString(), pattern3, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                string valor = "";


                                foreach (Match match3 in matches3)
                                {
                                    valor += match3.Groups[1].ToString().Replace("�", "Ñ");
                                }
                                Boolean flag = false;
                                if (matches2.Count != 7)
                                {
                                    flag = true;
                                }

                                Application.DoEvents();

                                if (contador == 1)
                                {
                                    if (valor == "")
                                    {
                                        valor = match2.ToString().Substring(match2.ToString().IndexOf("(") + 1);
                                    }

                                    if (valor == "")
                                    {

                                    }
                                    if (valor.Length < 10)
                                    {

                                    }
                                    personaRow["Nombre"] = valor;
                                }
                                else if (contador == 2)
                                {
                                    personaRow["Rut"] = valor;
                                }
                                else if (contador == 3)
                                {
                                    personaRow["Genero"] = valor;
                                }
                                else if (contador == 4)
                                {
                                    if (valor == "")
                                    {

                                    }
                                    personaRow["Direccion"] = valor;
                                }
                                else if (contador == 5)
                                {
                                    if (flag && valor == "EL TRANSITO")
                                    {
                                        Console.WriteLine(valor);
                                    }
                                    if (valor.Length < 5 && matches2.Count == 6 && ciudad != "CAMIÑA")
                                    {

                                    }
                                    else if (valor.Length < 5 && matches2.Count == 7)
                                    {

                                    }
                                    personaRow["Circunscripcion"] = valor;
                                }
                            }
                            personaTable.Rows.Add(personaRow);
                            if (personaTable.Rows.Count == 500000)
                            {
                                InsertarBBDD(personaTable);
                                personaTable.Rows.Clear();
                            }
                        }
                    }

                }

            }
            InsertarBBDD(personaTable);
            stopwatch.Stop();
            lblTiempoTranscurrido.Text = stopwatch.Elapsed.TotalMinutes.ToString(); ;
           

        }

        public void InsertarBBDD(DataTable personaTable)
        {
            //String connString = @"Server=tcp:sqllatin.database.windows.net,1433;Initial Catalog=sqllatin;Persist Security Info=False;User ID=latinadmin;Password=Latin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            String connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonaPdf;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
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
                bulkCopy.BulkCopyTimeout = 0;
                bulkCopy.BatchSize = personaTable.Rows.Count;
                connection.Open();

                // write the data in the "dataTable"
                bulkCopy.WriteToServer(personaTable);
                connection.Close();
            }
        }
    }



}
