using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class UserHistoryDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string JobTitle { get; set; }
        public string Role { get; set; }
        public DateTime LoginTime { get; set; }
        public bool Offline { get; set; }

    }
}
