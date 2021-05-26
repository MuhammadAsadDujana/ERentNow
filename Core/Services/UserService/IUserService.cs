using Core.Dto;
using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.UserService
{
    public partial interface IUserService
    {
        List<UserModel> ManuplateUser(UserModel userList = null);
        List<UserModel> GetUserInfo(UserModel userList = null);
        IEnumerable<UserHistoryDto> GetUserLoginHistory();
        UserLoginHistory InsertUserLoginHistory(UserLoginHistory entity);
        UserLoginHistory GetUserLoginHistory(int Id);
        UserLoginHistory UpdateUserLoginHistory(UserLoginHistory entity);
        CompanySetupPage InsertCompanySetupPage(CompanySetupPage entity);
        CompanySetupPage DeleteCompanyById(int Id);
        CompanySetupPage getCompanyById(int Id);
        CompanySetupPage UpdateCompanySetupPage(CompanySetupPage entity);
        User getUserByIdService(int Id);
        UserModel GenerateUserToken(UserModel userModel);

    }
}
