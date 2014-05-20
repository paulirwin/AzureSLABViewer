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
            using (var context = new ApplicationDbContext())
            {
                var connections = context.StorageConnections.OrderBy(i => i.DisplayName).ToList();

                return View(connections);
            }
        }
    }
}