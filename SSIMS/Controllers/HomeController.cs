using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSIMS.Filters;
using System.Diagnostics;
using SSIMS.Service;
using SSIMS.Models;
using SSIMS.DAL;

namespace SSIMS.Controllers
{
    [AuthenticationFilter]
    [AuthorizationFilter]
    public class HomeController : Controller
    {
        ILoginService loginService = new LoginService();
        public ActionResult Index()
        {
            Staff user = loginService.StaffFromSession;
            Debug.WriteLine("name of staff loggin in = " + user.Name);
            Debug.WriteLine("role of staff loggin in = " + user.StaffRole);
            Debug.WriteLine("dept of staff loggin in = " + user.DepartmentID);
            Debug.WriteLine("actual authorization role = " + Session["role"].ToString());
            RetrievalList RL = new RetrievalList(loginService.StaffFromSession, null);
            return RedirectToAction("Dashboard");
        }



        public ActionResult Dashboard()
        {
            Staff staff = loginService.StaffFromSession;
            var unitofwork = new UnitOfWork();
            if(Session["role"].ToString() == "admin")
            {
                return View("Index");
            }
            if (LoginService.IsAuthorizedRoles("staff"))
            {
                List<RequisitionOrder> ros = unitofwork.RequisitionOrderRepository.Get(filter: x => x.CreatedByStaff.ID == staff.ID && (x.Status == Status.Pending || x.Status == Status.Approved)).ToList();
                return View("Staff",ros);
            }
            else if (LoginService.IsAuthorizedRoles("head"))
            {
                List<RequisitionOrder> ros = unitofwork.RequisitionOrderRepository.Get(filter: x => x.CreatedByStaff.DepartmentID == staff.DepartmentID && x.Status == Status.Pending).ToList();
                var list = (ros.OrderBy(x => x.CreatedDate)).Take(5);
                return View("DepHead",list);
            }
            else if (LoginService.IsAuthorizedRoles("rep"))
            {
                Debug.Write(staff.DepartmentID);
                //Department dep = unitofwork.DepartmentRepository.GetByID(staff.DepartmentID);
                Department dep = unitofwork.DepartmentRepository.Get(filter: x => x.ID == staff.DepartmentID, includeProperties: "CollectionPoint").First();
                Debug.Write(dep.CollectionPoint.ID);
                Debug.Write(dep.CollectionPoint.Location);
                Debug.Write(dep.CollectionPoint.Time);
                //Department dept = unitofwork.DepartmentRepository.Get(filter: x => x.DeptRep.ID == staff.ID,includeProperties:"Staff").First();

                return View("DepRep",dep);
            }
            else if (LoginService.IsAuthorizedRoles("supervisor", "manager"))
            {
                List<PurchaseOrder> pos = unitofwork.PurchaseOrderRepository.Get(x => x.Status == Status.Pending).ToList();
                ViewBag.poscount = pos.Count;
                Debug.WriteLine(pos.Count);

                List<AdjustmentVoucher> advs = unitofwork.AdjustmentVoucherRepository.Get(x => x.Status == Status.Pending).ToList();
                ViewBag.advscount = advs.Count;
                Debug.WriteLine(advs.Count);

                List<InventoryItem> items = unitofwork.InventoryItemRepository.Get(includeProperties: "Item").ToList();
                var itemslist = items.OrderBy(x => x.InStoreQty).Take(3);

                return View("Supervisor",itemslist);
            }
            else if (LoginService.IsAuthorizedRoles("clerk"))
            {
                List<InventoryItem> items = unitofwork.InventoryItemRepository.Get(includeProperties: "Item").ToList();
                var itemslist = items.OrderBy(x => x.InStoreQty).Take(3);

                return View("Clerk",itemslist);
            }
            return RedirectToAction("Authentication", "Login");
        }


    }
}