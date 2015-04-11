using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Case518Sample150411.Utils
{
    public class Email
    {
        public static async Task SendValidationMail(string to, String content)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("mosluce@gmail.com", "h89270asd")
            };

            MailMessage mailMessage = new MailMessage {From = new MailAddress("mosluce@gmail.com")};
            mailMessage.To.Add(to);
            mailMessage.Subject = "Validation Mail";
            mailMessage.Body = content;

            await client.SendMailAsync(mailMessage);
        }
    }
}