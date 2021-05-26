using Core.Common.Communication;
using Core.Common.Cryptography;
using Core.Common.Helper;
using Core.Common.Miscellaneous;
using Core.Model;
using Core.Services.UserService;
using ERentApi.Attributes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ERentApi.Controllers
{
    public class UserController : BaseController
    {
        #region Fields

        private readonly UserService _IUserService = new UserService();

        #endregion

        //#region Contractor

        //public UserController()
        //{
        //    _IUserService = new UserService();
        //}

        //#endregion

        #region SignUp

        [HttpPost]
        public HttpResponseMessage SignUp(UserModel model)
        {
            
            model.Role = "Customer";
            model.Message = "";
            model.PasswordRecoveryCode = RandomNumber.RandomString(6, false);
            HttpResponseMessage result = null;

            var EmailAdressCheck = Common.IsValidEmail(model.EmailAddress);
            if (!EmailAdressCheck.Equals("Success"))
                return result = Request.CreateResponse(HttpStatusCode.BadRequest, EmailAdressCheck);

            var singUpUser = _IUserService.SignUpService(model);
            if (singUpUser == ConstantMessages.RegistrationSuccess)
            {
                string senderEmail = ConfigurationManager.AppSettings["SenderEmail"].ToString();

                string body = "We have received a request to verify your account.";
                body += Environment.NewLine;
                body += "Kindly enter provided below 6 digits code to verify your account";
                body += Environment.NewLine;
                body += "Code: " + model.PasswordRecoveryCode;

                Email.SendEmail(from: senderEmail, to: model.EmailAddress, subject: "Account verify request", body: body);

                result = Request.CreateResponse(HttpStatusCode.OK, "Email has been send to " + model.EmailAddress + ", " + singUpUser);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest, singUpUser);
            }

            return result;
        }

        [HttpPost]
        public HttpResponseMessage VerifyEmail(UserModel model)
        {
            HttpResponseMessage result = null;
            model.IsActive = true;
            model.IsPasswordRecovery = true;

            if (model.PasswordRecoveryCode == "" || model.EmailAddress == "")
                return result = Request.CreateResponse(HttpStatusCode.BadRequest, "Fields are empty");

            var checkEmail = _IUserService.GetUserInfo(model);
            if (checkEmail.Count > 0)
            {
                model.Id = checkEmail.FirstOrDefault().Id;
                model.PasswordRecoveryCode = "";

                var verifiyEmail = _IUserService.VerifyEmailService(model);
                if (verifiyEmail.Message == "Success")
                {
                    result = Request.CreateResponse(HttpStatusCode.OK, "Email has been verified successfully");
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, "Verifing Email request has been failed");
                }
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid 6 digits code Please try again");
            }

            return result;
        }

        [HttpPost]
        public HttpResponseMessage Login(UserModel model)
        {
            model.Role = "Customer";
            model.Message = "";
            model.IsLogin = true;
            string _FCMDeviceToken = model.FCMDeviceToken;
            HttpResponseMessage result = null;

            var loginUser = _IUserService.GetUserInfo(model);
            if (loginUser.Count > 0)
            {
                //Token Generation work done by Asad Dated: 4/2/2021
                var UserModel = loginUser.FirstOrDefault();
                UserModel.FCMDeviceToken = _FCMDeviceToken;
                var UserTokkenGenerated = _IUserService.GenerateUserToken(UserModel);
                result = Request.CreateResponse(HttpStatusCode.OK, UserTokkenGenerated);
                //Token Generation work done by Asad, ended here Dated: 4/2/2021
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest, ConstantMessages.EnterValidEmailOrPass);
            }

            return result;
        }

        [HttpPost]
        public HttpResponseMessage ForgetPassword(UserModel model)
        {
            HttpResponseMessage result = null;
            model.IsActive = true;

            var user = _IUserService.GetUserInfo(model);
            if (user.Count > 0)
            {
                model.Id = user.FirstOrDefault().Id;
                model.Role = user.FirstOrDefault().Role;
                model.PasswordRecoveryCode = RandomNumber.RandomString(6, false);
                model.DOB = user.FirstOrDefault().DOB;
                model.StateID = user.FirstOrDefault().StateID;

                var UpdateUsers = _IUserService.ManuplateUser(model);
                if (UpdateUsers != null)
                {
                    string senderEmail = ConfigurationManager.AppSettings["SenderEmail"].ToString();

                    string body = "We have received a request to create user a new password.Kindly login with the";
                    body += Environment.NewLine;
                    body += "temporary password provided below and then proceed to reset your password.";
                    body += Environment.NewLine;
                    body += "Password: " + model.PasswordRecoveryCode;

                    Email.SendEmail(from: senderEmail, to: model.EmailAddress, subject: "Password Reset", body: body);
                    result = Request.CreateResponse(HttpStatusCode.OK, "Email has been send to " + model.EmailAddress);
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, "Please try again later");
                }
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest, "Email not exist");
            }

            return result;
        }

        [HttpPost]
        public HttpResponseMessage ChangeForgetPassword(UserModel model)
        {
            HttpResponseMessage result = null;
            model.IsActive = true;
            model.IsPasswordRecovery = true;
          
            if(model.PasswordRecoveryCode == "" || model.EmailAddress == "" || model.NewPassword == "")
                return result = Request.CreateResponse(HttpStatusCode.BadRequest, "Fields are empty");

            var checkEmail = _IUserService.GetUserInfo(model);
            if (checkEmail.Count > 0)
            {
                model.Id = checkEmail.FirstOrDefault().Id;
                model.PasswordRecoveryCode = "";
                
                var updateUserPasswordRecovery = _IUserService.UpdateUserPasswordRecovery(model);
                if (updateUserPasswordRecovery.Message == "Success")
                {
                    result = Request.CreateResponse(HttpStatusCode.OK, "Password has been changed successfully");
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, "Password changing request has been failed");
                }
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest, "Wrong temporary password Please try again");
            }

            return result;
        }

        [HttpPost]
        [UserAuthorization]
        public HttpResponseMessage EditUser(UserModel model)
        {
            HttpResponseMessage result = null;
            int userId = UserId;
            var user = _IUserService.getUserByIdService(userId);
            if(user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.DOB = model.DOB;
                user.JobTitle = model.JobTitle;
                user.EmailAddress = model.EmailAddress;
                user.MobileNumber = model.MobileNumber;
                user.CompanyName = model.CompanyName;
                user.Address = model.Address;
                user.City = model.City;
                user.StateID = model.StateID;
                user.ZipCode = model.ZipCode;
                user.CompanyAccountInformation = model.CompanyAccountInformation;
                user.UpdatedBy = userId;
                user.UpdatedOn = DateTime.Now;

                var data = _IUserService.UpdatUserProfileService(user);
                model.FirstName = data.FirstName;
                model.LastName = data.LastName;
                model.DOB = data.DOB;
                model.JobTitle = data.JobTitle;
                model.EmailAddress = data.EmailAddress;
                model.MobileNumber = data.MobileNumber;
                model.CompanyName = data.CompanyName;
                model.Address = data.Address;
                model.City = data.City;
                model.StateID = data.StateID;
                model.ZipCode = data.ZipCode;
                model.CompanyAccountInformation = data.CompanyAccountInformation;
                model.UpdatedBy = data.UpdatedBy;
                model.UpdatedOn = data.UpdatedOn;
                model.Message = "User updated sucessfully";

                if (data != null)
                    result = Request.CreateResponse(HttpStatusCode.OK, model);
                else
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, "User not updated");
            }
            else
                result = Request.CreateResponse(HttpStatusCode.BadRequest, "No user found");

            return result;
        }

        [HttpGet]
        [UserAuthorization]
        public HttpResponseMessage Logout()
        {
            HttpResponseMessage result = null;
            int userId = UserId;
            try
            {
                var Result = _IUserService.LogoutService(UserId);
                if(Result == ConstantMessages.Logout)
                    result = Request.CreateResponse(HttpStatusCode.OK, "User logout sucessfully");
                else
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, "Something went wrong");
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            return result;
        }

        [HttpGet]
        [UserAuthorization]
        public HttpResponseMessage ViewProfile()
        {
            HttpResponseMessage result = null;
            int userId = UserId;
            try
            {
                var Result = _IUserService.getUserByIdService(UserId);
                if (Result != null)
                {
                    UserModel user = new UserModel();
                    user.FirstName = Result.FirstName;
                    user.LastName = Result.LastName;
                    user.DOB = Result.DOB;
                    user.JobTitle = Result.JobTitle;
                    user.EmailAddress = Result.EmailAddress;
                    user.MobileNumber = Result.MobileNumber;
                    user.CompanyName = Result.CompanyName;
                    user.Address = Result.Address;
                    user.City = Result.City;
                    user.StateID = Result.StateID;
                    user.ZipCode = Result.ZipCode;
                    user.CompanyAccountInformation = Result.CompanyAccountInformation;
                    result = Request.CreateResponse(HttpStatusCode.OK, user);

                }
                else
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, "Something went wrong");
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            return result;
        }

        [HttpGet]
        public HttpResponseMessage DecryptPassword([FromUri] string Value)
        {
            HttpResponseMessage result = null;
            var pass = Encrypt_Dycrypt.Decrypt(Value);
            result = Request.CreateResponse(HttpStatusCode.OK, pass);

            return result;
        }

        #endregion
    }
}
