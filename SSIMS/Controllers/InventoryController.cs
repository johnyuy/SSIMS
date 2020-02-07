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
using SSIMS.Service;

namespace SSIMS.Controllers
{
    [AuthenticationFilter]
    [AuthorizationFilter]
    public class InventoryController : Controller
    {
        private UnitOfWork uow = new UnitOfWork();
        private InventoryService InventoryService = new InventoryService();

        public ActionResult Index(string searchString, string lowStock)
        {
            if (!LoginService.IsAuthorizedRoles("manager", "supervisor", "clerk"))
                return RedirectToAction("Index", "Home");


            Debug.WriteLine("searchString = " + searchString + "\tlowStock = " + lowStock);
            bool low = lowStock == "true" ? true : false;
            InventoryViewModel inventoryViewModel =  new InventoryViewModel(searchString, low);
            ViewBag.LowStock = lowStock;
            ViewBag.SearchString = searchString;
            Session["InventoryLowStockMode"] = lowStock;
            Session["InventorySearchString"] = searchString;
            Session["InventorySearchList"] = inventoryViewModel;
            return View(inventoryViewModel.inventoryItems.ToList());
        }

        public ActionResult Details(int? id, string stockcard)
        {
            if (!LoginService.IsAuthorizedRoles("manager", "supervisor", "clerk"))
                return RedirectToAction("Index", "Home");
            if (id == null)
                return RedirectToAction("Index");

            InventoryItem inventoryItem = uow.InventoryItemRepository.Get(filter:x=>x.ID==id, includeProperties:"Item").First();
            InventoryItemDetailsVM inventoryItemDetailsVM = new InventoryItemDetailsVM(inventoryItem);

            if (inventoryItem == null)
                return RedirectToAction("Index");
            
            InventoryViewModel inventoryViewModel = (InventoryViewModel)Session["InventorySearchList"];
            ViewBag.MaxItemsCount = 1;
            ViewBag.CurrentItemIndex = 1;
            ViewBag.PrevItemID = "";
            ViewBag.NextItemID = "";
            ViewBag.StockCard = stockcard == "true" ? "true" : "";
            if (inventoryViewModel != null)
            {
                ViewBag.MaxItemsCount = inventoryViewModel.inventoryItems.Count();
                ViewBag.CurrentItemIndex = InventoryService.GetItemIndexFromSearchList(inventoryItem.ID, inventoryViewModel);
                ViewBag.PrevItemID = InventoryService.GetPrevIndexFromSearchList(inventoryItem.ID, inventoryViewModel);
                ViewBag.NextItemID = InventoryService.GetNextIndexFromSearchList(inventoryItem.ID, inventoryViewModel); ;
            }
            return View(inventoryItemDetailsVM);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                uow.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
