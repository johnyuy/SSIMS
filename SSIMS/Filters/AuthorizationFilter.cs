using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Web.Routing;

namespace SSIMS.Filters
{
    public class AuthorizationFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            

            Debug.WriteLine("\nController called: " + filterContext.ActionDescriptor.ControllerDescriptor.ControllerName);
            Debug.WriteLine("Action called: " + filterContext.ActionDescriptor.ActionName);
            GenerateAccessGroup();
            filterContext.Controller.ViewBag.usergroup = HttpContext.Current.Session["userGroup"];
            filterContext.Controller.ViewBag.usertype = HttpContext.Current.Session["userType"];

            //implement authorization challenge here

        }

        private static void GenerateAccessGroup()
        {
            string userGroup = "";
            string userType = HttpContext.Current.Session["role"].ToString();
            if (!String.IsNullOrEmpty(userType))
            {
                if (userType == "head" || userType == "rep" || userType == "staff")
                    userGroup = "dept";
                if (userType == "manager" || userType == "supervisor" || userType == "clerk")
                    userGroup = "store";
                Debug.Write("[Authorization Filter :  " + userType + "-" + userGroup + "]");
                HttpContext.Current.Session["userGroup"] = userGroup;
                HttpContext.Current.Session["userType"] = userType;
            }
        }
    }
}