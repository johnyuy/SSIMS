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
            Debug.WriteLine("In index now");
            if (Request.Cookies.Get("Auth") != null)
            {
                Debug.WriteLine("Auth cookie exists");
                //if cookie's session ID matches the sessionId in database then go to home controller
                if (LoginService.IsStoredSession(Request.Cookies.Get("Auth")))
                {
                    Debug.WriteLine("Auth cookie is valid");
                    updateAuthCookie(
                        Session["username"].ToString(),
                        HttpContext.Session.SessionID);
                    Logger(Session["username"].ToString(),true);
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
                LoginService.UpdateSession(userLogin.Username, HttpContext.Session.SessionID);
                updateAuthCookie(userLogin.Username, HttpContext.Session.SessionID);
                Logger(userLogin.Username, true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMsg = "Incorrect username or password";
                return RedirectToAction("Authentication", userLogin);
            }
                
           
        }

        private void updateAuthCookie(string username, string sessionid)
        {
            HttpCookie AuthCookie = Request.Cookies.Get("Auth");
            if (AuthCookie == null)
                AuthCookie = new HttpCookie("Auth");
            AuthCookie.Value = username + "#" + sessionid;
            AuthCookie.Expires = DateTime.Now.AddDays(7d);
            Response.Cookies.Add(AuthCookie);
        }

        public ActionResult Logout(string username)
        {
            LoginService.CancelSession(username);
            if(Request.Cookies.Get("Auth")!=null)
                Response.Cookies["Auth"].Expires = DateTime.Now.AddDays(-100);
            HttpContext.Session.Clear();
            HttpContext.Session.Abandon();
            Logger(username, false);
            return RedirectToAction("Authentication", "");
        }

        private static void Logger(string username, bool login)
        {
            string log = "\n[Logger: "+ username + " logged ";
            if (login) log += "in"; else log += "out";
            log +=  " at " + DateTime.Now.ToString("HH:mm:ss")
                    + " on " + DateTime.Now.ToString("dd/MM/yyyy")
                    + "]";
            Debug.WriteLine(log);
        }
    }
}