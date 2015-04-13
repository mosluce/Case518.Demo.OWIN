using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Sample.Utils
{
    public class EmailSender
    {
        public static async Task Send(string to, string title, string content)
        {
            var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("mosluce@gmail.com", "h89270asd");

            await smtp.SendMailAsync("mosluce@gmail.com", to, title, content);
        }
    }
}