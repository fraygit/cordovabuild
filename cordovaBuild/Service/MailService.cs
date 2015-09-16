using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace cordovaBuild.Service
{
    public class MailService
    {
        public void Send(string to, string templatePath, string username)
        {
            MailMessage mail = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "email-smtp.us-east-1.amazonaws.com";

            //mail.To.Add(new MailAddress(to));
            mail.To.Add(new MailAddress("mark@outloud.digital"));
            mail.To.Add(new MailAddress("cordovaBuildnz@gmail.com"));
            mail.From = new MailAddress("francis.yanga@gmail.com");
            mail.Subject = "Welcome to cordovaBuild";
            mail.IsBodyHtml = true;
            var param = new string[] { username };
            mail.Body = GetMessage(param, templatePath);
            client.UseDefaultCredentials = true;
            client.Credentials = new NetworkCredential("AKIAIROLINP45YCSEZXQ", "Agz+P17KklLc9e8RlH3/dWWyZjp5StsYzOOeYQoZZsG/");
            var userToken = "something";
            client.Send(mail);
        }

        private string GetMessage(string[] parameters, string templatePath)
        {
            if (File.Exists(templatePath))
            {
                var template = File.ReadAllText(templatePath);
                for (var i=0; i < parameters.Length; i++){
                    template = template.Replace("{" + i + "}", parameters[i]);
                }
                return template;
            }
            return "";
        }
    }
}
