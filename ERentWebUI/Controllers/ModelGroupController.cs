using Core.Common.Communication;
using Core.Common.Cryptography;
using Core.Common.Miscellaneous;
using Core.Dto;
using Core.Model;
using Core.Services.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERentWebUI.Controllers
{
    public class ModelGroupController : Controller
    {

        eRentEntities DbContext;

        public ModelGroupController()
        {
            DbContext = new eRentEntities();
        }

        public ActionResult Index(int? id)
        {
            if (id==null)
            {
                return RedirectToAction("Index","Home");
            }
            ForkliftDto model = new ForkliftDto();
            model.ListForkliftsModel = DbContext.ForkliftsModels.Where(x => x.ForkliftsId == id && x.IsDeleted == false).ToList();
            model.ListForklifts = DbContext.Forklifts.Where(x => x.IsDeleted == false).ToList();

            return View(model);
        }

        public ActionResult EquipmentDetail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.id = id;
            ForkliftDto model = new ForkliftDto();
            model.ListForkliftsModel = DbContext.ForkliftsModels.Where(x => x.IsDeleted == false).ToList();
            model.ForkliftsModel = model.ListForkliftsModel.Where(x => x.Id == id).FirstOrDefault();
            return View(model);
        }
    }
}   