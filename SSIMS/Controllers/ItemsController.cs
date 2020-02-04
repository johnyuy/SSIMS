using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SSIMS.Database;
using SSIMS.Models;
using SSIMS.DAL;
using PagedList;

namespace SSIMS.Controllers
{
    public class ItemsController : Controller
    {   
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Items
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //ViewBag.ItemDescriptionSortParm = String.IsNullOrEmpty(sortOrder) ? "Item_desc" : "";
            ViewBag.ItemDescriptionSortParm = String.IsNullOrEmpty(sortOrder) ? "Item_desc" : "";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "Category_desc" : "Category";
            var items = unitOfWork.ItemRepository.Get();

            if ( searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(i => i.Description.ToUpper().Contains(searchString.ToUpper())
                                       || i.Category.ToUpper().Contains(searchString.ToUpper())
                                       || i.Category.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "Item_desc":
                    items = items.OrderByDescending(i => i.Description);
                    break;
                case "Category":
                    items = items.OrderBy(i => i.Category);
                    break;
                case "Category_desc":
                    items = items.OrderByDescending(i => i.Category);
                    break;
                default:
                    items = items.OrderBy(i => i.Description);
                    break;
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Items/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = unitOfWork.ItemRepository.GetByID(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Category,Description,UnitOfMeasure")] Item item)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.ItemRepository.Insert(item);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(item);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = unitOfWork.ItemRepository.GetByID(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Category,Description,UnitOfMeasure")] Item item)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.ItemRepository.Update(item);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = unitOfWork.ItemRepository.GetByID(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Item item = unitOfWork.ItemRepository.GetByID(id);
            unitOfWork.ItemRepository.Delete(item);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
