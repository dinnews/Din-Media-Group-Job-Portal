using Din_Media_Group_Job_Portal.DBService;
using Din_Media_Group_Job_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Din_Media_Group_Job_Portal.Controllers
{
    public class EmployerController : Controller
    {
        DinJobPortalEntities db = new DinJobPortalEntities();
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
        public ActionResult ValidateEmployerSignUp(tb_user user, tb_employer_registration_data employer, string password_reenter) //*, string password_reenter, string mobileNo, string cnicNo*?/
        {
            #region Employer Registration Validation
            
           user.user_type = "employer";
           /*  var tempMobile = mobileNo.Split('-');
            employer.mobile = decimal.Parse(tempMobile[0] + tempMobile[1]);
            var tempCnic = cnicNo.Split('-');
            employer.cnic = decimal.Parse(tempCnic[0] + tempCnic[1] + tempCnic[2]);*/
            List<tb_employer_registration_data> emp = new List<tb_employer_registration_data>();
            emp.Add(employer);
            user.tb_employer_registration_data = emp;
            if (db.tb_user.Any(usr => usr.email == user.email))
            {
                ModelState.AddModelError("email", "This email already exists");
                return View("MyAccount", user);
            }
            if (!string.IsNullOrEmpty(user.password))
            {
                if (user.password != password_reenter)
                {
                    ModelState.AddModelError("password", "Kindly Enter Same Password in both fields");
                }
               // if (user.password.Length < 6 || user.password.Length > 32)
                //{
                  //  ModelState.AddModelError("password", "Kindly Enter Password between 6 and 32 sharacters");
                //}

            }
            #region Commented
            /*
            else
            {
                ModelState.AddModelError("password", "Password is required");
            }

            if (string.IsNullOrEmpty(user.tb_employer_registration_data.First().first_name))
            {
                ModelState.AddModelError("first_name", "First Name is required");
            }
            if (string.IsNullOrEmpty(user.tb_employer_registration_data.FirstOrDefault().last_name))
            {
                ModelState.AddModelError("last_name", "Last Name is required");
            }
            if (string.IsNullOrEmpty(user.tb_employer_registration_data.FirstOrDefault().company_name))
            {
                ModelState.AddModelError("company_name", "Company name is required");
            }
            if (user.tb_employer_registration_data.FirstOrDefault().mobile.ToString().Length<9)
            {
                ModelState.AddModelError("mobile", "Mobile is required and should be Valid");
            }
            if (user.tb_employer_registration_data.FirstOrDefault().cnic<12)
            {
                ModelState.AddModelError("cnic", "CNIC is required");
            }
            
            if (!string.IsNullOrEmpty(user.email))
            {
                string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(emailRegex);
                if (!re.IsMatch(user.email))
                {
                    ModelState.AddModelError("email", "Email is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("email", "Email is required");
            }*/
            #endregion
            if (ModelState.IsValid)
            {
                try
                {
                    PasswordEncryption enc = new PasswordEncryption();
                    string encPassword = enc.encryptPassword(user.password);
                    user.password = encPassword;
                    db.tb_user.Add(user);
                    db.SaveChanges();
                    int id = user.id;
                    return View("MyAccount");
                }
                catch (Exception e)
                {
                    ViewBag.Exception = e.Message;
                    return View("DbError");
                }
            }
            return View("MyAccount", user);
            #endregion
            //return View("MyAccount");
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
        public ActionResult EditProfile()
        {
            return View();
        }
        public ActionResult ViewDepartment()
        {
            return View();
        }
        public ActionResult DepartmentDetails()
        {
            return View();
        }
    }
}
