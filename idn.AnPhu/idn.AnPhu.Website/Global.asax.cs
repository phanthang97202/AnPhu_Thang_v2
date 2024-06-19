using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace idn.AnPhu.Website
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // chỉ sử dụng Razor
            //ViewEngines.Engines.Clear();
            //ViewEngines.Engines.Add(new RazorViewEngine());

            // 1: Loại bỏ thông tin X - AspNetMvc - Version ở header
            MvcHandler.DisableMvcResponseHeader = true;
        }

        protected void Application_PreSendRequestHeaders()
        {
            //2.1: Remove Server Header
            Response.Headers.Remove("Server");
            //2.2: Remove X-AspNet-Version Header
            Response.Headers.Remove("X-AspNet-Version");
            //// Fix iframe
            //Response.Headers.Remove("X-Frame-Options");
            //Response.AddHeader("X-Frame-Options", "AllowAll");
        }
        // 3: Loại bỏ X-Powered-By (webconfig)
    }
}
