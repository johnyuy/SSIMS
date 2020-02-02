using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSIMS.Service;
using SSIMS.ViewModels;

namespace SSIMS.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Authentication");
        }
        
        // GET: Login
        public ActionResult Authentication()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authenticate(UserLogin userLogin)
        {
            ILoginService LoginService = new LoginService();
            if(LoginService.VerifyPassword(userLogin.Username, userLogin.Password))
            {
               
                LoginService.CreateNewSession(userLogin.Username,HttpContext.Session.SessionID);
                return RedirectToAction("Index", "Home");
            }
                
            return RedirectToAction("Authentication", userLogin);
        }
    }
}