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
        [AuthorizationFilter]
        public ActionResult Index()
        {
            Staff user = loginService.StaffFromSession;
            Debug.WriteLine("name of staff loggin in = " + user.Name);
            Debug.WriteLine("role of staff loggin in = " + user.StaffRole);
            string dept = user.Department == null ? "no dept" : user.Department.DeptName;
            Debug.WriteLine("dept of staff loggin in = " + dept);
            Debug.WriteLine("actual authorization role = " + Session["role"].ToString());

            RetrievalList RL = new RetrievalList(loginService.StaffFromSession, null);


            if (LoginService.IsAuthorizedRoles("staff"))
                return RedirectToAction("Staff", "Home");
            if (LoginService.IsAuthorizedRoles("head"))
                return RedirectToAction("DepHead", "Home");
            if (LoginService.IsAuthorizedRoles("rep"))
                return RedirectToAction("DepRep", "Home");
            if (LoginService.IsAuthorizedRoles("supervisor"))
                return RedirectToAction("Supervisor", "Home");
            if (LoginService.IsAuthorizedRoles("clerk"))
                return RedirectToAction("Clerk", "Home");


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // GET: Home/Staff
        public ActionResult Staff()
        {
            Staff staff = loginService.StaffFromSession;


            var unitofwork = new UnitOfWork();
            List<RequisitionOrder> ros = unitofwork.RequisitionOrderRepository.Get(filter: x => x.CreatedByStaff.ID == staff.ID && (x.Status == Status.Pending || x.Status == Status.Approved)).ToList();

            return View(ros);

        }

        // GET: Home/DepHead
        public ActionResult DepHead()
        {
            Staff staff = loginService.StaffFromSession;

            var unitofwork = new UnitOfWork();
            List<RequisitionOrder> ros = unitofwork.RequisitionOrderRepository.Get(filter: x => x.CreatedByStaff.DepartmentID == staff.DepartmentID && x.Status == Status.Pending).ToList();
            var list = (ros.OrderBy(x => x.CreatedDate)).Take(5);
            return View(list);

        }

        // GET: Home/DepRep
        public ActionResult DepRep()
        {
            var unitofwork = new UnitOfWork();

            Staff staff = loginService.StaffFromSession;
            Debug.Write(staff.DepartmentID);
            //Department dep = unitofwork.DepartmentRepository.GetByID(staff.DepartmentID);

            Department dep = unitofwork.DepartmentRepository.Get(filter: x => x.ID == staff.DepartmentID, includeProperties: "CollectionPoint").First();

            Debug.Write(dep.CollectionPoint.ID);
            Debug.Write(dep.CollectionPoint.Location);
            Debug.Write(dep.CollectionPoint.Time);


            //Department dept = unitofwork.DepartmentRepository.Get(filter: x => x.DeptRep.ID == staff.ID,includeProperties:"Staff").First();

            return View(dep);
        }


        public ActionResult Supervisor()
        {
            
            var unitofwork = new UnitOfWork();
            List<PurchaseOrder> pos = unitofwork.PurchaseOrderRepository.Get(x => x.Status == Status.Pending).ToList();
            ViewBag.poscount = pos.Count;
            Debug.WriteLine(pos.Count);

            List<AdjustmentVoucher> advs = unitofwork.AdjustmentVoucherRepository.Get(x => x.Status == Status.Pending).ToList();
            ViewBag.advscount = advs.Count;
            Debug.WriteLine(advs.Count);

            List<InventoryItem> items = unitofwork.InventoryItemRepository.Get(includeProperties:"Item").ToList();
            var itemslist = items.OrderBy(x => x.InStoreQty).Take(3);

            return View(itemslist);

        }

        public ActionResult Clerk()
        {
            var unitofwork = new UnitOfWork();

            List<InventoryItem> items = unitofwork.InventoryItemRepository.Get(includeProperties: "Item").ToList();
            var itemslist = items.OrderBy(x => x.InStoreQty).Take(3);

            return View(itemslist);
        }

    }
}