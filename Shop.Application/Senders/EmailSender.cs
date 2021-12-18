using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Senders
{
    public static class EmailSender
    {
        public static void Send(string email, string subject, string template)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();


            message.From = new MailAddress("mohammadbookshoptest@gmail.com", "bookshop");

            message.To.Add(email);
            message.Body = template;
            message.Subject = subject;

            message.IsBodyHtml = true;

            smtpClient.Port = 587;
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Credentials = new NetworkCredential("mohammadbookshoptest@gmail.com", "mohammad1998");
            smtpClient.EnableSsl = true;
           

            smtpClient.Send(message);

        }
    }
}
