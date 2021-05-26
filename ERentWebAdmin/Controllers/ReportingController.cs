using Core.Common.Helper;
using Core.Services.ReportingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERentWebAdmin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportingController : Controller
    {
        ReportingService _service;
        public ReportingController()
        {
            _service = new ReportingService();
        }
        // GET: Reporting
        public ActionResult BillingHistroy()
        {
            var list = _service.getAllReservationList();
            if (list.Count() > 0)
                return View(list);
            else
                return View();
  
        }

        [HttpGet]
        public ActionResult ReservationDetails(int ReservationId)
        {
            var list = _service.getReservationDetailById(ReservationId);
            ViewBag.CurrentReservationStatus = list.FirstOrDefault().ReservationStatus != null ? list.FirstOrDefault().ReservationStatus : null;
            ViewBag.ReservationId = list.FirstOrDefault().ReservationId;

            var ReservationStatusList = _service.GetReservationStatusList();
            if(ReservationStatusList.Count() > 0)
                ViewBag.ddlReservationStatusList = ReservationStatusList;

            if (list.Count() > 0)
                return View(list);
            else
                return View();
        }

        [HttpPost]
        public JsonResult UpdateReservationStatus(int ReservationId, int StatusId)
        {
            var data = _service.UpdateReservationStatusService(ReservationId, StatusId);
            if(data == ConstantMessages.Success)
                return Json(true);
            else
                return Json(false);
        }


    }
}