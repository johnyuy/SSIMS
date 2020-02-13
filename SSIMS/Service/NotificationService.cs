using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.ModelBinding;

namespace SSIMS.Service
{
    public class NotificationService
    {
        public void SendEmail(string receiver, string subject, string message)
        {

            var senderEmail = new MailAddress("logicssims@outlook.com", "Logic University SSIMS");
            var receiverEmail = new MailAddress(receiver, "Receiver");
            var password = "ss1msadm1np@sswOrd";
            var sub = subject;
            var body = message;
            var smtp = new SmtpClient
            {
                Host = "smtp.outlook.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = subject,
                Body = body
            })
            {
                try
                {
                    smtp.Send(mess);
                    Debug.WriteLine("Email sent");
                }
                catch(SmtpException e)
                {
                     Debug.WriteLine(e.ToString());
                }
                finally
                {
                    smtp.Dispose();
                }
                
            }


        }
    }
}