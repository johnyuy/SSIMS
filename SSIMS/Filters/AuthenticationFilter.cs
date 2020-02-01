using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Filters;
using System.Diagnostics;
using System.Web.Mvc;

namespace SSIMS.Filters
{
    public class AuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        private bool _auth;
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            
            string sessionid = HttpContext.Current.Session.SessionID;
            string username = HttpContext.Current.Session[0].ToString();
            Debug.WriteLine("On Authentication : " + username + " / " + sessionid);
            _auth = true;
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            //throw new NotImplementedException();
        }

       
    }
}