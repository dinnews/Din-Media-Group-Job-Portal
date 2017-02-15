using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Din_Media_Group_Job_Portal.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Home()
        {

            return View();
        }
        [HttpPost]
         public ActionResult MyAccount(string userName, string password)
        {
            try
            {
                if (userName == "mohsin")
                {
                    Session["user"] = userName;
                    Session["type"] = "jobseeker";
                    return RedirectToAction("Home", "User");
                }
                else if (userName == "dinnews")
                {
                    Session["user"] = userName;
                    Session["type"] = "employer";
                    return RedirectToAction("EmployerHome", "Employer");
                }
                ViewBag.ErrorLogin = "Kindly Check Username or Password";
                return View("MyAccount");

            }
            catch(Exception e)
            {
                return View("MyAccount");
            }
            
           
        }
        
        public ActionResult ContactUs()
        {
            return View();
        }
        public ActionResult myAccount()
        {
            return View();
        }
        public ActionResult Premium()
        {
            return View("Premium");
        }      
       public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Home", "User");
        }

    }
}
