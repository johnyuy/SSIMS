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

namespace SSIMS.Controllers
{
    public class DepartmentsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Departments
        public ActionResult Index()
        {
            var departments = db.Departments.Include(d => d.CollectionPoint).Include(d => d.DeptHead).Include(d => d.DeptRep);
            return View(departments.ToList());
        }

        // GET: Departments/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            ViewBag.CollectionPointID = new SelectList(db.CollectionPoints, "ID", "Location");
            ViewBag.DeptHeadID = new SelectList(db.Staffs, "ID", "UserAccountID");
            ViewBag.DeptRepID = new SelectList(db.Staffs, "ID", "UserAccountID");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DeptRepID,DeptHeadID,CollectionPointID,DeptHeadAutorizationID,DeptName,PhoneNumber,FaxNumber")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CollectionPointID = new SelectList(db.CollectionPoints, "ID", "Location", department.CollectionPoint.ID);
            ViewBag.DeptHeadID = new SelectList(db.Staffs, "ID", "UserAccountID", department.DeptHead.ID);
            ViewBag.DeptRepID = new SelectList(db.Staffs, "ID", "UserAccountID", department.DeptRep.ID);
            return View(department);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.CollectionPointID = new SelectList(db.CollectionPoints, "ID", "Location", department.CollectionPoint.ID);
            ViewBag.DeptHeadID = new SelectList(db.Staffs, "ID", "UserAccountID", department.DeptHead.ID);
            ViewBag.DeptRepID = new SelectList(db.Staffs, "ID", "UserAccountID", department.DeptRep.ID);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DeptRepID,DeptHeadID,CollectionPointID,DeptHeadAutorizationID,DeptName,PhoneNumber,FaxNumber")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CollectionPointID = new SelectList(db.CollectionPoints, "ID", "Location", department.CollectionPoint.ID);
            ViewBag.DeptHeadID = new SelectList(db.Staffs, "ID", "UserAccountID", department.DeptHead.ID);
            ViewBag.DeptRepID = new SelectList(db.Staffs, "ID", "UserAccountID", department.DeptRep.ID);
            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
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
