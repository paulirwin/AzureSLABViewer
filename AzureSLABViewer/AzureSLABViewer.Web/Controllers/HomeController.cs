using AzureSLABViewer.Web.Models;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureSLABViewer.Web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigSettings.StorageConnectionString);

            var tableClient = storageAccount.CreateCloudTableClient();

            var table = tableClient.GetTableReference("SLABLogsTable");

            if (!table.Exists())
                return View("TableDoesntExist");

            var query = (from ent in table.CreateQuery<SLABLogsTable>()
                         select ent)
                        .Take(100)
                        .ToList();

            return View(query);
        }
    }
}