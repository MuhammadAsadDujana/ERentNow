using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERentWebUI.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string action = filterContext.ActionDescriptor.ActionName;
            string currentController = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var paramss = filterContext.ActionDescriptor.GetParameters();
            var attrFilter = filterContext.ActionDescriptor.GetFilterAttributes(true);

            if (Session["UserID"] != null)
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Home/Index"); //redirect Statement
            }
        }

    }
}