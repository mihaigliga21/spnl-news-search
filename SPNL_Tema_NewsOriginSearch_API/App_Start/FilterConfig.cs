using System.Web;
using System.Web.Mvc;

namespace SPNL_Tema_NewsOriginSearch_API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
