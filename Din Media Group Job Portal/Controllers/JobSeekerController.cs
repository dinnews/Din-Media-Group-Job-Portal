using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Din_Media_Group_Job_Portal.Controllers
{
    public class JobSeekerController : Controller
    {
        //
        // GET: /JobSeeker/
       public ActionResult Catagories()
        {
            return View("Catagories");
        }
       public ActionResult Dashboard()
       {
           return View();
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
       
        public ActionResult AddResume()
        {
            return View();
        }
        public ActionResult ManageResume()
        {
            return View();
        }

    }
}
