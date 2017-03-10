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
       
        public ActionResult EmployerHome()
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
        public ActionResult MyAccount()
        {
            return View();
        }
        public ActionResult AccountSetting()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult CompleteProfile()
        {
            return View();
        }
        public ActionResult ViewDepartment()
        {
            return View();
        }
    }
}
