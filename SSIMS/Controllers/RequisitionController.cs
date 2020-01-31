using SSIMS.DAL;
using SSIMS.Database;
using SSIMS.Models;
using SSIMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSIMS.Controllers
{
    public class RequisitionController : Controller
    {
        // GET: Requisition
        public ActionResult Index()
        {
            return View();
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
            vm.CreatedDate = DateTime.Today;
            vm.Status = 0;
            
            return View(vm);
        }

        // POST: Items/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SelectedCategory,SelectedDescription,Quantity")] RequisitionCreateViewModel rcvm)
        {
            var unitofwork = new UnitOfWork();
           
            if (ModelState.IsValid)
            {
               
                //get item
                Item item = unitofwork.ItemRepository.GetItembyDescrption(rcvm.SelectedDescription);
                Console.WriteLine(item.ID);

                //save this item into the doucumentitem(item, qty)
                unitofwork.DocumentItemRepository.InsertDocumentItembyItemandQty(item, rcvm.Quantity);

                //set get documenitem value
                DocumentItem doItem = new DocumentItem();
                doItem.Item = item;
                doItem.Qty = rcvm.Quantity;
               

                //save this docuItem
                unitofwork.DocumentItemRepository.Insert(doItem);


            
                //save all
                unitofwork.Save();

                //RequisitionOrder reqorder = unitofwork.RequisitionOrderRepository.Insert(documentitem);

                //save this documenitem into the reqOrder

                

                //return list after creating

                return RedirectToAction("View");
            }

            return View(rcvm);
        }

        // GET: Requisition/View
        public ActionResult View(Staff staff)
        {
           
            var unitofwork = new UnitOfWork();
            var reqList = unitofwork.DocumentItemRepository.GetRequisitionList(staff);
            return View(reqList);

          
        }
    }
}