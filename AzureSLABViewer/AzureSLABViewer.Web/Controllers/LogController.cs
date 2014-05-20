using AzureSLABViewer.Web.Models;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureSLABViewer.Web.Controllers
{
    [Authorize]
    public class LogController : Controller
    {
        public ActionResult Details(string pk, string rk)
        {
            if (string.IsNullOrEmpty(pk)
                || string.IsNullOrEmpty(rk))
                return Redirect("/");

            var storageAccount = CloudStorageAccount.Parse(ConfigSettings.StorageConnectionString);

            var tableClient = storageAccount.CreateCloudTableClient();

            var table = tableClient.GetTableReference("SLABLogsTable");

            if (!table.Exists())
                return HttpNotFound();

            var entry = (from ent in table.CreateQuery<SLABLogsTable>()
                         where ent.PartitionKey == pk
                         && ent.RowKey == rk
                         select ent)
                        .FirstOrDefault();

            if (entry == null)
                return HttpNotFound();

            return View(entry);
        }
	}
}