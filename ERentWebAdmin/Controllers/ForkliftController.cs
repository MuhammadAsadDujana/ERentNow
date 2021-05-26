using Core.Dto;
using Core.Model;
using Core.Services.ForkliftService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERentWebAdmin.Controllers
{
    public class ForkliftController : Controller
    {
        // GET: Forklift
        private readonly ForklistService _service;
        public ForkliftController()
        {
            _service = new ForklistService();
        }


        [HttpGet]
        public ActionResult ForkliftManagement()
        {
            try
            {
                var list = _service.GetForkliftsList();
                if (list.Count() > 0)
                    return View(list);
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return View();
            }
        }

        [HttpGet]
        public ActionResult CreateForklift()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return View();
            }
        }

        [HttpPost]
        public JsonResult CreateForklift(ForkliftDto forkliftDto)
        {
            string Message = "";
            try
            {
                if (ModelState.IsValid)
                {
                    string saveFilePath = "";
                    if (forkliftDto.PictureWrapper != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(forkliftDto.PictureWrapper.FileName);
                        string extension = Path.GetExtension(forkliftDto.PictureWrapper.FileName);
                        fileName = "Content/ProfilePictures/" + fileName + DateTime.Now.ToString("yymmssff") + extension;
                        string path = ConfigurationManager.AppSettings["liveUrl"] + fileName;

                        fileName = Path.Combine(Server.MapPath("~/"), fileName);
                        forkliftDto.PictureWrapper.SaveAs(fileName);
                        saveFilePath = path;
                        //  encImage = TripleDESCryptography.Encrypt(path);
                    }
                    else
                    {
                        saveFilePath = "";
                    }

                    Forklift newForklift = new Forklift
                    {
                        Name = forkliftDto.Name,
                        Picture = saveFilePath == "" ? null : saveFilePath,
                        Description = forkliftDto.Description,
                        CreatedBy = (int)Session["UserId"],
                        CreatedOn = DateTime.Now,
                        IsActive = true
                    };

                    
                    var res = _service.InsertForklift(newForklift);

                    if (res != null)
                        return Json(true);

                }
                Message = "Record not inserted";
                return Json(Message);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Message = ex.Message;
                return Json(Message);
            }
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public JsonResult ChangeForkliftStatus(int forkliftId, bool status)
        {
            string Message = "";
            try
            {
                if (!forkliftId.Equals(null))
                {
                    var res = _service.GetForkliftById(forkliftId);

                    res.IsActive = status;
                    //  user.IsActive = userStatus == 1 ? true : false;
                    var data = _service.UpdateForklift(res);
                    if (data != null)
                        return Json(true);

                }

                Message = "Record not updated";
                return Json(Message);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Message = ex.Message;
                return Json(Message);
            }

        }

        [HttpGet]
        public ActionResult EditForklift(int forkliftId)
        {
            try
            {
                var data = _service.GetForkliftById(forkliftId);

                if (data != null)
                {
                    return View(data);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return View();
            }
        }

        [HttpGet]
        public ActionResult ForkliftModelManagement()
        {
            try
            {
                var list = _service.GetForkliftModelList();
                if (list.Count() > 0)
                    return View(list);
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return View();
            }
        }

        [HttpGet]
        public ActionResult CreateForkliftModel()
        {
            var ddlForkliftsList = _service.GetForkliftsList();
            if(ddlForkliftsList.Count() > 0)
            ViewBag.ddlForkliftsList = ddlForkliftsList;

            return View();
       
        }

        [HttpPost]
        public JsonResult CreateForkliftModel(ForkliftModelViewModel model)
        {
            ResponseModel resp = new ResponseModel();
            try
            {
              //  return null;
                if (ModelState.IsValid)
                {
                    string saveFilePath = "";
                    if (model.imageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(model.imageUpload.FileName);
                        string extension = Path.GetExtension(model.imageUpload.FileName);
                        fileName = "Content/ProfilePictures/" + fileName + DateTime.Now.ToString("yymmssff") + extension;
                        string path = ConfigurationManager.AppSettings["liveUrl"] + fileName;

                        fileName = Path.Combine(Server.MapPath("~/"), fileName);
                        model.imageUpload.SaveAs(fileName);
                        saveFilePath = path;
                        //  encImage = TripleDESCryptography.Encrypt(path);
                    }
                    else
                    {
                        saveFilePath = "";
                    }

                    resp = _service.InsertForkliftModel(model, saveFilePath);
                }
            }
            catch (Exception ex)
            {
                resp.isSuccess = false;
                resp.isError = true;
                resp.msg = "Something went wrong " + ex.Message;
            }
            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult deleteForklift(int forkliftId)
        {
            string msg = "";
            try
            {
                var data = _service.DeleteForkliftById(forkliftId);
                if (data != null)
                    return Json(true);
                else
                {
                    msg = "Something went wrong record is not deleted";
                    return Json(msg);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                msg = ex.Message;
                return Json(msg);
            }

        }

        [HttpPost]
        public JsonResult deleteForkliftModel(int forkliftModelId)
        {
            string msg = "";
            try
            {
                var data = _service.DeleteForkliftModelById(forkliftModelId);
                if (data != null)
                    return Json(true);
                else
                {
                    msg = "Something went wrong record is not deleted";
                    return Json(msg);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                msg = ex.Message;
                return Json(msg);
            }

        }

        [HttpGet]
        public ActionResult ViewForkliftModel(int forkliftModelId)
        {
            try
            {
                var data = _service.GetForkliftModelById(forkliftModelId);

                if (data != null)
                {
                    return View(data);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return View();
            }
        }

        [HttpGet]
        public ActionResult EditForkliftModel(int forkliftModelId)
        {
            try
            {
                var data = _service.GetForkliftModelById(forkliftModelId);

                if (data != null)
                {
                    return View(data);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return View();
            }
        }

        [HttpPost]
        public JsonResult ChangeForkliftModelStatus(int forkliftId, bool status)
        {
            string Message = "";
            try
            {
                if (!forkliftId.Equals(null))
                {
                    var res = _service.GetForkliftModelById(forkliftId);

                    res.IsActive = status;
                    //  user.IsActive = userStatus == 1 ? true : false;
                    var data = _service.UpdateForkliftModel(res);
                    if (data != null)
                        return Json(true);

                }

                Message = "Record not updated";
                return Json(Message);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Message = ex.Message;
                return Json(Message);
            }

        }

    }
}