using Core.Common.Cryptography;
using Core.Common.Helper;
using Core.Common.Miscellaneous;
using Core.Dto;
using Core.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.UserService
{
    public partial class UserService : IUserService
    {
        eRentEntities _db;

        public UserService()
        {
            _db = new eRentEntities();
        }

        //Insert, Update, Delete, IsActive, ForgetPassword procedure
        public virtual List<UserModel> ManuplateUser(UserModel userList = null)
        {
            try
            {
                //  eRentEntities context = new eRentEntities();
                var result = _db.Database.SqlQuery<UserModel>("Exec sp_CUD_User @Id, @UserRole ,@FirstName,@LastName ,@DOB ,@JobTitle ,@CompanyName ,@Address ,@StateId ,@City ,@ZipCode ,@CompanyAccountInformation ,@MobileNumber ,@EmailAddress ,@Password ,@UpdatedBy ,@IsActive ,@IsDeleted ,@PasswordRecoveryCode",
                    new SqlParameter("Id", userList.Id),
                    new SqlParameter("UserRole", userList.Role),
                    new SqlParameter("FirstName", userList.FirstName != null ? userList.FirstName : ""),
                    new SqlParameter("LastName", userList.LastName != null ? userList.LastName : ""),
                    new SqlParameter("DOB", userList.DOB),
                    new SqlParameter ("JobTitle", userList.JobTitle != null ? userList.JobTitle : ""),
                    new SqlParameter("CompanyName", userList.CompanyName != null ? userList.CompanyName : ""),
                    new SqlParameter("Address", userList.Address != null ? userList.Address : ""),
                    new SqlParameter("StateId", userList.StateID),
                    new SqlParameter("City", userList.City != null ? userList.City : ""),
                    new SqlParameter("ZipCode", userList.ZipCode != null ? userList.ZipCode : ""),
                    new SqlParameter("CompanyAccountInformation", userList.CompanyAccountInformation != null ? userList.CompanyAccountInformation : ""),
                    new SqlParameter("MobileNumber", userList.MobileNumber != null ? userList.MobileNumber : ""),
                    new SqlParameter("EmailAddress", userList.EmailAddress != null ? userList.EmailAddress : ""),
                    new SqlParameter("Password", userList.Password != null ? Encrypt_Dycrypt.Encrypt(userList.Password) : ""),
                    new SqlParameter("UpdatedBy", userList.UpdatedBy),
                    new SqlParameter("IsActive", userList.IsActive),
                    new SqlParameter("IsDeleted", userList.IsDeleted),
                    new SqlParameter("PasswordRecoveryCode", userList.PasswordRecoveryCode != null ? userList.PasswordRecoveryCode : "")).ToList();



                return result;
            }
            catch (Exception ex)
            {
                userList.Message = ex.ToString();
                return null;
            }
        }
        public virtual List<UserModel> GetUserInfo(UserModel userList = null)
        {
            try
            {
                var result = _db.Database.SqlQuery<UserModel>("Exec sp_Get_UserInfo @FullName, @EmailAddress ,@RoleId ,@IsActive ,@IsDelete ,@Password ,@IsLogin ,@PasswordRecoveryCode,@Id ,@IsPasswordRecovery",
                    new SqlParameter("FullName", userList.FullName == null ? "" : userList.FullName),
                    new SqlParameter("EmailAddress", userList.EmailAddress == null ? "" : userList.EmailAddress),
                    new SqlParameter("RoleId", userList.RoleId),
                    new SqlParameter("IsActive", userList.IsActive),
                    new SqlParameter("IsDelete", userList.IsDeleted),
                    new SqlParameter("Password", userList.Password == null ? "" : Encrypt_Dycrypt.Encrypt(userList.Password)),
                    new SqlParameter("IsLogin", userList.IsLogin),
                    new SqlParameter("PasswordRecoveryCode", userList.PasswordRecoveryCode == null ? "" : userList.PasswordRecoveryCode),
                    new SqlParameter("Id", userList.Id),
                    new SqlParameter("IsPasswordRecovery", userList.IsPasswordRecovery)).ToList();
                //new SqlParameter("UserRole", userList.UserRole.Role == null ? "" : userList.UserRole.Role),
                //new SqlParameter("IsUserRole", userList.IsUserRole)).ToList();

                return result;
            }
            catch (Exception ex)
            {
                userList.Message = ex.ToString();
                return null;
            }
        }

        public virtual UserModel UpdateUserPasswordRecovery(UserModel userList = null)
        {
            try
            {
                string newPassword = Encrypt_Dycrypt.Encrypt(userList.NewPassword);
                var result = _db.Database.SqlQuery<UserModel>("Exec sp_Update @Id ,@PasswordRecoveryCode,@NewPassword ",
                    new SqlParameter("Id", userList.Id),
                    new SqlParameter("PasswordRecoveryCode", userList.PasswordRecoveryCode == null ? "" : userList.PasswordRecoveryCode),
                    new SqlParameter("NewPassword", newPassword)).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                userList.Message = ex.ToString();
                return null;
            }
        }

        public virtual UserModel VerifyEmailService(UserModel userList = null)
        {
            try
            {
            
                var result = _db.Database.SqlQuery<UserModel>("Exec sp_VerifyEmail @Id ,@PasswordRecoveryCode ",
                    new SqlParameter("Id", userList.Id),
                    new SqlParameter("PasswordRecoveryCode", userList.PasswordRecoveryCode == null ? "" : userList.PasswordRecoveryCode)).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                userList.Message = ex.ToString();
                return null;
            }
        }

        public User VerifyEmailadminService(string EmailAddress)
        {
            var user = _db.Users.Where(x => x.IsActive == true && x.EmailAddress == EmailAddress).FirstOrDefault();
            if (user != null)
                return user;
            else
                return null;
        }

        public User RecoverPasswordService(string id)
        {
            User user = _db.Users.Where(x => x.SecuredKey == id).FirstOrDefault();
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public User UpdateSecuredKeyService(User user)
        {
            try
            {
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<UserHistoryDto> GetUserLoginHistory()
        {
            try
            {

                var List = _db.Database.SqlQuery<UserHistoryDto>("sp_UserLoginHistory").ToList();
                if (List != null)
                    return List;
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public UserLoginHistory InsertUserLoginHistory(UserLoginHistory entity)
        {
            if (entity == null)
                return null;

            _db.UserLoginHistories.Add(entity);
            _db.SaveChanges();
            //_db.Entry(entity).State = EntityState.Added;
            //_db.SaveChangesAsync();
            return entity;
        }

        public UserLoginHistory GetUserLoginHistory(int Id)
        {
            if (Id == null)
                return null;

           var res = _db.UserLoginHistories.Where(x => x.Id == Id).FirstOrDefault();
           return res;
        }

        public UserLoginHistory UpdateUserLoginHistory(UserLoginHistory entity)
        {
            if (entity == null)
                return null; 

            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChangesAsync();
            return entity;
        }

        public CompanySetupPage InsertCompanySetupPage(CompanySetupPage entity)
        {
            if (entity == null)
                return null;

            _db.CompanySetupPages.Add(entity);
            _db.SaveChanges();
            //_db.Entry(entity).State = EntityState.Added;
            //_db.SaveChangesAsync();
            return entity;
        }

        public IEnumerable<CompanySetupPage> GetCompaniesProfileList()
        {
            try
            {

                var List = _db.CompanySetupPages.OrderByDescending(x => x.CreatedBy).Where(x => x.IsActive == true).ToList();
                if (List != null)
                    return List;
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<User> GetUserProfileList()
        {
            try
            {

                var List = _db.Users.OrderByDescending(x => x.CreatedOn).Where(x => x.IsActive == true).ToList();
                if (List != null)
                    return List;
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public CompanySetupPage DeleteCompanyById(int Id)
        {
            var company = _db.CompanySetupPages.Where(x => x.Id == Id).FirstOrDefault();
            if (company != null)
            {
                company.IsActive = false;
                company.IsDeleted = true;
                _db.Entry(company).State = EntityState.Modified;
                _db.SaveChanges();
            }

            return company;
        }

        public CompanySetupPage getCompanyById(int Id)
        {
            var company =  _db.CompanySetupPages.Where(x => x.Id.Equals(Id)).FirstOrDefault();
            return company;
        }

        public CompanySetupPage UpdateCompanySetupPage(CompanySetupPage entity)
        {
            if (entity == null)
                return null;

            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChangesAsync();
            return entity;
        }

        public User getUserByIdService(int Id)
        {
            var user = _db.Users.Where(x => x.Id.Equals(Id) && x.IsActive == true).FirstOrDefault();
            return user;
        }

        public virtual UserModel GenerateUserToken(UserModel userModel)
        {
            try
            {
                var user = getUserByIdService(userModel.Id);

                user.AccessToken = TokkenManager.GenerateToken(user.Id.ToString());
                user.TokenIssueDate = DateTime.Now;
                user.TokenExpiryDate = DateTime.Now.AddDays(7);
                user.FCMDeviceToken = userModel.FCMDeviceToken;

                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();

                userModel.AccessToken = user.AccessToken;
                userModel.FCMDeviceToken = user.FCMDeviceToken;
                

                if (userModel != null)
                    return userModel;
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ValidateModel LoginValidate(User model)
        {
            List<string> data = new List<string>();
            data.Add(Validations.RequiredValidator(model.Password, "Password"));
            data.Add(Validations.EmailValidator("Email Address", model.EmailAddress));
            return Validations.ValidateFields(data);
        }

        public ValidateModel SignupValidate(User model)
        {
            List<string> data = new List<string>();
            data.Add(Validations.RequiredValidator(model.FirstName, "First Name"));
            data.Add(Validations.EmailValidator("Email", model.EmailAddress));
            data.Add(Validations.RequiredValidator("Password", model.Password));
            data.Add(Validations.RequiredValidator("Mobile Number", model.MobileNumber));
            return Validations.ValidateFields(data);
        }


        public ValidateModel ForgotPasswordValidation(string Email)
        {
            List<string> data = new List<string>();
            data.Add(Validations.EmailValidator("Email",Email));
            return Validations.ValidateFields(data);
        }

        public string SignUpService(UserModel userModel)
        {
            try
            {
                var checkUserEmail = _db.Users.Where(x => x.IsActive == true && x.EmailAddress == userModel.EmailAddress.ToLower()).FirstOrDefault();
                if (checkUserEmail != null)
                    return ConstantMessages.EmailExist;

                User user = new User
                {
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    DOB = userModel.DOB,
                    JobTitle = userModel.JobTitle,
                    CompanyName = userModel.CompanyName,
                    Address = userModel.Address,
                    StateID = userModel.StateID,
                    City = userModel.City,
                    ZipCode = userModel.ZipCode,
                    MobileNumber = userModel.MobileNumber,
                    EmailAddress = userModel.EmailAddress.ToLower(),
                    Password = Encrypt_Dycrypt.Encrypt(userModel.Password),
                    IsActive = false,
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    RoleId = 3,
                    FCMDeviceToken = userModel.FCMDeviceToken,
                    PasswordRecoveryCode = userModel.PasswordRecoveryCode  //for email verification
                };

                _db.Users.Add(user);
                _db.SaveChanges();

                if (user.Id > 0)
                    return ConstantMessages.RegistrationSuccess;
                else
                    return ConstantMessages.RegistrationFailed;
            }
            catch (Exception ex)
            {
                return ConstantMessages.SomethingWentWrong + ex.Message;
            }
        }

        public string SignUpAdminService(UserModel userModel)
        {
            try
            {
                var checkUserEmail = _db.Users.Where(x => x.IsActive == true && x.EmailAddress == userModel.EmailAddress.ToLower()).FirstOrDefault();
                if (checkUserEmail != null)
                    return ConstantMessages.EmailExist;

                User user = new User
                {
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    DOB = userModel.DOB,
                    JobTitle = userModel.JobTitle,
                    CompanyName = userModel.CompanyName,
                    Address = userModel.Address,
                    StateID = userModel.StateID,
                    City = userModel.City,
                    ZipCode = userModel.ZipCode,
                    MobileNumber = userModel.MobileNumber,
                    EmailAddress = userModel.EmailAddress.ToLower(),
                    Password = Encrypt_Dycrypt.Encrypt(userModel.NewPassword),
                    IsActive = true,
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    RoleId = userModel.RoleId,
                    FCMDeviceToken = null,
                    Image = userModel.Image == "" ? null : userModel.Image
                    //  PasswordRecoveryCode = userModel.PasswordRecoveryCode  //for email verification
                };

                _db.Users.Add(user);
                _db.SaveChanges();

                if (user.Id > 0)
                    return ConstantMessages.RegistrationSuccess;
                else
                    return ConstantMessages.RegistrationFailed;
            }
            catch (Exception ex)
            {
                return ConstantMessages.SomethingWentWrong + ex.Message;
            }
        }

        public User UpdatUserProfileService(User entity)
        {
            if (entity == null)
                return null;

            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChanges();
            return entity;
        }

        public string LogoutService(int UserId)
        {
            try
            {
                var user = _db.Users.Where(x => x.Id.Equals(UserId) && x.IsActive == true).FirstOrDefault();
                if (user != null)
                {
                    user.AccessToken = null;
                    user.TokenExpiryDate = user.TokenIssueDate;

                    _db.Entry(user).State = EntityState.Modified;
                    _db.SaveChanges();

                    return ConstantMessages.Logout;
                }
                else
                {
                    return ConstantMessages.Failed;
                }
            }
            catch (Exception ex)
            {
                return ConstantMessages.SomethingWentWrong + ex.Message;
            }
        }

        public ResponseModel ChangePasswordService(ChangePassViewModel model)
        {
            try
            {
              
                var user = getUserByIdService(model.UserId);
                if (user == null || user.Password != Encrypt_Dycrypt.Encrypt(model.OldPassword))
                {
                    return new ResponseModel { isSuccess = false, isError = true , msg = "Current password is not correct." };
                }

                if (model.NewPassword != model.ConfirmPassword)
                {
                    return new ResponseModel { isSuccess = false, isError = true, msg = "Password and confirm password do not match." };
                }

                if (model.NewPassword == model.OldPassword)
                {
                    return new ResponseModel { isSuccess = false, isError = true, msg = "New password should be different." };
                }

                user.Password = Encrypt_Dycrypt.Encrypt(model.NewPassword);
                UpdatUserProfileService(user);

                return new ResponseModel { isSuccess = true, isError = false, msg = "Password has been Changed" };
            }
            catch (Exception ex)
            {
                return new ResponseModel { isSuccess = false, isError = true, msg = ConstantMessages.SomethingWentWrong + ex.Message };
            }
        }

        public string GetUserByNameService(string email)
        {
            try
            {
                string role = _db.Users.Where(x => x.EmailAddress == email && x.IsActive == true).FirstOrDefault().UserRole.Role.ToString();

                if (role != null)
                    return role;

                return null;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return null;
            }
        }

        public IEnumerable<UserRole> GetUserRoleList()
        {
            try
            {

                var List = _db.UserRoles.ToList();
                if (List != null)
                    return List;
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<State> GetStateList()
        {
            try
            {

                var List = _db.States.ToList();
                if (List != null)
                    return List;
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public User DeleteUserById(int Id)
        {
            var user = _db.Users.Where(x => x.Id == Id).FirstOrDefault();
            if (user != null)
            {
                user.IsActive = false;
                user.IsDeleted = true;
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
            }

            return user;
        }
    }
}
