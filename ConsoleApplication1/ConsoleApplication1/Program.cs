using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.SendEmail("bcsf13m030@gmail.com",5000);
            Console.ReadKey();
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
                mail.Body = "Your verification code is <b>"+code+"</b>";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("bitf12m060", "asp.net60");
                

                SmtpServer.Send(mail);
                Console.WriteLine("Message sent");
               
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
