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

namespace SSIMS.Controllers
{
    public class CollectionPointsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: CollectionPoints
        public ActionResult Index()
        {
            var collectionpoints = unitOfWork.CollectionPointRepository.Get();
            return View(collectionpoints.ToList());
        }

        // GET: CollectionPoints/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CollectionPoint collectionPoint = unitOfWork.CollectionPointRepository.GetByID(id);
            if (collectionPoint == null)
            {
                return HttpNotFound();
            }
            return View(collectionPoint);
        }

        // GET: CollectionPoints/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CollectionPoints/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CollectionPointID,Location,Time")] CollectionPoint collectionPoint)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.CollectionPointRepository.Insert(collectionPoint);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(collectionPoint);
        }

        // GET: CollectionPoints/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CollectionPoint collectionPoint = unitOfWork.CollectionPointRepository.GetByID(id);
            if (collectionPoint == null)
            {
                return HttpNotFound();
            }
            return View(collectionPoint);
        }

        // POST: CollectionPoints/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CollectionPointID,Location,Time")] CollectionPoint collectionPoint)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.CollectionPointRepository.Update(collectionPoint);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(collectionPoint);
        }

        // GET: CollectionPoints/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CollectionPoint collectionPoint = unitOfWork.CollectionPointRepository.GetByID(id);
            if (collectionPoint == null)
            {
                return HttpNotFound();
            }
            return View(collectionPoint);
        }

        // POST: CollectionPoints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CollectionPoint collectionPoint = unitOfWork.CollectionPointRepository.GetByID(id);
            unitOfWork.CollectionPointRepository.Delete(collectionPoint);
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
