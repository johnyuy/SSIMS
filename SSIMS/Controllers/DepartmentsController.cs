using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SSIMS.Database;
using SSIMS.DAL;
using SSIMS.Models;
using System.Diagnostics;
using SSIMS.Filters;
using SSIMS.Service;
using SSIMS.ViewModels;


namespace SSIMS.Controllers
{
    [AuthenticationFilter]
    [AuthorizationFilter]
    public class DepartmentsController : Controller
    {
        
        UnitOfWork unitOfWork = new UnitOfWork();
        ILoginService loginService = new LoginService();
        DepartmentService departmentService = new DepartmentService();
        IStaffService staffService = new StaffService();
        // GET: Departments
        public ActionResult Index()
        {
            Debug.WriteLine("Hey drake , user type = " + Session["usertype"]);
            Debug.WriteLine("Hey drake , user group = " + Session["usergroup"]);
            var departments = unitOfWork.DepartmentRepository.Get(includeProperties: "CollectionPoint");
            ViewBag.RepList = unitOfWork.StaffRepository.GetDeptRepList();
            Debug.WriteLine("number of heads: " + unitOfWork.StaffRepository.GetDeptHeadList().Count());
            var Deps = unitOfWork.DepartmentRepository.Get(filter: x => x.ID != "STOR", includeProperties: "DeptHeadAuthorization.Staff");
            ViewBag.Departments = Deps;
            List<string> authNames = new List<string>();
            foreach (Department d in Deps)
            {
                if (d.DeptHeadAuthorization == null)
                    authNames.Add("none");
                else
                    authNames.Add(d.DeptHeadAuthorization.Staff.Name);
            }
            ViewBag.HeadList = unitOfWork.StaffRepository.Get(filter: x => x.StaffRole == "DeptHead");
            ViewBag.RepList = unitOfWork.StaffRepository.Get(filter: x => x.StaffRole == "DeptRep");
            ViewBag.Auth = authNames;
            ViewBag.DeptCount = unitOfWork.DepartmentRepository.Get().Count();
            return View(departments.ToList());
        }

        // GET: Departments/Details/5
        public ActionResult Details(string id)
        {
            Debug.WriteLine("welcome to department details, routevalue id = " + id);
            if (String.IsNullOrEmpty(id))
                return RedirectToAction("Index");
            
            Department department = unitOfWork.DepartmentRepository.Get(filter: x => x.ID == id, includeProperties: "CollectionPoint").First();
                
            CollectionPoint selected = department.CollectionPoint;
            ViewBag.SelectedPoint = selected.Location;
            ViewBag.OtherPoints = unitOfWork.CollectionPointRepository.Get(filter: x => x.Location != selected.Location);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.Department = department;
            Session["CurrentDepartmentID"] = department.ID;
            return View();
        }

        [HttpPost]
        public ActionResult UpdateCollectionPoint(string id)
        {
            string deptId = Session["CurrentDepartmentID"].ToString();
            Department department = unitOfWork.DepartmentRepository.GetByID(deptId);
            CollectionPoint collectionPoint = unitOfWork.CollectionPointRepository.GetByID(int.Parse(id));
            if (collectionPoint != null) {
                department.CollectionPoint = collectionPoint;
                unitOfWork.DepartmentRepository.Update(department);
                unitOfWork.Save();
                Debug.WriteLine("Collection point for " + department.DeptName + " update to " + collectionPoint.Location);
            }
            return RedirectToAction("Details", new { id = deptId });
        }

       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult SelectCollectionPoint (string id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = unitOfWork.DepartmentRepository.GetByID(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.CollectionPointID = new SelectList(unitOfWork.CollectionPointRepository.Get(), "ID", "Location", department.CollectionPoint.ID);
            return View(department);
        }


        //Go to view for dep head only to select staff for delegation
        public ActionResult DelegateAuthority()
        {
            UnitOfWork uow = new UnitOfWork();
            string dept = loginService.StaffFromSession.DepartmentID;
            //show history and form together
            //get a list of auths (history)
            List<DeptHeadAuthVM> vmlist = departmentService.GetDeptHeadAuthorizationVMs(dept, uow);
            if(vmlist == null)
            {
                Debug.WriteLine("EMPTY AUTHLIST!!");
            }

            //get a list of staff (new delegation)
            List<Staff> stafflist = staffService.GetStaffByDeptID(dept);

            //get a list of staff without head
            var staffwoheadlist = new List<Staff>();
            foreach(Staff s in stafflist)
            {
                if(s.StaffRole != "DeptHead")
                {
                    staffwoheadlist.Add(s);
                }
            }

            //get a list of staff without head/rep
            var staffonlylist = new List<Staff>();
            foreach (Staff s in stafflist)
            {
                if(s.StaffRole == "Staff")
                {
                    staffonlylist.Add(s);
                }
            }

            ViewBag.StaffList = stafflist;
            ViewBag.StaffWOHeadList = staffwoheadlist;
            ViewBag.StaffOnlyList = staffonlylist;

            //get current active delegation (if exists)
            ViewBag.CurrentDelegation = departmentService.IsActiveAuthExist(dept, out DeptHeadAuthorization auth, uow) ? (new DeptHeadAuthVM(auth)) : null;

            ViewBag.DelegateError = TempData["delegateError"];
           
            return View(vmlist);
        }


        //behind the scene to process the submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DelegateAuthority(string StaffName, string StartDate, string EndDate)
        {
            UnitOfWork uow = new UnitOfWork();
            string deptID = loginService.StaffFromSession.DepartmentID;
            if (ModelState.IsValid)
            {
                Debug.WriteLine(Server.UrlDecode(StaffName) + " " + StartDate + " " + EndDate);
                string name = Server.UrlDecode(StaffName);
                if(!departmentService.SubmitNewAuth(name, StartDate, EndDate, deptID))
                    TempData["delegateError"] = "Please try again";
                return RedirectToAction("DelegateAuthority");
            }
            return RedirectToAction("Details", new { id = deptID});
        }




        //End Delegation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelAuthorization()
        {
            string deptID = loginService.StaffFromSession.DepartmentID;
            if (departmentService.CancelAuth(deptID))
                Debug.WriteLine("Auth for" + deptID + "canceled");

            return RedirectToAction("DelegateAuthority");
        }

    }
}
