using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Dto
{
    public class UserDto
    {
        public HttpPostedFileBase imageUpload { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int MyProperty { get; set; }

    }
}
