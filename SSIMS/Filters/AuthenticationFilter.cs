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
            int c = HttpContext.Current.Session.Count;
            int d = HttpContext.Current.Session.Keys.Count;
            Debug.WriteLine("Session count = " + c);
            Debug.WriteLine("Session count = " + d);

            if(c > 0)
            {
                string username = HttpContext.Current.Session[0].ToString();
                Debug.WriteLine("On Authentication : " + username + " / " + sessionid);
                if (loginService.AuthenticateSession(username, sessionid))
                {
                    authenticated = true;
                }
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (!authenticated)
            {
                Debug.WriteLine("Authentication Failed!");
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