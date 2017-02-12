using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Din_Media_Group_Job_Portal.Controllers
{
    public class EmployerController : Controller
    {
        //
        // GET: /Employer/
        public ActionResult Home()
        {
            if (Session["type"] == null )
            {
                return Redirect(Url.Content("~/User/MyAccount"));
            }
            return Redirect(Url.Content("~/Employer/Index"));

        }
        public ActionResult Index()
        {
            return View("EmployerHome");
        }
        public ActionResult PostJob()
        {
            return View();
        }
        public ActionResult ManageJobs()
        {
            return View();
        }
        public ActionResult ManageApplications()
        {
            return View();
        }
        public ActionResult BrowseResume()
        {
            return View();
        }
       

    }
}
