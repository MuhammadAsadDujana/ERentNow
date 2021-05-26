using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Model
{
    public class UserModel : User
    {
        public string Role { get; set; }
        public string Message { get; set; }
        public string FullName { get; set; }
        public bool IsLogin { get; set; }
        public bool IsPasswordRecovery { get; set; }
        public bool IsUserRole { get; set; }
      //  public string FCMDeviceToken { get; set; }
        public string AccessToken { get; set; }
        public HttpPostedFileBase UserImage { get; set; }
        public string NewPassword { get; set; }
        public HttpPostedFileBase imageUpload { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
