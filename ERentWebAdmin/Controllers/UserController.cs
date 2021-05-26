using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Core.Common.Communication;
using Core.Common.Cryptography;
using Core.Common.Helper;
using Core.Common.Miscellaneous;
using Core.Dto;
using Core.Model;
using Core.Services.UserService;

namespace ERentWebAdmin.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _service;
        public UserController()
        {
            _service = new UserService();
        }

        // GET: User
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            HttpCookie cookie = Request.Cookies["UserCookie"];
            if (cookie != null)
            {
                string encryptedPass = cookie["password"].ToString();
                byte[] pass = Convert.FromBase64String(encryptedPass);
                string decryptedPass = ASCIIEncoding.ASCII.GetString(pass);
                ViewBag.email = cookie["email"].ToString();
                ViewBag.password = decryptedPass.ToString();
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Login(string email, string password, bool RememberMe)
        {
            UserModel userModel = new UserModel();
            try
            {
                if (ModelState.IsValid)
                {


                    userModel.EmailAddress = email;
                    userModel.Password = password;
                    userModel.IsLogin = true;
                    userModel.IsActive = true;


                    var userList = _service.GetUserInfo(userModel);

                    if (userList.Count > 0)
                    {
                        string encPass = Encrypt_Dycrypt.Encrypt(password);

                        var user = userList.Where(x => x.EmailAddress == email && x.Password == encPass && (x.RoleId == 1 || x.RoleId == 2)).FirstOrDefault();

                        if (user != null)
                        {
                            

                            Session["UserId"] = user.Id;
                            Session["RoleId"] = user.RoleId;
                            Session["Email"] = user.EmailAddress;
                            var SessionId = HttpContext.Session.SessionID;

                            if (user.RoleId == 2)
                            {
                                UserLoginHistory loginHistory = new UserLoginHistory
                                {
                                    UserId = user.Id,
                                    LoginTime = DateTime.Now,
                                    Offline = true,
                                 //   SessionId = Convert.ToInt32(SessionId)
                                };
                                var res = _service.InsertUserLoginHistory(loginHistory);
                                if (res != null)
                                    Session["loginHistoryId"] = loginHistory.Id;
                            }

                            FormsAuthentication.SetAuthCookie(user.EmailAddress, false);
                            HttpCookie cookie = new HttpCookie("UserCookie");
                            if (RememberMe == true)
                            {
                                byte[] pass = ASCIIEncoding.ASCII.GetBytes(password);
                                string encryptedPass = Convert.ToBase64String(pass);
                                cookie["email"] = email;
                                cookie["password"] = encryptedPass;
                                cookie.Expires = DateTime.Now.AddHours(3);
                                HttpContext.Response.Cookies.Add(cookie);
                            }
                            else
                            {
                                cookie.Expires = DateTime.Now.AddDays(-1);
                                HttpContext.Response.Cookies.Add(cookie);
                            }

                            return Json(true, JsonRequestBehavior.AllowGet);
                        }

                    }
                    userModel.Message = "No user found";
                    return Json(userModel.Message);
                }
                else
                {
                    userModel.Message = "All fields required*";
                    return Json(userModel.Message);
                }
            }

            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                userModel.Message = ex.Message;
                return Json(userModel.Message);
            }

        }

        [HttpGet]
        //  [Authorize(Roles = "Admin")]
        public JsonResult Logout()
        {
            try
            {
                if ((int)Session["RoleId"] == 2)
                {
                    var data = _service.GetUserLoginHistory((int)Session["loginHistoryId"]);

                    data.LogoutTime = DateTime.Now;
                    data.Offline = false;

                    var res = _service.UpdateUserLoginHistory(data);
                }
                Session.Abandon();
                HttpContext.Session.Clear();
                FormsAuthentication.SignOut();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return Json(ex.Message);
            }

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult ForgotPassword(string email)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var EmailAdressCheck = Common.IsValidEmail(email);
                    if (!EmailAdressCheck.Equals("Success"))
                        return Json(EmailAdressCheck);

                    var user = _service.VerifyEmailadminService(email);
                    if (user == null)
                        return Json(ConstantMessages.EmailDoesNotExist);

                    user.SecuredKey = RandomNumber.RandomString(20, false) + DateTime.Now.ToString("mmddyyyyhhmmss");
                    user.IsActive = true;
                    _service.UpdateSecuredKeyService(user);

                    string Url = ConfigurationManager.AppSettings["liveUrl"].ToString() + "User/RecoverPassword/" + user.SecuredKey;
                    string senderEmail = ConfigurationManager.AppSettings["SenderEmail"].ToString();
                    Email.SendEmail(senderEmail, email, "Password Recovery", Url);

                    response.isSuccess = true;
                    response.msg = "Email has been sent";
                }

            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.isError = true;
                response.msg = "An Error Occured";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult RecoverPassword(string id)
        {
            var user = _service.RecoverPasswordService(id);
            if (user != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult UpdatePassword(string Password, string ConfirmPassword, string SecuredKey)
        {
            ResponseModel resp = new ResponseModel();
            try
            {
                if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword) || string.IsNullOrWhiteSpace(SecuredKey))
                {
                    resp.isSuccess = false;
                    resp.msg = "Form Contains Validation Error";
                    return Json(resp, JsonRequestBehavior.AllowGet);
                }
                if (Password != ConfirmPassword) 
                { 
                    resp.isSuccess = false; 
                    resp.msg = "Password & confirm password must be matched"; 
                    return Json(resp, JsonRequestBehavior.AllowGet); 
                }

                User user = _service.RecoverPasswordService(SecuredKey);
                if (user != null)
                {
                    user.Password = Encrypt_Dycrypt.Encrypt(ConfirmPassword);
                    user.SecuredKey = RandomNumber.RandomString(20, false) + DateTime.Now.ToString("MMddyyyyhhmmss");
                    _service.UpdateSecuredKeyService(user);
 
                    resp.isSuccess = true;
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

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ChangePassword(ChangePassViewModel model)
        {
            ResponseModel resp = new ResponseModel();
            try
            {
                var PasswordValidation = Common.CheckPasswordValid(model.NewPassword, model.ConfirmPassword);
                if (!PasswordValidation.isSuccess)
                    return Json(PasswordValidation, JsonRequestBehavior.AllowGet);

                model.UserId = (int)Session["UserId"];
                resp = _service.ChangePasswordService(model);

            }
            catch(Exception ex)
            {
                resp.isSuccess = false;
                resp.isError = true;
                resp.msg = "An Error Occured " + ex.Message;
            }
            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UserManagement()
        {
            var list = _service.GetUserProfileList();
            if (list.Count() > 0)
                return View(list);
            else
            {
                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateUser()
        {
            var ddlUserRoleList = _service.GetUserRoleList();
            var ddlStateList = _service.GetStateList();

            if (ddlUserRoleList.Count() > 0)
                ViewBag.ddlUserRoleList = ddlUserRoleList;

            if (ddlStateList.Count() > 0)
                ViewBag.ddlStateList = ddlStateList;


            return View();
        }

        [HttpPost]
        public JsonResult CreateUser(UserModel model)
        {
            ResponseModel resp = new ResponseModel();
            try
            {
                var PasswordValidation = Common.CheckPasswordValid(model.NewPassword, model.ConfirmPassword);
                if (!PasswordValidation.isSuccess)
                    return Json(PasswordValidation, JsonRequestBehavior.AllowGet);

                var EmailAdressCheck = Common.IsValidEmail(model.EmailAddress);
                if (!EmailAdressCheck.Equals("Success"))
                    return Json(EmailAdressCheck, JsonRequestBehavior.AllowGet);

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
                        model.Image = saveFilePath;
                        //  encImage = TripleDESCryptography.Encrypt(path);
                    }
                    else
                    {
                        saveFilePath = "";
                        model.Image = saveFilePath;
                     }

                var singUpUser = _service.SignUpAdminService(model);
                if (singUpUser == ConstantMessages.RegistrationSuccess)
                {
                    resp.isSuccess = true;
                    resp.msg = "User successfully added";
                }
                else
                {
                    resp.isSuccess = false;
                    resp.msg = "User not added successfully:" + singUpUser;
                }
             
            }
            catch(Exception ex)
            {
                resp.isSuccess = false;
                resp.isError = true;
                resp.msg = "Server side error " + ex.Message;
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult LoginHistory()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult GetLoginHistory()
        {
            string msg = "";
            var list = _service.GetUserLoginHistory();
            if (list.Count() > 0)
                return Json(list, JsonRequestBehavior.AllowGet);
            else
            {
                msg = "No record found";
                return Json(msg);
            }

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateCompanyProfile()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult CreateCompanyProfile(CompanySetupPageDto companySetupPageDto)
        {
            string Message = "";
            try
            {
                if (ModelState.IsValid)
                {
                    string saveFilePath = "";
                    if (companySetupPageDto.Logo != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(companySetupPageDto.Logo.FileName);
                        string extension = Path.GetExtension(companySetupPageDto.Logo.FileName);
                        fileName = "Content/ProfilePictures/" + fileName + DateTime.Now.ToString("yymmssff") + extension;
                        string path = ConfigurationManager.AppSettings["liveUrl"] + fileName;

                        fileName = Path.Combine(Server.MapPath("~/"), fileName);
                        companySetupPageDto.Logo.SaveAs(fileName);
                        saveFilePath = path;
                        //  encImage = TripleDESCryptography.Encrypt(path);
                    }
                    else
                    {
                        saveFilePath = "";
                    }

                    CompanySetupPage newCompanySetup = new CompanySetupPage
                    {
                        UserId = (int)Session["UserId"],
                        Logo = saveFilePath == "" ? null : saveFilePath,
                        Header = companySetupPageDto.Header,
                        Footer = companySetupPageDto.Footer,
                        BannerMassage = companySetupPageDto.BannerMassage,
                        TermsOfUse = companySetupPageDto.TermsOfUse,
                        PrivacyPolicy = companySetupPageDto.PrivacyPolicy,
                        CreatedBy = (int)Session["UserId"],
                        CreatedOn = DateTime.Now,
                        IsActive = true
                    };
                    var res = _service.InsertCompanySetupPage(newCompanySetup);

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

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult CompanyProfileManagement()
        {
      
            var list = _service.GetCompaniesProfileList();
            if (list.Count() > 0)
                return View(list);
            else
            {
                return View();
            }
        }

        [HttpPost]
        public JsonResult DeleteCompany(int companyId)
        {
            string msg = "";
            try
            {
                var data = _service.DeleteCompanyById(companyId);
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
        public ActionResult EditCompany(int companyId)
        {
            try
            {
                var data = _service.getCompanyById(companyId);

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
        public JsonResult EditCompany(CompanySetupPageDto companySetupPageDto)
        {
            string Message = "";
            try
            {
                ModelState.Remove("Logo");
                if (ModelState.IsValid)
                {
                    string saveFilePath = "";
                    if (companySetupPageDto.Logo != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(companySetupPageDto.Logo.FileName);
                        string extension = Path.GetExtension(companySetupPageDto.Logo.FileName);
                        fileName = "Content/ProfilePictures/" + fileName + DateTime.Now.ToString("yymmssff") + extension;
                        string path = ConfigurationManager.AppSettings["liveUrl"] + fileName;

                        fileName = Path.Combine(Server.MapPath("~/"), fileName);
                        companySetupPageDto.Logo.SaveAs(fileName);
                        saveFilePath = path;
                        //  encImage = TripleDESCryptography.Encrypt(path);
                    }
                    else
                    {
                        saveFilePath = companySetupPageDto.ProfileLogo;
                    }

                    var data = _service.getCompanyById(companySetupPageDto.CompanyId);

                    if(data != null)
                    {
                        data.Header = companySetupPageDto.Header;
                        data.Footer = companySetupPageDto.Footer;
                        data.BannerMassage = companySetupPageDto.BannerMassage;
                        data.TermsOfUse = companySetupPageDto.TermsOfUse;
                        data.PrivacyPolicy = companySetupPageDto.PrivacyPolicy;
                        data.UpdatedBy = (int)Session["UserId"];
                        data.UpdatedOn = DateTime.Now;
                        data.Logo = saveFilePath == "" ? null : saveFilePath;
                    }

                    var res = _service.UpdateCompanySetupPage(data);

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

        [HttpGet]
        public ActionResult ViewCompany(int companyId)
        {
            try
            {
                var data = _service.getCompanyById(companyId);

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
        public ActionResult ViewUser(int userId)
        {
            try
            {
                var data = _service.getUserByIdService(userId);

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
        public ActionResult EditUser(int userId)
        {
            try
            {
                var data = _service.getUserByIdService(userId);

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
        public JsonResult DeleteUser(int userId)
        {
            string msg = "";
            try
            {
                var data = _service.DeleteUserById(userId);
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

    }
}