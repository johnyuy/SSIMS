using SSIMS.DAL;
using SSIMS.Database;
using SSIMS.Models;
using SSIMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SSIMS.Controllers
{
    public class RequisitionController : Controller
    {
        // GET: Requisition
        public ActionResult Index(Staff staff)
        {
            staff.ID = 10006;
            var unitofwork = new UnitOfWork();

            staff = unitofwork.StaffRepository.GetStaffbyID(staff.ID);
           
            var reqList = unitofwork.RequisitionOrderRepository.Get(filter:x=>x.CreatedByStaffID==staff.ID);
            return View(reqList);

           
        }

        // GET: Requisition/ViewAllList
        public ActionResult ViewAllList()
        {
            
            var unitofwork = new UnitOfWork();

            var reqList = unitofwork.RequisitionOrderRepository.Get();

            return View(reqList);

        }


        [HttpGet]
        public ActionResult GetDescription(string category)
        {
            var unitofwork = new UnitOfWork();
            var irepo = unitofwork.ItemRepository;

                IEnumerable<SelectListItem> descriptionlist = (IEnumerable<SelectListItem>)irepo.GetDescription(category);
                return Json(descriptionlist, JsonRequestBehavior.AllowGet);
            
        }

        // GET: Requisition/Create
        public ActionResult Create()
        {
            //for create dynamic dropdownlist
            var unitofwork = new UnitOfWork();
            var irepo = unitofwork.ItemRepository;
            RequisitionCreateViewModel vm = new RequisitionCreateViewModel();
            vm.Categories = irepo.GetCategories();
            vm.Descriptions = irepo.GetDescription();
            //vm.CreatedDate = DateTime.Today;
            vm.Status = 0;
          
            
            return View(vm);
        }

        // POST: Requisition/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SelectedCategory,SelectedDescription,Quantity")] RequisitionCreateViewModel rcvm)
        {
            var unitofwork = new UnitOfWork();
            Debug.WriteLine(rcvm.SelectedCategory + "\n" + rcvm.SelectedDescription + " x " + rcvm.Quantity.ToString());
            Debug.WriteLine(rcvm.CreatedDate);
            Debug.WriteLine(rcvm.Status);
           // Debug.WriteLine(rcvm.RequisitionOrderID);
            if (ModelState.IsValid)
            {
               
                //get item by itemID
                Item item = unitofwork.ItemRepository.GetItembyDescrption(rcvm.SelectedDescription);
                Debug.WriteLine(item.ID);

                //save this item into the doucumentitem(item, qty)
                DocumentItem doitem= unitofwork.DocumentItemRepository.InsertDocumentItembyItemandQty(item, rcvm.Quantity);
                unitofwork.Save();
                Debug.WriteLine(doitem.ID);

               RequisitionOrder rq = new RequisitionOrder(unitofwork.StaffRepository.GetByID(10006));
               rq.DocumentItems.Add(unitofwork.DocumentItemRepository.GetByID(doitem.ID));

               unitofwork.RequisitionOrderRepository.Insert(rq);
               
               unitofwork.Save();

               return RedirectToAction("Index");
            }

            return View(rcvm);
        }


        // GET: Requisition/Details/4
        public ActionResult Details(int id)
        {
            //return documentitem by input requisitionOrder ID
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var unitofwork = new UnitOfWork();

            var requisitionOrder = unitofwork.RequisitionOrderRepository.Get(filter: x => x.ID == id, includeProperties:"DocumentItems.Item").FirstOrDefault();

           
            if (requisitionOrder == null)
            {
                return HttpNotFound();
            }
            return View(requisitionOrder);

        }

    }
}