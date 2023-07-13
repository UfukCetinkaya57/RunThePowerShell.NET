using System.Management.Automation;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using WebApplication8.Entities;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    public class Excel : Controller
    {
        public IActionResult Index ()
        {
            return View(new DataInfoViewModel());
        }
        
        [HttpPost]
        [Consumes(contentType: "application/json")]
        public JsonResult RunStartThePowershellWithDataFile([FromBody]DataInfo dataInfo)
        {
            //FormData
            //var invoiceNumberQueryResult= _viewModelBuilder.RunStartThePowershellWithDataFile();
            Business.PowerShellCommand.RunPowerShellcode(dataInfo.siteLink);
            return Json("");
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost]
        public JsonResult RunStartThePowershellWithEntryFile(IFormFile file)
        {
            string[,] result=  Business.PowerShellCommand.ImportExcel(file);
            foreach (var item in result)
            {
                if(item.Length > 0)
                {
                    Console.Write(item);
                    Business.PowerShellCommand.RunPowerShellcode(item);
                }
            }
            return Json("");
        }
    }
}
