using Din_Media_Group_Job_Portal.DBService;
using Din_Media_Group_Job_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Din_Media_Group_Job_Portal.Controllers
{
    public class EmployerController : Controller
    {
        DinJobPortalEntities db = new DinJobPortalEntities();
        #region All_Class_Level_Variables
        private UtilityMethods.Utility objUtility ;
        private PasswordEncryption enc = new PasswordEncryption();
        #endregion
        
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
           try{
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
                        Random rnd = new Random();
                        decimal randomNo = rnd.Next(10000000, 99999999);
                        objUtility = new UtilityMethods.Utility();
                        bool isEmailSent = objUtility.SendVerificationEmail(user.email, randomNo);
                        if(isEmailSent)
                        {
                            Session["user_email"] = user.email;
                            Session["verification_code"] = randomNo;
                            return RedirectToAction("VerifyEmail","User");
                        }
                
                    }
                    catch (Exception e)
                    {
                        objUtility = new UtilityMethods.Utility();
                        objUtility.SaveException_for_ExceptionLog(e);
                        ViewBag.Exception = e.Message;
                        return View("DbError");
                    }
                }
                return View("MyAccount", user);
            }
            catch (Exception e)
            {
                
                //tb_exception ex = new tb_exception();
                //ex.date = System.DateTime.Now;
                //ex.exception_message = e.Message;
                //ex.exception_stack_trace = e.StackTrace;
                //db.tb_exception.Add(ex);
                objUtility = new UtilityMethods.Utility();
                objUtility.SaveException_for_ExceptionLog(e);
                ViewBag.Exception = e.Message;
                return View("DbError");
            }
            

            #endregion
            //return View("MyAccount");
        }
        #region UpdateAccountSettings for Employer
        [HttpPost]
        public ActionResult UpdateAccountSettings(tb_employer_registration_data changed_employer_data)
        {
            db = new DinJobPortalEntities();
            
            tb_user existing_user_data;
            try
            {
                //tb_user user;
                existing_user_data = (tb_user)Session["user"];
                if (existing_user_data != null)
                {
                    if (existing_user_data.user_type == "employer")
                    {
                        existing_user_data = db.tb_user.Where(tempuser => tempuser.id == existing_user_data.id).Include(tempuser => tempuser.tb_employer_registration_data).FirstOrDefault<tb_user>();
                        existing_user_data.email = changed_employer_data.email;
                        existing_user_data.tb_employer_registration_data.FirstOrDefault().email = changed_employer_data.email;
                        existing_user_data.tb_employer_registration_data.FirstOrDefault().first_name = changed_employer_data.first_name;
                        existing_user_data.tb_employer_registration_data.FirstOrDefault().last_name = changed_employer_data.last_name;
                        existing_user_data.tb_employer_registration_data.FirstOrDefault().company_name = changed_employer_data.company_name;
                        existing_user_data.tb_employer_registration_data.FirstOrDefault().mobile = changed_employer_data.mobile;
                        existing_user_data.tb_employer_registration_data.FirstOrDefault().cnic = changed_employer_data.cnic;
                        if (ModelState.IsValid)
                        {
                            db.Entry(existing_user_data).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            Session["user"] = existing_user_data;
                            return RedirectToAction("AccountSetting");
                        }
                        else
                        {
                            return View("AccountSetting", existing_user_data);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                objUtility = new UtilityMethods.Utility();
                objUtility.SaveException_for_ExceptionLog(e);
                ViewBag.Exception = e.Message;
                return RedirectToAction("DbError", "User", e);
            }
            return View("AccountSetting");
        }
        #endregion
        
        [HttpPost]
        public ActionResult UpdatePassword(string new_password, string old_password, string new_password_reenter)
        {
            db = new DinJobPortalEntities();

             tb_user existing_user_data;
            try
            {
                //tb_user user;
                existing_user_data = (tb_user)Session["user"];
                if (existing_user_data != null)
                {
                    if (existing_user_data.user_type == "employer")
                    {
                        ModelState.Clear();
                        existing_user_data = db.tb_user.Where(tempuser => tempuser.id == existing_user_data.id).Include(tempuser => tempuser.tb_employer_registration_data).FirstOrDefault<tb_user>();
                        TryValidateModel(existing_user_data);
                        string encPassword = enc.encryptPassword(old_password);
                        if(encPassword!=existing_user_data.password)
                        {
                            ViewBag.ErrorMessage= "Old Password does not match";
                            return View("AccountSetting", existing_user_data);
                        }
                        encPassword= enc.encryptPassword(new_password);
                        if (encPassword == existing_user_data.password)
                        {
                            ViewBag.ErrorMessage= "New and old Passwords should not be same";
                            return View("AccountSetting", existing_user_data);
                        }
                        if(new_password!=new_password_reenter)
                        {
                           ViewBag.ErrorMessage="New Password and Re enter Password should be same";
                            return View("AccountSetting", existing_user_data);
                            
                        }
                        if (new_password.Length < 6 || new_password.Length > 32)
                        {
                             ViewBag.ErrorMessage="Password Length must be atleast 6 Characters";
                            return View("AccountSetting", existing_user_data);
                        }
                        existing_user_data.password = new_password;
                        TryValidateModel(existing_user_data);
                        if (ModelState.IsValid)
                        {
                            existing_user_data.password = encPassword;
                            existing_user_data.last_change_password = old_password;
                            db.Entry(existing_user_data).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            Session["user"] = existing_user_data;
                            return RedirectToAction("AccountSetting");
                        }
                        else
                        {
                            return View("AccountSetting", existing_user_data);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                objUtility = new UtilityMethods.Utility();
                objUtility.SaveException_for_ExceptionLog(e);
                ViewBag.Exception = e.Message;
                return View("DbError", "User", e);
            }
            return View("AccountSetting");
        }
        public ActionResult AccountSetting()
        {
            tb_user session_user_data;
            try
            {
                session_user_data = (tb_user)Session["user"];
                if (session_user_data != null)
                {
                    return View(session_user_data);
                }
                else
                {
                    return RedirectToAction("MyAccount", "User");
                }
            }
            catch (Exception e)
            {

            }
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
