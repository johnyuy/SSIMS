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
        readonly ILoginService LoginService = new LoginService();
        public ActionResult Index()
        {
            if (Request.Cookies.Get("Auth") != null)
            {
                //if cookie's session ID matches the sessionId in database then go to home controller
                if (LoginService.IsStoredSession(Request.Cookies.Get("Auth")))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            //go to login
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
            
            if(LoginService.VerifyPassword(userLogin.Username, userLogin.Password))
            {
                String sessionId = LoginService.CreateNewSession(userLogin.Username, HttpContext.Session.SessionID);
                HttpCookie AuthCookie = new HttpCookie("Auth", userLogin.Username+"#"+sessionId);
                AuthCookie.Expires = DateTime.Now.AddDays(7d);
                Response.Cookies.Add(AuthCookie);
                return RedirectToAction("Index", "Home");
            }
                
            return RedirectToAction("Authentication", userLogin);
        }

        public ActionResult Logout(string username)
        {
            LoginService.CancelSession(username);
            Response.Cookies["SessionID"].Expires = DateTime.Now.AddDays(-100);
            Response.Cookies["Username"].Expires = DateTime.Now.AddDays(-100);
            HttpContext.Session.Clear();
            HttpContext.Session.Abandon();
            Debug.WriteLine("\n[" + username + " logged out]");
            return RedirectToAction("Authentication", "");
        }
    }
}