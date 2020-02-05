using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSIMS.Filters;
using System.Diagnostics;
using SSIMS.Service;
using SSIMS.Models;

namespace SSIMS.Controllers
{
    [AuthenticationFilter]
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
    }
}