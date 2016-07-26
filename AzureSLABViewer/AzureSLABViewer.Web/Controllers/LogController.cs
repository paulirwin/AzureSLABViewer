using System.Configuration;
using AzureSLABViewer.Web.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Queryable;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using System.Web;

namespace AzureSLABViewer.Web.Controllers
{
    public class LogController : Controller
    {
        private static readonly string _storageConnection = ConfigurationManager.AppSettings["StorageConnection"] ?? "UseDevelopmentStorage=true";
        private static readonly string _siteTitle = ConfigurationManager.AppSettings["SiteTitle"] ?? "Azure SLAB Logs";
        private CloudStorageAccount _storageAccount;
        private CloudTableClient _tableClient;
        private const string cookieName = "SelectedSLABLogTable";
        private HttpCookie _slabViewerCookie;

        public LogController()
        {
            _storageAccount = CloudStorageAccount.Parse(_storageConnection);
            _tableClient = _storageAccount.CreateCloudTableClient();
        }

        public ActionResult Details(string pk, string rk)
        {
            _slabViewerCookie = Request.Cookies[cookieName];

            if (string.IsNullOrEmpty(pk) ||
                string.IsNullOrEmpty(rk) ||
                _slabViewerCookie == null)
            {
                return Redirect("/");
            }
            
            var table = _tableClient.GetTableReference(_slabViewerCookie.Value);

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
            var selectedSlabLog = string.Empty;

            _slabViewerCookie = Request.Cookies[cookieName];
            
            if (_slabViewerCookie != null)
            {
                selectedSlabLog = _slabViewerCookie.Value;
            }

            var tables = _tableClient.ListTables()
                                            .Where(t => t.Name.ToLower().Contains("logstable"))
                                            .OrderBy(t => t.Name);

            var vm = SLABLogListViewModel.Create(new List<SlabLogsTable>(), tables, selectedSlabLog);
        
            if (string.IsNullOrEmpty(selectedSlabLog))
            {
                ViewBag.HasNextPage = false;
                return View(vm);
            }
            else
            {
                var table = _tableClient.GetTableReference(selectedSlabLog);

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

                vm.SLABLogItems = results;

                return View(vm);
            }

        }


        [HttpPost]
        public ActionResult List(string selectedSLABLogTable)
        {
            var slabViewerCookie = new HttpCookie(cookieName);
            slabViewerCookie.Value = selectedSLABLogTable;
            slabViewerCookie.Expires = DateTime.Now.AddYears(1); //whenever...

            Response.Cookies.Add(slabViewerCookie);

            return RedirectToAction("List");
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