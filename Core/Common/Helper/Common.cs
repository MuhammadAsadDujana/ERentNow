using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Common.Helper
{
    public class Common
    {
        public static string IsValidEmail(string emailaddress) 
        {
            try
            {
                if (string.IsNullOrEmpty(emailaddress))
                    return ConstantMessages.EmailRequired;

                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(emailaddress);
                if (match.Success)
                {
                    return ConstantMessages.Success;
                }
                else
                {
                    return ConstantMessages.Failed;
                }
            }
            catch (FormatException ex)
            {
                return ConstantMessages.SomethingWentWrong + ex.Message;
            }
        }

        public static ResponseModel CheckPasswordValid(string password, string confirmPassword)
        {
            try
            {
                
                char[] special = { '@', '#', '$', '%', '^', '&', '+', '=' };
                if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
                {
                    return new ResponseModel { isSuccess = false, isError = true, msg = "Please fill Password." };
                }
                else if (string.IsNullOrEmpty(confirmPassword) || string.IsNullOrWhiteSpace(confirmPassword))
                {
                    return new ResponseModel { isSuccess = false, isError = true, msg = "Please fill Confirm Password." };
                }
                else if (!password.Contains(confirmPassword))
                {
      
                    return new ResponseModel { isSuccess = false, isError = true, msg = "Password and Confirm password do not match." };
                }
                else if (password.Length < 9)
                {
                    return new ResponseModel { isSuccess = false, isError = true, msg = "Password should be at least 8 characters long and should include numbers, letters and special characters" };
            
                }
                else if (password.IndexOfAny(special) == -1)
                {
                    return new ResponseModel { isSuccess = false, isError = true, msg = "Password should be at least 8 characters long and should include numbers, letters and special characters" };
                }
                else
                {
                    return new ResponseModel { isSuccess = true, isError = false, msg = ConstantMessages.Success };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel { isSuccess = false, isError = true, msg = ConstantMessages.SomethingWentWrong + ex.Message };        
            }
        }

    }
}
