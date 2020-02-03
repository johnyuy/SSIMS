﻿using SSIMS.DAL;
using SSIMS.Service;
using SSIMS.Models;
using SSIMS.ViewModels;
using SSIMS.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SSIMS.Controllers
{
    [AuthenticationFilter]
    [AuthorizationFilter]
    public class RequisitionController : Controller
    {
        //DatabaseContext db = new DatabaseContext();

        readonly IRequisitionService RequisitionService = new RequisitionService();
        readonly IStaffService StaffService = new StaffService();
        //for json implementation of dynamic html
        public JsonResult GetDocumentItems()
        {
            
            var qry = new List<RequisitionItemVM>();//need to according to staff
            return Json(qry, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Add([ModelBinder(typeof(RequisitionCreateBinder))] List<RequisitionItemVM> requisitionItems)
        {
            
            if (ModelState.IsValid)
            {
                //call service to add 
                Staff user = StaffService.GetStaffByUsername(Session["username"].ToString());
                RequisitionService.CreateNewRequistionOrder(requisitionItems, user);
                TempData["message"] = "Record saved successfully";
            }
            return View();
        }

        public class RequisitionCreateBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {  //throw new NotImplementedException();
                HttpContextBase objContext = controllerContext.HttpContext;
                //String category = objContext.Request.Form["SelectedCategory"];
                //String description= objContext.Request.Form["SelectedDescription"];//return item ID
                int quantity = Convert.ToInt32(objContext.Request.Form["Quantity"]);

                DocumentItem objdoitem = new DocumentItem
                {

                    Qty = quantity
                    
                };

                return objdoitem;
            }

        }








        // GET: Requisition
        public ActionResult Index(Staff staff)
        {
            staff.ID = 10006;
            var unitofwork = new UnitOfWork();

            staff = unitofwork.StaffRepository.GetStaffbyID(staff.ID);

            var reqList = unitofwork.RequisitionOrderRepository.Get(filter: x => x.CreatedByStaffID == staff.ID);
            return View(reqList);


        }

        // GET: Requisition/ViewHistory
        public ActionResult ViewHistory()
        {

            var unitofwork = new UnitOfWork();

            var reqList = unitofwork.RequisitionOrderRepository.Get(filter: x => x.Status == Models.Status.Approved|| x.Status == Models.Status.Rejected);

            return View(reqList);

        }

        //GET:Requisition/Deal
        public ActionResult Deal()
        {
            var unitofwork = new UnitOfWork();
            var reqList = unitofwork.RequisitionOrderRepository.Get(filter: x => x.Status == Models.Status.Pending);
            return View(reqList);
        }

        [HttpGet]
        public ActionResult GetDescription(string category)
        {
            var unitofwork = new UnitOfWork();
            var irepo = unitofwork.ItemRepository;

            IEnumerable<SelectListItem> descriptionlist = (IEnumerable<SelectListItem>)irepo.GetDescriptions(category);
            return Json(descriptionlist, JsonRequestBehavior.AllowGet);

        }

        // GET: Requisition/Create
        public ActionResult Create2()
        {
            //for create dynamic dropdownlist
            var unitofwork = new UnitOfWork();
            var irepo = unitofwork.ItemRepository;
            RequisitionItemVM vm = new RequisitionItemVM();
            vm.Categories = irepo.GetCategories();
            vm.Descriptions = irepo.GetDescriptions();

            return View(vm);
        }
        public ActionResult Create()
        {
            //for create dynamic dropdownlist
            RequisitionItemVM vm = new RequisitionItemVM();
            List<RequisitionItemVM> vmList = new List<RequisitionItemVM>();
            vmList.Add(vm);
            return View(vm);
        }

        ////POST: Requisition/Add
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Add([Bind(Include = "SelectedCategory,SelectedDescription,Quantity")] RequisitionCreateViewModel rcvm)
        //{
        //    var unitofwork = new UnitOfWork();
        //    Debug.WriteLine(rcvm.SelectedCategory + "\n" + rcvm.SelectedDescription + " x " + rcvm.Quantity.ToString());
        //    Debug.WriteLine(rcvm.CreatedDate);
        //    Debug.WriteLine(rcvm.Status);
        //    // Debug.WriteLine(rcvm.RequisitionOrderID);
        //    if (ModelState.IsValid)
        //    {

        //        //get item by itemID
        //        Item item = unitofwork.ItemRepository.GetItembyDescrption(rcvm.SelectedDescription);
        //        Debug.WriteLine(item.ID);

        //        //save this item into the doucumentitem(item, qty)
        //        DocumentItem doitem = unitofwork.DocumentItemRepository.InsertDocumentItembyItemandQty(item, rcvm.Quantity);
        //        unitofwork.Save();
        //        Debug.WriteLine(doitem.ID);

        //        //RequisitionOrder rq = new RequisitionOrder(unitofwork.StaffRepository.GetByID(10006));
        //        //rq.DocumentItems.Add(unitofwork.DocumentItemRepository.GetByID(doitem.ID));

        //        //unitofwork.RequisitionOrderRepository.Insert(rq);

        //        //unitofwork.Save();

        //        return View(doitem);
        //    }

        //    return View(rcvm);
        //}


        // POST: Requisition/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SelectedCategory,SelectedDescription,Quantity")] RequisitionItemVM rcvm)
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
                DocumentItem doitem = unitofwork.DocumentItemRepository.InsertDocumentItembyItemandQty(item, rcvm.Quantity);
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

            var requisitionOrder = unitofwork.RequisitionOrderRepository.Get(filter: x => x.ID == id, includeProperties: "DocumentItems.Item").FirstOrDefault();


            if (requisitionOrder == null)
            {
                return HttpNotFound();
            }
            return View(requisitionOrder);

        }

        // GET: Requisition/Edit/4
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Debug.WriteLine("Edit get");

            // //delete requisitionOrder by ID
            // var unitofwork = new UnitOfWork();
            // RequisitionOrder ro = unitofwork.RequisitionOrderRepository.GetByID(id);
            // var doitems = ro.DocumentItems.ToList();
            // unitofwork.DocumentItemRepository.Delete(doitems);
            // Debug.WriteLine(ro.ID);
            //  //unitofwork.RequisitionOrderRepository.DeleteRObyID(id);

            // //unitofwork.RequisitionOrderRepository.Delete(ro);



            // unitofwork.Save();
            // if (ro.ID == null)
            // {
            //     Debug.WriteLine(" delete");
            // }
            // else
            // {
            //     Debug.WriteLine("not delete :"+ro.ID);
            // }

            var unitofwork = new UnitOfWork();
            var irepo = unitofwork.ItemRepository;
            RequisitionItemVM vm = new RequisitionItemVM();
            vm.Categories = irepo.GetCategories();
            vm.Descriptions = irepo.GetDescriptions();

            vm.ROID = id;



            return View(vm);
        }

        // POST: Requisition/Edit/4
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SelectedCategory,SelectedDescription,Quantity,ROID")] RequisitionItemVM rcvm)
        {
            var unitofwork = new UnitOfWork();
            Debug.WriteLine(rcvm.SelectedCategory + "\n" + rcvm.SelectedDescription + " x " + rcvm.Quantity.ToString());

            Debug.WriteLine(rcvm.ROID);
            // Debug.WriteLine(rcvm.RequisitionOrderID);
            if (ModelState.IsValid)
            {

                //get item by itemID
                Item item = unitofwork.ItemRepository.GetItembyDescrption(rcvm.SelectedDescription);
                Debug.WriteLine(item.ID);

                //save this item into the doucumentitem(item, qty)
                DocumentItem doitem = unitofwork.DocumentItemRepository.InsertDocumentItembyItemandQty(item, rcvm.Quantity);

                //doitem.Document.ID = rcvm.ROID;
                unitofwork.Save();
                Debug.WriteLine(doitem.ID);

                RequisitionOrder rq = new RequisitionOrder(unitofwork.StaffRepository.GetByID(10006));
                rq.DocumentItems.Add(unitofwork.DocumentItemRepository.GetByID(doitem.ID));
                rq.ID = rcvm.ROID;
                Debug.WriteLine(rq.ID);


                unitofwork.RequisitionOrderRepository.Insert(rq);
                Debug.WriteLine(rq.ID);

                unitofwork.Save();
                Debug.WriteLine(rq.ID);

                return RedirectToAction("Index");
            }

            return View(rcvm);
        }

        //GET: Requisition/Cancel/4
        public ActionResult Cancel(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var unitofwork = new UnitOfWork();

            var requisitionOrder = unitofwork.RequisitionOrderRepository.Get(filter: x => x.ID == id, includeProperties: "DocumentItems.Item").FirstOrDefault();
            requisitionOrder.Status = Models.Status.Cancelled;
            unitofwork.Save();

            if (requisitionOrder == null)
            {
                return HttpNotFound();
            }
            return View(requisitionOrder);

        }

        //GET: Requisition/Approve/4
        public ActionResult Approve(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var unitofwork = new UnitOfWork();

            var requisitionOrder = unitofwork.RequisitionOrderRepository.Get(filter: x => x.ID == id, includeProperties: "DocumentItems.Item").FirstOrDefault();
            requisitionOrder.Status = Models.Status.Approved;
            requisitionOrder.ResponseDate = DateTime.Now;
            unitofwork.Save();

            if (requisitionOrder == null)
            {
                return HttpNotFound();
            }
            return View(requisitionOrder);
        }


        //GET: Requisition/Reject/4
        public ActionResult Reject(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var unitofwork = new UnitOfWork();

            var requisitionOrder = unitofwork.RequisitionOrderRepository.Get(filter: x => x.ID == id, includeProperties: "DocumentItems.Item").FirstOrDefault();
            requisitionOrder.Status = Models.Status.Rejected;
            requisitionOrder.ResponseDate = DateTime.Now;
            unitofwork.Save();

            if (requisitionOrder == null)
            {
                return HttpNotFound();
            }
            return View(requisitionOrder);
        }

    }

    

}
