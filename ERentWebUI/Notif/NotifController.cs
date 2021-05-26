using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERentWebUI.Notif
{
    public class NotifController : Controller
    {
        // GET: Notif

        public JsonResult NotifData()
        {
            var fg = NotifBll.GetNotification();

            return Json("", JsonRequestBehavior.AllowGet);
        }


        public ActionResult NotifDatsa()
        {

            return View();
        }

    }
}