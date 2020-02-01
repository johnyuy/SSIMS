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
        
        // GET: Login
        public ActionResult Authenthication()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authenticate(UserLogin userLogin)
        {
            ILoginService LoginService = new LoginService();
            if(LoginService.VerifyPassword(userLogin.Username, userLogin.Password))
            {
                Debug.WriteLine("\n");
                Debug.WriteLine("Session.SessionID = " + Session.SessionID);
                Debug.WriteLine("HttpContext.Session.SessionID = "+ HttpContext.Session.SessionID);
                return RedirectToAction("Index", "Home");
            }
                
            
            return RedirectToAction("Authenthication", userLogin);
        }
    }
}