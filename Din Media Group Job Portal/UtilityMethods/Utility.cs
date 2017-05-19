using Din_Media_Group_Job_Portal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Din_Media_Group_Job_Portal.UtilityMethods
{
    public class Utility
    {
        DinJobPortalEntities db = new DinJobPortalEntities();
        public void SaveException_for_ExceptionLog(Exception e)
        {
            try
            {
            //ViewBag.Exception = e.Message;
            tb_exception ex = new tb_exception();
            ex.date = System.DateTime.Now;
            ex.exception_message = e.Message;
            ex.exception_stack_trace = e.StackTrace;
            db.tb_exception.Add(ex);;
            db.SaveChanges();
            }
            catch(Exception)
            {
                //throw;
            }
        }
        public bool SendVerificationEmail(string email, decimal radomNo)
        {
           

                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("dinnewsgroup@gmail.com");
                    mail.To.Add(email);
                    mail.Subject = "Verification Code";
                    mail.Body = "Your verification code is <b>" + radomNo + "</b>";

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("dinnewsgroup@gmail.com", "dinnews123");

                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);

                    return true;

                }
                catch (Exception ex)
                {
                    throw ex;
                    return false;
                }

                return true;

            }
           
            //return false;
        }
        
    }
