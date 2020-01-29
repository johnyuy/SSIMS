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


            //for create qty
            
            
            return View(vm);
        }
    }
}