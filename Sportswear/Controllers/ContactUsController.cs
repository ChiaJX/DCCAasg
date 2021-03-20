using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Sportswear.Controllers
{
    public class ContactUsController : Controller
    {
        public IActionResult ContactUs()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> sendFeedback(String senderName, String subject, String senderEmail, String content)
        {
            //String senderName = fc["senderName"];
            //String subject = fc["subject"];
            //String senderEmail = fc["senderEmail"];
            //String content = fc["message"];

            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;


            // setup Smtp authentication
            //System.Net.NetworkCredential credentials =
            //    new System.Net.NetworkCredential("your_account@gmail.com", "yourpassword");
            //client.UseDefaultCredentials = false;
            //client.Credentials = credentials;

            string to = "junchengkhong.com";
            string from = senderEmail;
            MailMessage message = new MailMessage(from, to);
            message.Subject = subject;
            message.Body = @content;

            message.IsBodyHtml = true;
            message.Body = string.Format("<html><head></head><body><b>Test HTML Email</b></body>");

            client.UseDefaultCredentials = false;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                    ex.ToString());
            }

            return View(); //return to the original page when the content is error
        }
    }


    
}

