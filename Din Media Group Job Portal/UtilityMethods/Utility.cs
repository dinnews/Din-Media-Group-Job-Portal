using Din_Media_Group_Job_Portal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
              
                /*
             * Verify Email and then send a verifiction this verifcation code to that email..
             * if everything done successfully then return true otherwise return false
             
             */

                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            //return false;
        }
        
    }
}