using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using ClosedXML.Excel;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Management.Automation;
using WebApplication8.Models;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace WebApplication8.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static PowerShell ps = PowerShell.Create();

     
        public static string Command(string script)
        {
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
            StringBuilder sb = new StringBuilder();

            //foreach (var outputItem in outputCollection)
            //{
            //    sb.AppendLine(outputItem.BaseObject.ToString());
            //}
            Console.WriteLine(sb.ToString());
            ps.Commands.Clear();
            if (!string.IsNullOrEmpty(errorMsg))
                return errorMsg;
            return sb.ToString().Trim();
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public void DataInput2()
        {
            Console.WriteLine("33331");
        }
        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult ImportExcelGet(/*IFormFile file*/ )
        {            
            return View();
        }

        public IActionResult Excel(/*IFormFile file*/ )
        {
            return View();
        }
    }
}