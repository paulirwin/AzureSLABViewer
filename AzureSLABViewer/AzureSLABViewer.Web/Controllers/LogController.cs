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
        public ActionResult Details(int id, string pk, string rk)
        {
            if (string.IsNullOrEmpty(pk)
                || string.IsNullOrEmpty(rk))
                return Redirect("/");

            using (var context = new ApplicationDbContext())
            {
                var connection = context.StorageConnections.Find(id);

                if (connection == null)
                    return HttpNotFound();

                ViewBag.ConnectionID = id;

                var storageAccount = CloudStorageAccount.Parse(connection.ConnectionString);

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

        public ActionResult List(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var connection = context.StorageConnections.Find(id);

                if (connection == null)
                    return HttpNotFound();

                ViewBag.ConnectionName = connection.DisplayName;
                ViewBag.ConnectionID = id;

                var storageAccount = CloudStorageAccount.Parse(connection.ConnectionString);

                var tableClient = storageAccount.CreateCloudTableClient();

                var table = tableClient.GetTableReference("SLABLogsTable");

                if (!table.Exists())
                    return View("TableDoesntExist");

                var query = (from ent in table.CreateQuery<SLABLogsTable>()
                             select ent)
                            .Take(100)
                            .ToList()
                            .OrderByDescending(i => i.EventDate); // added to handle issue when rowkey not deterministic across apps

                return View(query);
            }
        }
	}
}