using SSIMS.DAL;
using SSIMS.Filters;
using SSIMS.Models;
using SSIMS.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSIMS.Controllers
{
    [AuthenticationFilter]
    [AuthorizationFilter]
    public class DashboardController : Controller
    {
        readonly ILoginService loginService = new LoginService();

        // GET: Dashboard
        public ActionResult Index()
        {
            if (LoginService.IsAuthorizedRoles("staff"))
                return RedirectToAction("Staff", "Dashboard");
            if (LoginService.IsAuthorizedRoles("head"))
                return RedirectToAction("DepHead", "Dashboard");
            if (LoginService.IsAuthorizedRoles("rep"))
                return RedirectToAction("DepRep", "Dashboard");

            return View();
        }

        // GET: Dashboard/Staff
        public ActionResult Staff()
        {
            Staff staff = loginService.StaffFromSession;
            

            var unitofwork = new UnitOfWork();
            List<RequisitionOrder> ros = unitofwork.RequisitionOrderRepository.Get(filter: x => x.CreatedByStaff.ID == staff.ID && (x.Status==Status.Pending ||x.Status==Status.Approved)).ToList();

            return View(ros);

        }

        // GET: Dashboard/DepHead
        public ActionResult DepHead()
        {
            Staff staff = loginService.StaffFromSession;

            var unitofwork = new UnitOfWork();
            List<RequisitionOrder> ros = unitofwork.RequisitionOrderRepository.Get(filter: x => x.CreatedByStaff.DepartmentID == staff.DepartmentID && x.Status == Status.Pending).ToList();
            var list = (ros.OrderBy(x=>x.CreatedDate)).Take(5);
            return View(list);

        }


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


    }
}