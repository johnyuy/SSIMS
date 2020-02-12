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
            if (!LoginService.IsAuthorizedRoles("clerk", "supervisor", "manager"))
                return RedirectToAction("Index", "Home");

            var departments = unitOfWork.DepartmentRepository.Get(includeProperties: "CollectionPoint");
            ViewBag.RepList = unitOfWork.StaffRepository.GetDeptRepList();
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


            Staff staff = loginService.StaffFromSession;

            if (!LoginService.IsAuthorizedRoles("head", "rep"))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
            //Session["CurrentDepartmentID"] = department.ID;
            return View();
        }



        // GET: Departments/Details/id
        public ActionResult Details2(String id)
        {
            if (!LoginService.IsAuthorizedRoles("clerk", "supervisor", "manager"))
                return RedirectToAction("Index", "Home");

            var department = unitOfWork.DepartmentRepository.Get(filter: x => x.ID == id, includeProperties: "DeptRep,DeptHead,CollectionPoint,DeptHeadAuthorization.Staff").First();

            
            return View(department);
        }


        [HttpPost]
        public ActionResult UpdateCollectionPoint(string id)
        {
            UnitOfWork uow = new UnitOfWork();
            if (!LoginService.IsAuthorizedRoles("head","rep"))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            string deptId = loginService.StaffFromSession.DepartmentID;
            Department department = uow.DepartmentRepository.GetByID(deptId);
            CollectionPoint collectionPoint = uow.CollectionPointRepository.GetByID(int.Parse(id));
            if (collectionPoint != null) {
                department.CollectionPoint = collectionPoint;
                uow.DepartmentRepository.Update(department);
                uow.Save();
                Debug.WriteLine("Collection point for " + department.DeptName + " updated to " + collectionPoint.Location);
            }
            return RedirectToAction("Dashboard", "Home");
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
            if (!LoginService.IsAuthorizedRoles("head", "rep")) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
            if (!LoginService.IsAuthorizedRoles("head"))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            UnitOfWork uow = new UnitOfWork();
            string dept = loginService.StaffFromSession.DepartmentID;

            Debug.WriteLine("Staff is from " + dept);
            Department department = uow.DepartmentRepository.Get(filter: x => x.ID == dept, includeProperties: "DeptRep").First();

            //get current active delegation (if exists)
            ViewBag.CurrentDelegation = departmentService.IsActiveAuthExist(dept, out DeptHeadAuthorization auth, uow) ? (new DeptHeadAuthVM(auth)) : null;

            ViewBag.DelegateError = TempData["delegateError"];


            //show history and form together
            //get a list of auths (history)
            List<DeptHeadAuthVM> vmlist = departmentService.GetDeptHeadAuthorizationVMs(dept, uow);
            if(vmlist == null)
            {
                Debug.WriteLine("EMPTY AUTHLIST!!");
            }

            //get a list of staff (new delegation)
            List<Staff> stafflistfull = staffService.GetStaffByDeptID(dept);

            //get a list of staff without head
            var stafflistnohead = new List<Staff>();
            foreach (Staff s in stafflistfull)
            {
                if(s.StaffRole != "DeptHead")
                    stafflistnohead.Add(s);
            }

            //get a list of staff without head/rep
            var stafflistordinary = new List<Staff>();
            foreach (Staff s in stafflistfull)
            {
                if(s.StaffRole == "Staff")
                    stafflistordinary.Add(s);
            }

            ViewBag.StaffList = stafflistfull;
            ViewBag.StaffWOHeadList = stafflistnohead;
            ViewBag.StaffOnlyList = stafflistordinary;


            Staff selected = department.DeptRep;
            ViewBag.SelectedRep = selected.Name;
                
            if (department == null)
                return HttpNotFound();
            ViewBag.Department = department;
            Session["CurrentDepartmentID"] = department.ID;

            return View(vmlist);
        }


        //behind the scene to process the submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DelegateAuthority(string StaffName, string StartDate, string EndDate)
        {
            if (!LoginService.IsAuthorizedRoles("head"))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            string deptID = loginService.StaffFromSession.DepartmentID;
            if (ModelState.IsValid)
            {
                Debug.WriteLine(Server.UrlDecode(StaffName) + " " + StartDate + " " + EndDate);
                string name = Server.UrlDecode(StaffName);
                if(!departmentService.SubmitNewAuth(name, StartDate, EndDate, deptID))
                {
                    TempData["delegateError"] = "Please try again";
                }
                else
                {
                    if(!loginService.StaffToAuthorizedHead(name,true))
                        TempData["delegateError"] = "Failed to authorize!";
                }
                return RedirectToAction("DelegateAuthority");
            }
            return RedirectToAction("Details", new { id = deptID});
        }

        //End Delegation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelAuthorization()
        {
            if (!LoginService.IsAuthorizedRoles("head"))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            string deptID = loginService.StaffFromSession.DepartmentID;
            if (departmentService.CancelAuth(deptID))
            {
                Debug.WriteLine("Auth for" + deptID + "canceled");
            }
                

            return RedirectToAction("DelegateAuthority");
        }


        [HttpPost]
        public ActionResult UpdateDeptRep(int id)
        {
            if(!LoginService.IsAuthorizedRoles("head"))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            string deptId = loginService.StaffFromSession.DepartmentID;
            
            UnitOfWork uow = new UnitOfWork();
            Department department = uow.DepartmentRepository.Get(filter: x=> x.ID == deptId, includeProperties:"DeptRep").FirstOrDefault();
            
            Staff NewRep = uow.StaffRepository.GetByID(id);
            NewRep.StaffRole = "DeptRep";
            uow.StaffRepository.Update(NewRep);
            loginService.UpdateDeptAccessByRole(NewRep.Name, uow);
            Debug.WriteLine("Updating new department rep to " + NewRep.Name);

            Staff OldRep = department.DeptRep;
            OldRep.StaffRole = "Staff";
            uow.StaffRepository.Update(OldRep);
            loginService.UpdateDeptAccessByRole(OldRep.Name, uow);
            Debug.WriteLine("Removed old department rep " + OldRep.Name);

            department.DeptRep = NewRep;
            uow.DepartmentRepository.Update(department);
            Debug.WriteLine("Updated department " + department.ID);

            uow.Save();

            return RedirectToAction("DelegateAuthority", new { id = deptId });
        }


    }
}
