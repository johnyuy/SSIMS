using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Diagnostics;
using System.Web.Mvc;
using SSIMS.Database;
using SSIMS.Models;
using SSIMS.DAL;
using SSIMS.Service;
using SSIMS.ViewModels;
using PagedList;
using SSIMS.Filters;

namespace SSIMS.Controllers
{

    [AuthenticationFilter]
    [AuthorizationFilter]
    public class PurchaseItemsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private UnitOfWork uow = new UnitOfWork();
        private ILoginService loginService = new LoginService();

        // GET: PurchaseItems
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (!LoginService.IsAuthorizedRoles( "clerk"))
                return RedirectToAction("Index", "Home");

            ViewBag.ItemIDSortParm = String.IsNullOrEmpty(sortOrder) ? "Item_ID" : "";
            ViewBag.ItemDescSortParm = sortOrder == "Desc" ? "Item_desc" : "Desc";
            //ViewBag.ItemSupplierSortParm = sortOrder == "Supplier" ? "Item_Supplier" : "Supplier";
            var purchaseItems = uow.PurchaseItemRepository.Get(filter: x => x.PurchaseOrder == null, includeProperties: "Tender.Item");

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                purchaseItems = purchaseItems.Where(i => i.Tender.Item.Description.ToUpper().Contains(searchString.ToUpper())
                                       || i.Tender.Item.ID.ToUpper().Contains(searchString.ToUpper())
                                       || i.Tender.Supplier.ID.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "Item_ID":
                    purchaseItems = purchaseItems.OrderByDescending(i => i.Tender.Item.ID);
                    break;
                case "Item_Desc":
                    purchaseItems = purchaseItems.OrderBy(i => i.Tender.Item.Description);
                    break;
                case "Desc":
                    purchaseItems = purchaseItems.OrderByDescending(i => i.Tender.Item.Description);
                    break;
                default:
                    purchaseItems = purchaseItems.OrderBy(i => i.Tender.Item.ID);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            List<PurchaseItemVM> mv = new List<PurchaseItemVM>();

            foreach(PurchaseItem pi in purchaseItems)
            {
                mv.Add(new PurchaseItemVM(pi, uow));

            }

            return View(mv.ToPagedList(pageNumber, pageSize));
        }

        // GET: PurchaseItems/Details/5
        public ActionResult Details(int? id)
        {
            if (!LoginService.IsAuthorizedRoles("clerk"))
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseItem purchaseItem = db.PurchaseItems.Find(id);
            if (purchaseItem == null)
            {
                return HttpNotFound();
            }
            return View(purchaseItem);
        }

        // GET: PurchaseItems/GeneratePurchaseOrders
        public ActionResult GeneratePurchaseOrders()
        {
            if (!LoginService.IsAuthorizedRoles("clerk"))
                return RedirectToAction("Index", "Home");

            var suppliers = uow.SupplierRepository.GetWithRawSql("Select DISTINCT Suppliers.ID, Suppliers.SupplierName, " +
                "Suppliers.Address, Suppliers.PhoneNumber, Suppliers.FaxNumber, Suppliers.GstReg, Suppliers.ContactName " +
                "From Suppliers, Tenders, PurchaseItems where Suppliers.ID = Tenders.Supplier_ID AND PurchaseItems.Tender_ID = Tenders.ID AND PurchaseItems.PurchaseOrder_ID IS NULL");

            // use session data later 
            Staff clerk = loginService.StaffFromSession;

            Debug.WriteLine("GeneratePurchaseOrders");

            foreach (Supplier s in suppliers)
            {
                PurchaseOrder PO = new PurchaseOrder(clerk, s);
                List<PurchaseItem> purchaseItems = uow.PurchaseItemRepository.Get(filter: x => x.PurchaseOrder == null && x.Tender.Supplier.ID == s.ID, includeProperties: "Tender.Item").ToList();

                PO.PurchaseItems = purchaseItems;
                uow.PurchaseOrderRepository.Insert(PO);
                uow.Save();
                Debug.WriteLine("Saved");

            }
            return RedirectToAction("Index", "PurchaseOrders");
        }


        public ActionResult AddPurchaseItem(int id)
        {
            if (!LoginService.IsAuthorizedRoles("clerk"))
                return RedirectToAction("Index", "Home");

            Tender tender = uow.TenderRepository.Get(filter: x => x.ID == id, includeProperties: "Item, Supplier").FirstOrDefault();
            InventoryItem inventoryItem = uow.InventoryItemRepository.Get(filter: x => x.Item.ID == tender.Item.ID).FirstOrDefault();
            PurchaseItem purchaseItem = new PurchaseItem();
            purchaseItem  = uow.PurchaseItemRepository.Get(filter: x => x.PurchaseOrder == null & x.Tender.ID == id, includeProperties: "Tender.Item").FirstOrDefault();

            if (purchaseItem == null)
            {
                PurchaseItem PI = new PurchaseItem(tender.Item, tender.Supplier, inventoryItem.ReorderQty, uow);
                uow.PurchaseItemRepository.Insert(PI);
                uow.Save();
            } else
            {
                purchaseItem.Qty = purchaseItem.Qty + inventoryItem.ReorderQty;
                uow.PurchaseItemRepository.Update(purchaseItem);
                uow.Save();
            }
            return RedirectToAction("Index");
        }


        // GET: PurchaseItems/AddAllLowStockToPurchaseItems
        public ActionResult AddAllLowStockToPurchaseItems()
        {
            if (!LoginService.IsAuthorizedRoles("clerk"))
                return RedirectToAction("Index", "Home");

            var lowstockitems = uow.InventoryItemRepository.Get(filter: i => i.InStoreQty < i.ReorderLvl, includeProperties: "Item");
            // need to change later
            Supplier s = uow.SupplierRepository.GetByID("ALPA");

            var purchaseItems = uow.PurchaseItemRepository.Get(filter: x => x.PurchaseOrder == null, includeProperties: "Tender.Item");


            foreach (InventoryItem ii in lowstockitems)
            {
                int flag = 0;
                //check if existing item is already in purchaseItems 
                foreach (PurchaseItem pi in purchaseItems)
                {
                    if (ii.Item == pi.Tender.Item)
                    {
                        pi.Qty = pi.Qty + ii.ReorderQty;
                        uow.PurchaseItemRepository.Update(pi);
                        uow.Save();
                        flag = 1;
                    }
                }

                if(flag == 0)
                {
                    PurchaseItem PI = new PurchaseItem(ii.Item, s, ii.ReorderQty, uow);
                    uow.PurchaseItemRepository.Insert(PI);
                    uow.Save();
                    Debug.WriteLine("Created Purchase Item");
                }
            }

            return RedirectToAction("Index");
        }

        // GET: PurchaseItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!LoginService.IsAuthorizedRoles("clerk"))
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseItem purchaseItem = uow.PurchaseItemRepository.Get(filter: x => x.ID == id, includeProperties: "Tender.Item").FirstOrDefault();
            if (purchaseItem == null)
            {
                return HttpNotFound();
            }
            return View(purchaseItem);
        }

        // POST: PurchaseItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!LoginService.IsAuthorizedRoles("clerk"))
                return RedirectToAction("Index", "Home");

            PurchaseItem purchaseItem = db.PurchaseItems.Find(id);
            db.PurchaseItems.Remove(purchaseItem);
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
