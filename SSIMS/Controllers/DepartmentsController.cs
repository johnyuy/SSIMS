using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SSIMS.Database;
using SSIMS.DAL;
using SSIMS.Models;
using System.Diagnostics;

namespace SSIMS.Controllers
{
    public class DepartmentsController : Controller
    {

        // GET: Departments
        public ActionResult Index()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            var departments = unitOfWork.DepartmentRepository.Get(includeProperties: "CollectionPoint");
            ViewBag.RepList = unitOfWork.StaffRepository.GetDeptRepList();
            Debug.WriteLine("number of heads: " + unitOfWork.StaffRepository.GetDeptHeadList().Count());
            var Deps = unitOfWork.DepartmentRepository.Get(filter: x => x.ID != "STOR", includeProperties: "DeptHeadAuthorization.Staff");
            ViewBag.Departments = Deps;
            List<string> authNames = new List<string>();
            foreach (Department d in Deps)
            {
                if (d.DeptHeadAuthorization == null)
                    authNames.Add("none");
                else
                    authNames.Add(d.DeptHeadAuthorization.Staff.Name);
            }
            ViewBag.HeadList = unitOfWork.StaffRepository.Get(filter: x => x.StaffRole == "DeptHead");
            ViewBag.RepList = unitOfWork.StaffRepository.Get(filter: x => x.StaffRole == "DeptRep");
            ViewBag.Auth = authNames;
            ViewBag.DeptCount = unitOfWork.DepartmentRepository.Get().Count();
            return View(departments.ToList());
        }

        // GET: Departments/Details/5
        public ActionResult Details(string id)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            Department department = unitOfWork.DepartmentRepository.Get(filter: x => x.ID == id, includeProperties: "CollectionPoint").First();
            CollectionPoint selected = department.CollectionPoint;
            ViewBag.SelectedPoint = selected.Location;
            ViewBag.OtherPoints = unitOfWork.CollectionPointRepository.Get(filter: x => x.Location != selected.Location);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.Department = department;

            return View();
        }

        // GET: Departments/Create 
        public ActionResult Create()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            ViewBag.CollectionPointID = new SelectList(unitOfWork.CollectionPointRepository.Get(), "ID", "Location");
            ViewBag.DeptHeadID = new SelectList(unitOfWork.StaffRepository.Get(), "ID", "UserAccountID");
            ViewBag.DeptRepID = new SelectList(unitOfWork.StaffRepository.Get(), "ID", "UserAccountID");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DeptRepID,DeptHeadID,CollectionPointID,DeptHeadAutorizationID,DeptName,PhoneNumber,FaxNumber")] Department department)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            if (ModelState.IsValid)
            {
                unitOfWork.DepartmentRepository.Update(department);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            ViewBag.CollectionPointID = new SelectList(unitOfWork.CollectionPointRepository.Get(), "ID", "Location", department.CollectionPoint.ID);
            ViewBag.DeptHeadID = new SelectList(unitOfWork.StaffRepository.Get(), "ID", "UserAccountID", department.DeptHead.ID);
            ViewBag.DeptRepID = new SelectList(unitOfWork.StaffRepository.Get(), "ID", "UserAccountID", department.DeptRep.ID);
            return View(department);
        }

        // GET: Departments/Edit/ARCH
        public ActionResult Edit(string id)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = unitOfWork.DepartmentRepository.GetByID(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.CollectionPointID = new SelectList(unitOfWork.CollectionPointRepository.Get(), "ID", "Location", department.CollectionPoint.ID);
            ViewBag.DeptHeadID = new SelectList(unitOfWork.StaffRepository.Get(), "ID", "UserAccountID", department.DeptHead.ID);
            ViewBag.DeptRepID = new SelectList(unitOfWork.StaffRepository.Get(), "ID", "UserAccountID", department.DeptRep.ID);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DeptRepID,DeptHeadID,CollectionPointID,DeptHeadAutorizationID,DeptName,PhoneNumber,FaxNumber")] Department department)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            if (ModelState.IsValid)
            {
                unitOfWork.DepartmentRepository.Update(department);
                unitOfWork.Save();
                return RedirectToAction("Index");

            }
            ViewBag.CollectionPointID = new SelectList(unitOfWork.CollectionPointRepository.Get(), "ID", "Location", department.CollectionPoint.ID);
            ViewBag.DeptHeadID = new SelectList(unitOfWork.StaffRepository.Get(), "ID", "UserAccountID", department.DeptHead.ID);
            ViewBag.DeptRepID = new SelectList(unitOfWork.StaffRepository.Get(), "ID", "UserAccountID", department.DeptRep.ID);

            //ViewBag.CollectionPointID = new SelectList(unitOfWork.CollectionPointRepository.Get(), "ID", "Location", department.CollectionPoint.ID);
            //ViewBag.DeptHeadID = new SelectList(unitOfWork.StaffRepository.Get(), "UserAccountID", department.DeptHead.ID);
            //ViewBag.DeptRepID = new SelectList(unitOfWork.StaffRepository.Get(), "UserAccountID", department.DeptRep.ID);
            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(string id)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = unitOfWork.DepartmentRepository.GetByID(id);
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
            UnitOfWork unitOfWork = new UnitOfWork();
            Department department = unitOfWork.DepartmentRepository.GetByID(id);
            unitOfWork.DepartmentRepository.Delete(department);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult SelectCollectionPoint (string id)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = unitOfWork.DepartmentRepository.GetByID(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.CollectionPointID = new SelectList(unitOfWork.CollectionPointRepository.Get(), "ID", "Location", department.CollectionPoint.ID);
            return View(department);
        }
     
    }
}

