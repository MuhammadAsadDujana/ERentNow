using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Communication
{
    public class Email
    {
        public static void SendEmail(string from = "", string to = "", string subject = "", string body = "")
        {
            MailMessage mail = new MailMessage(from, to);
            mail.Subject = subject;
            mail.Body = body;
            SmtpClient client = new SmtpClient();
            client.Send(mail);
        }
    }
}
