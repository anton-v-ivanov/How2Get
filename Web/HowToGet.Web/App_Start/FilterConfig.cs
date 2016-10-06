using System.Web.Mvc;
using HowToGet.Web.Filters;

namespace HowToGet.Web.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new HttpsAttribute());
        }
    }
}