using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Din_Media_Group_Job_Portal.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult AdminDashboard()
        {
            return View();
        }
        public ActionResult ViewEmployers()
        {
            return View();
        }
        public ActionResult ViewJobSeekers()
        {
            return View();
        }
        public ActionResult ViewComplaints()
        {
            return View();
        }
        public ActionResult ReadComplain()
        {
            return View();
        }


    }
}
