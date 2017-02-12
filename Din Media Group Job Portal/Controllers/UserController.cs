using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Din_Media_Group_Job_Portal.Models;

namespace Din_Media_Group_Job_Portal.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/





        public ActionResult Home()
        {
            if (Session["type"] == null)
            {
                return Redirect(Url.Content("~/User/MyAccount"));
            }
            return View("BrowseJob");
        }
       
        //jobseeker.userName="mohsin";
       
        public ActionResult Index()
        {
            
           return View();
        }
        [HttpPost]
         public ActionResult MyAccount(string userName, string password)
        {
            if(userName=="mohsin")
            {
               
                Session["user"] = userName;
                Session["type"] = "jobseeker";
               return Redirect(Url.Content("~/User/Home"));
            }
            else if (userName == "dinnews")
            {

                Session["user"] = userName;
                Session["type"] = "employer";
                return Redirect(Url.Content("~/Employer"));
            }
            ViewBag.ErrorLogin = "Kindly Check Username or Password";
            return View("MyAccount");

           
        }
        public ActionResult UserProfile()
        {
            return View("UserProfile");
        }
        public ActionResult BrowseJob()
        {
            return View();
        }
        public ActionResult JobDetails()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        }
        public ActionResult myAccount()
        {
            return View();
        }
        public ActionResult AddResume()
        {
            return View();
        }
        public ActionResult ManageResume()
        {
            return View();
        }

        public ActionResult BuyPremiumServices()
        {
            return View("Premium");
        }
        public ActionResult AllCatagories()
        {
            return View("Catagories");
        }
        
       public ActionResult LogOut()
        {
            Session.Abandon();
            return Redirect(Url.Content("~/User/Index"));
        }

    }
}
