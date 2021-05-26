using Core.Common.Cryptography;
using Core.Common.Miscellaneous;
using Core.Model;
using Core.Services;
using Core.Services.Billing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace ERentWebUI.Controllers
{
    public class UserController : BaseController
    {
        eRentEntities DbContext;

        public UserController()
        {
            DbContext = new eRentEntities();
        }

        #region User

        public ActionResult UserProfile()
        {
            User data = DbContext.Users.Find(Session["UserID"].ToInt());
            data.Password = Encrypt_Dycrypt.Decrypt(data.Password);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateProfile(UserModel input)
        {
            ResponseModel resp = new ResponseModel();

            string LocalPath = "";
            string dateTime = DateTime.Now.ToString("MMddyyyyhhmmss");
            try
            {
                string ImgPath = "";
                if (input.UserImage != null)
                {
                    double Kbs = (input.UserImage.ContentLength / 1024);
                    double Mbs = Kbs / 1024;
                    if (Mbs > 5)
                    {
                        resp.isSuccess = false;
                        resp.msg = "Max 5 MB";
                        return Json(resp, JsonRequestBehavior.AllowGet);
                    }
                    var allowedExtensions = new[] { ".Jpg", ".JPG", ".jpg", ".png", ".PNG", "jpeg" };
                    if (!allowedExtensions.Contains(Path.GetExtension(input.UserImage.FileName)))
                    {
                        resp.isSuccess = false;
                        resp.msg = "Please choose only Image file";
                        return Json(resp, JsonRequestBehavior.AllowGet);
                    }
                    LocalPath = Server.MapPath(@"/Content/UserPics/") + dateTime + input.UserImage.FileName;
                    ImgPath = ConfigurationManager.AppSettings["BaseUrl"].ToString() + "Content/UserPics/" + dateTime + input.UserImage.FileName;
                    input.UserImage.SaveAs(LocalPath);
                }

                var User = DbContext.Users.Where(x => x.EmailAddress == input.EmailAddress).FirstOrDefault();

                User.FirstName = input.FirstName;
                User.LastName = input.LastName;
                User.Image = ImgPath;
                User.DOB = input.DOB;
                User.JobTitle = input.JobTitle;
                User.CompanyName = input.CompanyName;
                User.Address = input.Address;
                User.City = input.City;
                User.StateID = input.StateID;
                User.ZipCode = input.ZipCode;
                User.CompanyAccountInformation = input.CompanyAccountInformation;
                User.MobileNumber = input.MobileNumber;
                User.Password = Encrypt_Dycrypt.Encrypt(input.Password);

                DbContext.Entry(User).State = EntityState.Modified;
                DbContext.SaveChanges();
                CreateSession(User);
                resp.isSuccess = true;
                resp.msg = "Profile has been updated";
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists(LocalPath))
                {
                    System.IO.File.Delete(LocalPath);
                }
                //DbContext.Dispose();
                resp.isSuccess = false;
                resp.isError = true;
                resp.msg = "An Error Occured";
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        public void CreateSession(User data)
        {
            Session["UserInfo"] = data;
            Session["UserID"] = data.Id;
        }
        #endregion

        #region Checkout

        string PO()
        {
            string val = new Random().Next(10000, 99999).ToString();
            if (DbContext.ReservationHdrs.Where(x => x.CustomerPO == val).FirstOrDefault() == null ? false : true)
            {
                return PO();
            }
            return val;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddReservation(ReservationHdr input, HttpPostedFileBase InsuranceFile)
        {
            string LocalPath = "";
            ResponseModel resp = new ResponseModel();
            try
            {
                string dateTime = DateTime.Now.ToString("MMddyyyyhhmmss");
                string ImgPath = "";
                if (InsuranceFile == null)
                {
                    resp.isSuccess = false;
                    resp.msg = "Please attach insurance policy";
                    return Json(resp, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    double Kbs = (InsuranceFile.ContentLength / 1024);
                    double Mbs = Kbs / 1024;
                    if (Mbs > 5) { resp.isSuccess = false; resp.msg = "Max 5 MB"; return Json(resp, JsonRequestBehavior.AllowGet); }
                    var allowedExtensions = new[] { ".jpg", ".png", "jpeg", ".doc", ".docx", ".pdf", ".xls", ".xlsx", ".ppt", ".pptx" };
                    if (!allowedExtensions.Contains(Path.GetExtension(InsuranceFile.FileName).ToLower()))
                    {
                        resp.isSuccess = false;
                        resp.msg = "Please atttach only Image or Document file";
                        return Json(resp, JsonRequestBehavior.AllowGet);
                    }
                    LocalPath = Server.MapPath(@"/Content/Documents/") + dateTime + InsuranceFile.FileName;
                    ImgPath = ConfigurationManager.AppSettings["BaseUrl"].ToString() + "Content/Documents/" + dateTime + InsuranceFile.FileName;
                    InsuranceFile.SaveAs(LocalPath);
                    input.Insurance = ImgPath;
                    input.TotalRentAmount = input.ReservationDtls.Select(x => x.RentPrice).Sum();
                    input.DocumentNumber = "";
                    input.ReservationStatusId = 1;
                    input.customerNumber = Session["UserID"].ToInt();
                    input.CreatedBy = Session["UserID"].ToInt();
                    input.CreatedOn = DateTime.Now;
                    input.IsDeleted = false;
                    var Hdr = DbContext.ReservationHdrs.Add(input);
                    DbContext.SaveChanges();
                    resp.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists(LocalPath))
                {
                    System.IO.File.Delete(LocalPath);
                }
                resp.isSuccess = false;
                resp.isError = true;
                resp.msg = "An Error Occured";
            }
            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCartData(string input)
        {
            ResponseModel resp = new ResponseModel();
            List<CartModel> list = new List<CartModel>();
            try
            {
                var Cart = JsonConvert.DeserializeObject<List<CartModel>>(input).Where(x => x != null && x.ModelID != null).GroupBy(p => p.ModelID).Select(grp => grp.FirstOrDefault()).ToList();
                foreach (var item in Cart)
                {
                    CartModel model = new CartModel();
                    var data = DbContext.ForkliftsModels.Where(x => x.Id == item.ModelID).FirstOrDefault();
                    model.ModelID = data.Id;
                    model.Picture = data.Picture;
                    model.Model = data.Model;
                    model.Make = data.Make;
                    model.Capacity = data.Capacity;
                    model.Description = data.Description;
                    model.Quantity = item.Quantity;
                    model.StartDate = item.StartDate;
                    model.ReturnDate = item.ReturnDate;
                    model.PricingStandard_Daily = data.PricingStandard_Daily;
                    model.PricingStandard_Weekly = data.PricingStandard_Weekly;
                    model.PricingStandard_Montly = data.PricingStandard_Montly;
                    model.PurchasePrice = data.PurchasePrice;
                    model.ForkliftsCharges = DbContext.ForkliftsModelCharges.Where(x => x.ForkliftsModelId == item.ModelID).Select(x => new { ChargesName = x.ChargesName, Amount = x.Amount }).ToList();
                    list.Add(model);
                }
                resp.isSuccess = true;
                resp.CartData = Cart.Select(x => new { ModelID = x.ModelID, StartDate = x.StartDate, ReturnDate = x.ReturnDate, Quantity = x.Quantity }).ToList();
                resp.data = list;
            }
            catch (Exception ex)
            {
                resp.isSuccess = false;
                resp.isError = true;
                resp.msg = "An Error Occured";
            }
            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckOut()
        {
            ViewBag.PO = PO();

            return View();
        }

        #endregion

        #region Contract

        public ActionResult ProductTerm(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || DbContext.Users.Where(x => x.SecuredKey == id).FirstOrDefault() == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        public ActionResult ProductTerm1()
        {
            return View();
        }


        [HttpPost, ValidateInput(false)]
        public JsonResult SaveContract(string str)
        {
            string FilePath = Server.MapPath(@"~\Content\Documents\hh1.pdf");
            PdfGenerator.GeneratePdf(str, PdfSharp.PageSize.A4, 20).Save(FilePath);
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Billing

        public ActionResult Billing()
        {
            return View();
        }

        public JsonResult GetBilling(RequestModel request)
        {
            ResponseModel resp = new ResponseModel();
            request.search = HttpContext.Request.QueryString["search[value]"].ToString();
            try
            {
                request.UserID = Session["UserID"].ToInt();
                resp.data = new BillingService().BLLGetBilling(request);

                resp.recordsTotal = resp?.Count;
                resp.recordsFiltered = DbContext.ReservationHdrs.Where(x => x.IsDeleted == false && x.customerNumber == request.UserID).ToList().Count;
            }
            catch (Exception ex)
            {
                resp.isError = true;
                resp.msg = "An exception occured";
            }
            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReservationDtl(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Redirect("Billing");

            int UserID = Session["UserID"].ToInt();
            int ReservationID = id.ToInt();
            return View(DbContext.ReservationHdrs.Where(x => x.id == ReservationID && x.customerNumber == UserID && x.IsDeleted == false).FirstOrDefault());
        }

        #endregion

        [HttpPost]
        public JsonResult ChangePassword(string CurrentPassword, string Password, string ConfirmPassword)
        {
            ResponseModel resp = new ResponseModel();
            try
            {
                if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    resp.isSuccess = false;
                    resp.msg = "Form Contains Validation Error";
                    return Json(resp, JsonRequestBehavior.AllowGet);
                }
                if (Password != ConfirmPassword) { resp.isSuccess = false; resp.msg = "Password & confirm password must be matched"; return Json(resp, JsonRequestBehavior.AllowGet); }

                int UserID = Session["UserID"].ToInt();
                CurrentPassword = Encrypt_Dycrypt.Encrypt(CurrentPassword);
                User user = DbContext.Users.Where(x => x.Id == UserID && x.Password == CurrentPassword).FirstOrDefault();
                if (user != null)
                {
                    user.Password = Encrypt_Dycrypt.Encrypt(ConfirmPassword);
                    DbContext.Entry(user).State = EntityState.Modified;
                    DbContext.SaveChanges();
                    resp.isSuccess = true;
                    resp.msg = "Password has been changed successfully";
                }
                else
                {
                    resp.isSuccess = false;
                    resp.msg = "Invalid Current Password";
                }
            }
            catch (Exception ex)
            {
                resp.isSuccess = false;
                resp.isError = true;
                resp.msg = "An Error Occured";
            }
            return Json(resp, JsonRequestBehavior.AllowGet);
        }

    }
}