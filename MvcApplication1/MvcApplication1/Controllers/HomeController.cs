using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            try
            {
                SendEmail("bcsf13m030@gmail.com", 5000);
            }
            catch (Exception ex)
            {
                ViewBag.messg = ex.Message;
            }
            return View();
        }

        private void SendEmail(string email, int code)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("bitf12m060@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Verification Code";
                mail.Body = "Your verification code is <b>" + code + "</b>";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("bitf12m060", "asp.net60");

                SmtpServer.EnableSsl = true;
                 SmtpServer.Send(mail);
               


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
