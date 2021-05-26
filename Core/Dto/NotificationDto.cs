using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class NotificationDto
    {
        public int UserId { get; set; }
        public string FCMToken { get; set; }
    }
}
