using Core.Common.Communication;
using Core.Common.Cryptography;
using Core.Common.Miscellaneous;
using Core.Model;
using Core.Services.UserService;
using ERentWebUI.Notif;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERentWebUI.Controllers
{
    public class Test
    {
        public string JobPlanNumber { get; set; }
        public string JobPlanName { get; set; }
        public string StepNo { get; set; }
        public string StepName { get; set; }
    }


    public class HomeController : Controller
    {
        eRentEntities DbContext;

        public HomeController()
        {
            DbContext = new eRentEntities();
        }

        public ActionResult Index(string id)
        {

        //    using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-KA12GVJ\SQLEXPRESS;Initial Catalog=rhinoware;Integrated Security=True"))
        //    {
        //        string Qry1 = @"
        //                    select jp.ID,jp.JobPlanNumber,jp.JobPlanName,jts.JobPlanId,jts.StepNo,jts.StepName from JobPlan jp
        //                    inner join JobTaskSteps jts on jp.ID = jts.JobPlanId";

        //        SqlDataAdapter sq = new SqlDataAdapter(Qry1, con);
        //        DataTable dt = new DataTable();
        //        sq.Fill(dt);
        //        int sadas = dt.Rows.Count;
        //        string sda = JsonConvert.SerializeObject(dt);
        //        var Test = JsonConvert.DeserializeObject<List<Test>>(sda);
        //        string Qry2 = @"select * from [test] ";


        //        SqlDataAdapter sq1 = new SqlDataAdapter(Qry2, con);
        //        DataTable dt1 = new DataTable();
        //        sq1.Fill(dt1);

        //        var JobTaskSteps = JsonConvert.DeserializeObject<List<Test>>(JsonConvert.SerializeObject(dt1));


        //        int count = 0;
        //        int count1 = 0;
        //        for (int i = 0; i < JobTaskSteps.Count; i++)
        //        {
        //            var Exists = Test.Where(x => x.JobPlanNumber == JobTaskSteps[i].JobPlanNumber && x.JobPlanName == JobTaskSteps[i].JobPlanName && x.StepNo == JobTaskSteps[i].StepNo && x.StepName == JobTaskSteps[i].StepName).FirstOrDefault();
        //            if (Exists == null)
        //            {
        //                count++;
        //                string sfd = "";
        //            }
        //            else
        //            {
        //                count1++;
        //            }
        //        }
        //        int sadasds = count + count1;
        //        con.Close();
        //    }

            if (!string.IsNullOrWhiteSpace(id) && Session["UserID"] == null)
            {
                User user = DbContext.Users.Where(x => x.SecuredKey == id).FirstOrDefault();
                if (user != null)
                {
                    user.SecuredKey = RandomNumber.RandomString(20, false) + DateTime.Now.ToString("mmddyyyyhhmmss");
                    user.IsActive = true;
                    DbContext.Entry(user).State = EntityState.Modified;
                    DbContext.SaveChanges();
                    CreateSession(user);
                }
            }

            List<Forklift> ListForklift = new List<Forklift>();
            ListForklift = DbContext.Forklifts.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Banner = true;
            return View(ListForklift);
        }

        public ActionResult Invoice()
        {
            return View();

        }

        public ActionResult Location()
        {

            return View();
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }

        public ActionResult TermsConditions()
        {

            return View();
        }

        public ActionResult PrivacyPolicy()
        {

            return View();
        }

        public ActionResult RecoverPassword(string id)
        {
            User user = DbContext.Users.Where(x => x.SecuredKey == id).FirstOrDefault();
            if (user != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public JsonResult Login(User input)
        {
            ResponseModel resp = new ResponseModel();
            try
            {
                resp.Validate = new UserService().LoginValidate(input);
                if (!resp.Validate.res) { resp.isSuccess = false; return Json(resp, JsonRequestBehavior.AllowGet); }
                input.Password = Encrypt_Dycrypt.Encrypt(input.Password);
                var data = DbContext.Users.Where(x => x.EmailAddress == input.EmailAddress && x.Password == input.Password && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                if (data != null)
                {
                    CreateSession(data);
                    resp.isSuccess = true;
                }
                else
                {
                    resp.isSuccess = false;
                    resp.msg = "Invalid Credentials";
                }
            }
            catch (Exception e)
            {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Signup(User input)
        {
            ResponseModel resp = new ResponseModel();
            using (var trans = DbContext.Database.BeginTransaction())
            {
                try
                {
                    resp.Validate = new UserService().SignupValidate(input);
                    if (!resp.Validate.res) { resp.isSuccess = false; return Json(resp, JsonRequestBehavior.AllowGet); }
                    bool EmailExist = DbContext.Users.Where(x => x.EmailAddress == input.EmailAddress).FirstOrDefault() == null ? false : true;
                    if (EmailExist) { resp.isSuccess = false; resp.msg = "Email Already Exists."; return Json(resp, JsonRequestBehavior.AllowGet); }
                    input.Password = Encrypt_Dycrypt.Encrypt(input.Password);
                    input.IsActive = false;
                    input.IsDeleted = false;
                    input.CreatedOn = DateTime.Now;
                    input.RoleId = 3;
                    input.SecuredKey = RandomNumber.RandomString(20, false) + DateTime.Now.ToString("MMddyyyyhhmmss");
                    DbContext.Users.Add(input) ;

                    DbContext.SaveChanges();

                    string Url = ConfigurationManager.AppSettings["BaseUrl"].ToString() + "Home/Index/" + input.SecuredKey;
                    string senderEmail = ConfigurationManager.AppSettings["SenderEmail"].ToString();
                    Email.SendEmail(senderEmail, input.EmailAddress, "Verification", Url);
                    trans.Commit();

                    resp.isSuccess = true;
                    resp.msg = "Please check your email to get verified";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    //DbContext.Dispose();
                    resp.isSuccess = false;
                    resp.isError = true;
                    resp.msg = "An Error Occured";
                }
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetStates()
        {
            ResponseModel resp = new ResponseModel();
            try
            {
                resp.data = DbContext.States.Select(x => new { id = x.id, StateName = x.StateName }).ToList();
                resp.isSuccess = true;
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
        public JsonResult GetCompanyInfo(string Url)
        {
            ResponseModel resp = new ResponseModel();
            try
            {
                resp.data = DbContext.CompanySetupPages.Where(x => x.Url == Url).Select(x => x.Logo).FirstOrDefault();

                resp.isSuccess = true;
            }
            catch (Exception ex)
            {
                resp.isSuccess = false;
                resp.isError = true;
                resp.msg = "An Error Occured";
            }
            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ForgotPassword(string email)
        {
            ResponseModel resp = new ResponseModel();
            try
            {
                resp.Validate = new UserService().ForgotPasswordValidation(email);
                if (!resp.Validate.res) { resp.isSuccess = false; return Json(resp, JsonRequestBehavior.AllowGet); }
                var user = DbContext.Users.Where(x => x.EmailAddress == email).FirstOrDefault();
                if (user == null) { resp.isSuccess = false; resp.msg = "Invalid Email"; return Json(resp, JsonRequestBehavior.AllowGet); }

                string Url = ConfigurationManager.AppSettings["BaseUrl"].ToString() + "Home/RecoverPassword/" + user.SecuredKey;
                string senderEmail = ConfigurationManager.AppSettings["SenderEmail"].ToString();
                Email.SendEmail(senderEmail, email, "Password Recovery", Url);

                resp.isSuccess = true;
                resp.msg = "Email has been sent";
            }
            catch (Exception ex)
            {
                resp.isSuccess = false;
                resp.isError = true;
                resp.msg = "An Error Occured";
            }
            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                if (Password != ConfirmPassword) { resp.isSuccess = false; resp.msg = "Password & confirm password must be matched"; return Json(resp, JsonRequestBehavior.AllowGet); }

                User user = DbContext.Users.Where(x => x.SecuredKey == SecuredKey).FirstOrDefault();
                if (user != null)
                {
                    user.Password = Encrypt_Dycrypt.Encrypt(ConfirmPassword);
                    user.SecuredKey = RandomNumber.RandomString(20, false) + DateTime.Now.ToString("MMddyyyyhhmmss");
                    DbContext.Entry(user).State = EntityState.Modified;
                    DbContext.SaveChanges();

                    CreateSession(user);
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


        public ActionResult Logout()
        {
            Session.Abandon();
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}