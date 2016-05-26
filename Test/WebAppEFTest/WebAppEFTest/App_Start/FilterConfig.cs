using System.Web;
using System.Web.Mvc;
using WebAppEFTest.Filters;

namespace WebAppEFTest
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new EmployeeExceptionFilter());
            //filters.Add(new HandleErrorAttribute());
            //filters.Add(new AuthorizeAttribute());
        }
    }
}
