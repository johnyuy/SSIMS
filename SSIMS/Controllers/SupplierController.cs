using SSIMS.DAL;
using SSIMS.Filters;
using SSIMS.Models;
using SSIMS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SSIMS.Controllers
{
    [AuthenticationFilter]
    [AuthorizationFilter]
    public class SupplierController : Controller
    {
        readonly ILoginService loginService = new LoginService();
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Supplier
        public ActionResult Index()
        {
            Staff staff = loginService.StaffFromSession;
            ViewBag.staffrole = staff.StaffRole;

            var suppliers = unitOfWork.SupplierRepository.Get();

            return View(suppliers);
        }

        // GET: Supplier/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Supplier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SupplierName,Address,PhoneNumber,FaxNumber,GstReg,ContactName")] Supplier supplier)
        {

            if (ModelState.IsValid)
            {
                unitOfWork.SupplierRepository.Insert(supplier);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        // GET: Supplier/Edit/ID
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Supplier supplier = unitOfWork.SupplierRepository.GetByID(id);

         
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Items/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SupplierName,Address,PhoneNumber,FaxNumber,GstReg,ContactName")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.SupplierRepository.Update(supplier);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(supplier);
        }


        // GET: Supplier/Delete/id
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = unitOfWork.SupplierRepository.GetByID(id);
            
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Supplier/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSupplier(string id)
        {
            Supplier supplier = unitOfWork.SupplierRepository.GetByID(id);
            unitOfWork.SupplierRepository.Delete(supplier);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        // GET: Supplier/Details/id
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Staff staff = loginService.StaffFromSession;
            ViewBag.staffrole = staff.StaffRole;

            Supplier supplier = unitOfWork.SupplierRepository.GetByID(id);
           
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

    } 
}