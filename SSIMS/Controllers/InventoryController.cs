using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using SSIMS.DAL;
using SSIMS.Database;
using SSIMS.Models;
using SSIMS.ViewModels;
using SSIMS.Filters;

namespace SSIMS.Controllers
{
    [AuthenticationFilter]
    [AuthorizationFilter]
    public class InventoryController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index(string searchString, string lowStock)
        {
            Debug.WriteLine("searchString = " + searchString + "\nlowStock = " + lowStock);
            bool low = lowStock == "true" ? true : false;
            InventoryViewModel inventoryViewModel =  new InventoryViewModel(searchString, low);
            ViewBag.LowStock = lowStock;
            ViewBag.SearchString = searchString;
            return View(inventoryViewModel.inventoryItems.ToList());
        }

        // GET: Inventory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryItem inventoryItem = db.InventoryItems.Find(id);
            if (inventoryItem == null)
            {
                return HttpNotFound();
            }
            return View(inventoryItem);
        }

        // GET: Inventory/Create
        public ActionResult Create()
        {
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Category");
            return View();
        }

        // POST: Inventory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ItemID,InStoreQty,InTransitQty,ReorderLvl,ReorderQty,StockCheck")] InventoryItem inventoryItem)
        {
            if (ModelState.IsValid)
            {
                db.InventoryItems.Add(inventoryItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemID = new SelectList(db.Items, "ID", "Category", inventoryItem.ItemID);
            return View(inventoryItem);
        }

        // GET: Inventory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryItem inventoryItem = db.InventoryItems.Find(id);
            if (inventoryItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Category", inventoryItem.ItemID);
            return View(inventoryItem);
        }

        // POST: Inventory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ItemID,InStoreQty,InTransitQty,ReorderLvl,ReorderQty,StockCheck")] InventoryItem inventoryItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inventoryItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Category", inventoryItem.ItemID);
            return View(inventoryItem);
        }

        // GET: Inventory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryItem inventoryItem = db.InventoryItems.Find(id);
            if (inventoryItem == null)
            {
                return HttpNotFound();
            }
            return View(inventoryItem);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InventoryItem inventoryItem = db.InventoryItems.Find(id);
            db.InventoryItems.Remove(inventoryItem);
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
