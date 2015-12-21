using System.Configuration;
using AzureSLABViewer.Web.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Queryable;
using System.Linq;
using System.Web.Mvc;

namespace AzureSLABViewer.Web.Controllers
{
    public class LogController : Controller
    {
        private static readonly string _storageConnection = ConfigurationManager.AppSettings["StorageConnection"] ?? "UseDevelopmentStorage=true";
        private static readonly string _siteTitle = ConfigurationManager.AppSettings["SiteTitle"] ?? "Azure SLAB Logs";

        public ActionResult Details(string pk, string rk)
        {
            if (string.IsNullOrEmpty(pk)
                || string.IsNullOrEmpty(rk))
                return Redirect("/");

            var storageAccount = CloudStorageAccount.Parse(_storageConnection);

            var tableClient = storageAccount.CreateCloudTableClient();

            var table = tableClient.GetTableReference("SlabLogsTable");

            if (!table.Exists())
                return HttpNotFound();

            var entry = (from ent in table.CreateQuery<SlabLogsTable>()
                         where ent.PartitionKey == pk
                         && ent.RowKey == rk
                         select ent)
                        .FirstOrDefault();

            if (entry == null)
                return HttpNotFound();

            return View(entry);
        }

        public ActionResult List(string pk, string rk)
        {
            var storageAccount = CloudStorageAccount.Parse(_storageConnection);

            var tableClient = storageAccount.CreateCloudTableClient();

            var table = tableClient.GetTableReference("SlabLogsTable");

            if (!table.Exists())
                return HttpNotFound();

            var query = (from ent in table.CreateQuery<SlabLogsTable>()
                         select ent)
                         .Take(100)
                         .AsTableQuery();

            TableContinuationToken token = null;

            if (!string.IsNullOrEmpty(pk) && !string.IsNullOrEmpty(rk))
                token = new TableContinuationToken() { NextPartitionKey = pk, NextRowKey = rk };

            var queryResult = query.ExecuteSegmented(token);

            var results = queryResult
                            .ToList()
                            .OrderByDescending(i => i.EventDate); // added to handle issue when rowkey not deterministic across apps

            if (queryResult.ContinuationToken != null)
            {
                ViewBag.HasNextPage = true;
                ViewBag.NextPartitionKey = queryResult.ContinuationToken.NextPartitionKey;
                ViewBag.NextRowKey = queryResult.ContinuationToken.NextRowKey;
            }
            else
                ViewBag.HasNextPage = false;

            return View(results);
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            ViewBag.SiteTitle = _siteTitle;
            return base.View(viewName, masterName, model);
        }

        protected override ViewResult View(IView view, object model)
        {
            ViewBag.SiteTitle = _siteTitle;
            return base.View(view, model);
        }
    }
}