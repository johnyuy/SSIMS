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
        private bool authenticated = false;
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            ILoginService loginService = new LoginService();
            string sessionid = HttpContext.Current.Session.SessionID;
            if(HttpContext.Current.Session.Count > 0)
            {
                string username = HttpContext.Current.Session["username"].ToString();
                
                if (loginService.AuthenticateSession(username, sessionid))
                {
                    Debug.WriteLine("\n[Authentication Filter : \t" + username + " + " + sessionid +"]\n");
                    authenticated = true;
                }
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (!authenticated)
            {
                Debug.WriteLine("\n[Authentication Filter :\tFAILED!" +
                    "\nRedirecting to Login Page..");
                filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary
                        {
                            {"controller", "Login" },
                            {"action","Authentication" }
                        });
            }
        }

       
    }
}