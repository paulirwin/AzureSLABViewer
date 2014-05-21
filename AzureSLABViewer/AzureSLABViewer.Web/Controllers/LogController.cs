using AzureSLABViewer.Web.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Queryable;
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

        public ActionResult List(int id, string pk, string rk)
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
        }
    }
}