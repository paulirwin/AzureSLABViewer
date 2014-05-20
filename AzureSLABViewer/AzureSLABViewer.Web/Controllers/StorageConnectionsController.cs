using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AzureSLABViewer.Web.Models;

namespace AzureSLABViewer.Web.Controllers
{
    [Authorize]
    public class StorageConnectionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /StorageConnections/
        public ActionResult Index()
        {
            return View(db.StorageConnections.ToList());
        }

        // GET: /StorageConnections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StorageConnection storageconnection = db.StorageConnections.Find(id);
            if (storageconnection == null)
            {
                return HttpNotFound();
            }
            return View(storageconnection);
        }

        // GET: /StorageConnections/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /StorageConnections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="StorageConnectionID,DisplayName,ConnectionString")] StorageConnection storageconnection)
        {
            if (ModelState.IsValid)
            {
                db.StorageConnections.Add(storageconnection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(storageconnection);
        }

        // GET: /StorageConnections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StorageConnection storageconnection = db.StorageConnections.Find(id);
            if (storageconnection == null)
            {
                return HttpNotFound();
            }
            return View(storageconnection);
        }

        // POST: /StorageConnections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="StorageConnectionID,DisplayName,ConnectionString")] StorageConnection storageconnection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(storageconnection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(storageconnection);
        }

        // GET: /StorageConnections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StorageConnection storageconnection = db.StorageConnections.Find(id);
            if (storageconnection == null)
            {
                return HttpNotFound();
            }
            return View(storageconnection);
        }

        // POST: /StorageConnections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StorageConnection storageconnection = db.StorageConnections.Find(id);
            db.StorageConnections.Remove(storageconnection);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
