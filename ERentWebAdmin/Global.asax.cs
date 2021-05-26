using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ERentWebAdmin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
          //  var a = HttpContext.Current.Session.SessionID;
            System.Diagnostics.Debug.WriteLine("Session_Start");
        }
        protected void Session_End(object sender, EventArgs e)
        {
            //if(HttpContext.Current.Session.SessionID.Length > 0)
            //{
            //    var a = Session["loginHistoryId"].ToString();
            //}
            System.Diagnostics.Debug.WriteLine("Session_End");
        }

        protected void Application_Error(object sender, EventArgs args)
        {
            Exception ex = Server.GetLastError();
            Server.ClearError();
            Response.Redirect("/Home/ErrorMessage");
        }

    }
}
