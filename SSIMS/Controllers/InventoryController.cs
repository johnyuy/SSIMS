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
        private ILoginService loginService = new LoginService();
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
            if (!id.HasValue)
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

        public ActionResult Adjustment(int? id, string item = "", string command = "")
        {
            if (!LoginService.IsAuthorizedRoles("manager", "supervisor", "clerk"))
                return RedirectToAction("Index", "Home");

            if (command == "add" && item!="")
            {
                Debug.WriteLine("adding adjusted item to adjustment cart = " + item);
                InventoryService.addItemToAdjustmentCart(new AdjustmentItemVM(item));
                return RedirectToAction("Adjustment", new { command = "view" });


            } else if (command == "view" && item == "")
            {
                Debug.WriteLine("viewing adjustment cart");
                if (Session["AdjustmentCart"] == null)
                {
                    ViewBag.Empty = "true";
                    return View("AdjustmentVoucherNew");
                }
                else
                {
                    ViewBag.ErrorMsg = TempData["ErrorMsg"];
                    AdjustmentVoucherVM adjustmentCart = (AdjustmentVoucherVM)Session["AdjustmentCart"];
                    return View("AdjustmentVoucherNew", adjustmentCart);
                }
            } else if (id.HasValue)
            {
                AdjustmentVoucherVM VM = InventoryService.GetAdjustmentVoucherVMSingle(id.Value);
                if (VM != null)
                {
                    if (LoginService.IsAuthorizedRoles("manager", "supervisor"))
                        ViewBag.IsSuper = true;
                    return View("AdjustmentVoucher", VM);
                }
                    
            }

            //redirect to AV list
            return View("AdjustmentList", InventoryService.GetAdjustmentVoucherVMList());
        }


        [HttpPost]
        public ActionResult SaveAdjustments([Bind(Include = "AdjustmentID,ReportedByStaffName,ReportedByStaffID,Status,AdjustmentItems")]AdjustmentVoucherVM AdjustmentVM, string change)
        {
            Debug.WriteLine("Saving adjustments cart via ajax...");
            Debug.WriteLine(Server.UrlDecode(change));
            for (int i = 0; i < change.Split('&').Count(); i++)
            {
                string changestr = change.Split('&')[i];
                if (i % 2 == 0)
                {
                    string qty = Server.UrlDecode(changestr.Split('=')[1]);
                    if (int.TryParse(qty, out int output)){
                        AdjustmentVM.AdjustmentItems[i / 2].QtyAdjusted = output;
                        Debug.WriteLine(AdjustmentVM.AdjustmentItems[i / 2].QtyAdjusted);
                    }
                }
                else
                {
                    string remarks = Server.UrlDecode(changestr.Split('=')[1]);
                    AdjustmentVM.AdjustmentItems[i / 2].Remarks = remarks;
                    Debug.WriteLine(AdjustmentVM.AdjustmentItems[i / 2].Remarks);
                }
            }
            Session["AdjustmentCart"] = AdjustmentVM;
            return Content("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitAdjustments()
        {
            if (!LoginService.IsAuthorizedRoles("manager", "supervisor", "clerk"))
                return RedirectToAction("Index", "Home");

            if (Session["AdjustmentCart"] == null)
                return RedirectToAction("Adjustment", new { command = "view" });

            if (InventoryService.VerifyAdjustmentCart())
            {
               if(InventoryService.ProcessStockAdjustmentEntry(loginService.StaffFromSession))
                {
                    Session["AdjustmentCart"]= null;
                    return RedirectToAction("Adjustment");
                }
                TempData["ErrorMsg"] = "Error: An error occured during submission, please inform system admin";
            } else
            {
                TempData["ErrorMsg"] = "Error: Please ensure that all quantities are not zero";
            }
            return RedirectToAction("Adjustment", new { command = "view" });

        }

        public ActionResult RemoveAdjustment(int? index)
        {
            AdjustmentVoucherVM adjustmentCart = (AdjustmentVoucherVM)Session["AdjustmentCart"];
            if(adjustmentCart != null && index.HasValue)
            {
                Debug.WriteLine("Removing item " + index.Value + "from adjustment cart");
                adjustmentCart.AdjustmentItems.RemoveAt(index.Value);
                if (adjustmentCart.AdjustmentItems.Count == 0)
                    Session.Remove("AdjustmentCart");
            }
            return RedirectToAction("Adjustment", new { command = "view" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReplyAdjustment(int? id, bool response)
        {
            if (!LoginService.IsAuthorizedRoles("manager", "supervisor"))
                return RedirectToAction("Index", "Home");

            //update
            InventoryService.UpdateAdjustmentVoucherStatus(id.Value, response, loginService.StaffFromSession);
            return RedirectToAction("Adjustment");
        }

        public ActionResult InventoryStockCheck()
        {
            if (!LoginService.IsAuthorizedRoles("manager", "supervisor","clerk"))
                return RedirectToAction("Index", "Home");

            //session's adjustment cart must be empty first
            if (Session["AdjustmentCart"] != null)
            {
                TempData["ErrorMsg"] = "Please submit or clear you new adjustments before starting inventory stock check!";
                return RedirectToAction("Adjustment", new { command = "view" });
            }

            //contruct list of inventorycheckVM for view's model
            return View(InventoryService.GenerateInventoryCheckList());
        }


        [HttpPost]
        public ActionResult UpdateStockCheck(string itemcode, string qtyChanged, string remarks)
        {
            string itemCode = itemcode.Trim();
            string qtyx = qtyChanged.Trim();
            string re = remarks.Trim();

            Debug.WriteLine("Updating stock check: " + itemcode.Trim() + " " + qtyChanged.Trim() + " " + remarks.Trim());
            
            //LOGIC : IF QTY is the same as the one in the inventory, we do not save it
            // Else we store the discrepancy in the Session object "Adjustment Cart" as a
            // AdjustmentItemVm (DONE)
            if(int.TryParse(qtyx, out int x)) //check for if qtyChanged is a integer first
            {
                if (x != 0)
                {
                    Debug.WriteLine("\tCreating a adjustment");
                    AdjustmentItemVM adjustment = new AdjustmentItemVM(itemCode, x, re);
                    if (adjustment != null)
                        InventoryService.addItemToAdjustmentCart(adjustment);
                }
                else
                {
                    Debug.WriteLine("No changed qty detected");
                }
            }
            //no need to return anything since we are just receiving information
            return Content("");
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
