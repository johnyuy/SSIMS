using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Filters;
using System.Diagnostics;
using System.Web.Mvc;
using SSIMS.Service;

namespace SSIMS.Filters
{
    public class AuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        private bool authenticated;
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            authenticated = false;
            HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
            HttpContext.Current.Response.AddHeader("Expires", "0");

            ILoginService loginService = new LoginService();
            if(HttpContext.Current.Session.Count > 0)
            {
                if (loginService.AuthenticateSession(
                    HttpContext.Current.Session["username"].ToString(), 
                    HttpContext.Current.Session.SessionID))
                {
                    Debug.WriteLine("\n[Authentication Filter :\tSUCCESS!]\n");
                    authenticated = true;
                }
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (!authenticated)
            {
                Debug.WriteLine("\n[Authentication Filter :\tFAILED!]" +
                    "\nRedirecting to Login Page..");
                filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary
                        {
                            {"controller", "Login" },
                            {"action","Index" }
                        });
            }
        }

       
    }
}