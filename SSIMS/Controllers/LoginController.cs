using System;
using System.Collections.Generic;
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
            LoginService.VerifyPassword("ssimstestaccount", "password33");
            
            return RedirectToAction("Authenthication", userLogin);
        }
    }
}