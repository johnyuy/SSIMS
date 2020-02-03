using System.Web;
using System.Web.Mvc;
using SSIMS.Filters;

namespace SSIMS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new AuthenticationFilter());
            
        }
    }
}
