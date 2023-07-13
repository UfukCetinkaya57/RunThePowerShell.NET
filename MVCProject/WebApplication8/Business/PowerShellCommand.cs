using System.Management.Automation;
using System.Text;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using WebApplication8.Entities;

namespace WebApplication8.Business
{
    public static class PowerShellCommand
    {
        private static PowerShell ps = PowerShell.Create();

        public static string RunPowerShellcode(string script) 
        {
            script = $"Start {script}";
            string errorMsg = string.Empty;
            string output = string.Empty;
            ps.AddScript(script);

            ps.AddCommand("Out-String");

            PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();
            ps.Streams.Error.DataAdded += (object sender, DataAddedEventArgs e) =>
            {
                errorMsg = ((PSDataCollection<ErrorRecord>)sender)[e.Index].ToString();
            };

            IAsyncResult result = ps.BeginInvoke<PSObject, PSObject>(null, outputCollection);
            ps.EndInvoke(result);

            //foreach (var outputItem in outputCollection)
            //{
            //    sb.AppendLine(outputItem.BaseObject.ToString());
            //}
            ps.Commands.Clear();
            if (!string.IsNullOrEmpty(errorMsg))
                return errorMsg;
            return "Success";
        }

        public static string[,] ImportExcel(IFormFile file)
        {
            //[Route("run/powershell")]
            string[,] Commands;

            // System.Data.DataTable dt = new System.Data.DataTable();
            // Ekranda bir alan o alana veri girilmişse o alandan olacak değilse
            // Excel dosyası zorunlu yüklenecek (buton if)
            // RunStartWebPageThePowershell fonksiyonlu isim
            // Private fonksion tarzında olmalı
            // String dizisi döndüren fonksiyon oluşturulucak
            // Veri girilmişse direkt verinin valuesunu alıcak
            // Javascript fetch post request gönderilebilir (front)
            // Sayfa yenilenmesin (alert)

            // excel dosyamızı stream'e çeviriyoruz
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);

                // excel dosyamızı streamden okuyoruz
                using (var workbook = new XLWorkbook(ms))
                {
                    int i, n;
                    var worksheet = workbook.Worksheet(1); // sayfa 1

                    //Sayfada kaç sütun kullanılmış onu buluyoruz ve sütunları DataTable'a ekliyoruz, ilk satırda sütun başlıklarımız var
                    //for (i = 1; i <= n; i++)
                    //{
                    //dt.Columns.Add(worksheet.Cell(1, i).Value.ToString());
                    //}

                    //sayfada kaç satır kullanılmış onu buluyoruz ve DataTable'a satırlarımızı ekliyoruz
                    n = worksheet.Rows().Count();
                    int j, k = worksheet.Columns().Count();

                    Commands = new string[n, k];

                    for (i = 1; i <= n; i++)
                    {
                        for (j = 1; j <= k; j++)
                        {
                            // i= satır index, j=sütun index, closedXML worksheet için indexler 1'den başlıyor, ama datatable için 0'dan başladığı için j-1 diyoruz

                            Commands[i - 1, j - 1] = (worksheet.Cell(i, j).Value).ToString();
                            //Console.WriteLine(worksheet.Cell(i, j));

                        }
                    }
                }
            }

            return Commands;
        }
    }
}
