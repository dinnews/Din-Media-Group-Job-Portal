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
    public class UserController : Controller
    {
        DinJobPortalEntities db = new DinJobPortalEntities();
        #region All_Class_Level_Variables
        private UtilityMethods.Utility objUtility ;
        #endregion
        //
        // GET: /User/

        public ActionResult Home()
        {
            return View();
        }
        #region Signup, Validation and verification
        [HttpPost]
        public ActionResult ValidateEmployeeSignUp(tb_user user, tb_employee_registration_data employee, string password_reenter)
        {
            #region Employer Registration Validation
            try
            {
                user.user_type = "employee";
                //harcoded job_interest///////
                /// didn't get any idea to vlaidate
                employee.job_interest = "abc";
                List<tb_employee_registration_data> emp = new List<tb_employee_registration_data>();
                emp.Add(employee);
                user.tb_employee_registration_data = emp;
                if (db.tb_user.Any(existing_User_Email => existing_User_Email.email == user.email))
                {
                    ModelState.AddModelError("email", "Email already exists");
                    return View("MyAccount", user);
                }
                if (!string.IsNullOrEmpty(user.password))
                {
                    if (user.password != password_reenter)
                    {
                       ModelState.AddModelError("password", "Kindly Enter Same Password in both fields");
                    }
                    //if (user.password.Length < 6 || user.password.Length > 32)
                    //{
                    //    ModelState.AddModelError("password", "Kindly Enter Password between 6 and 32 sharacters");
                    //}

                }
                    /*
                else
                {
                    ModelState.AddModelError("password", "Password is required");
                }

                if (string.IsNullOrEmpty(user.tb_employee_registration_data.First().first_name))
                {
                    ModelState.AddModelError("first_name", "First Name is required");
                }
                if (string.IsNullOrEmpty(user.tb_employee_registration_data.FirstOrDefault().last_name))
                {
                    ModelState.AddModelError("last_name", "Last Name is required");
                }
                if (string.IsNullOrEmpty(user.tb_employee_registration_data.FirstOrDefault().gender))
                {
                    ModelState.AddModelError("gender", "Gender is required");
                }
                if (string.IsNullOrEmpty(user.tb_employee_registration_data.FirstOrDefault().location))
                {
                    ModelState.AddModelError("location", "Location is required");
                }
                if (string.IsNullOrEmpty(user.tb_employee_registration_data.FirstOrDefault().job_title))
                {
                    ModelState.AddModelError("job_title", "Job Tilte is required");
                }
                if (string.IsNullOrEmpty(user.tb_employee_registration_data.FirstOrDefault().job_catagory_field))
                {
                    ModelState.AddModelError("job_catagory_field", "Catagory is required");
                }
                if (!string.IsNullOrEmpty(user.email))
                {
                    //string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                    //                         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                    //                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                    //Regex re = new Regex(emailRegex);
                    //if (!re.IsMatch(user.email))
                    //{
                    //    ModelState.AddModelError("email", "Email is not valid");
                    //}
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
                        return SendVerificationEmail(user.email);
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
        }
        public ActionResult SendVerificationEmail(string email)
        {
            tb_verification_code verification_obj = new tb_verification_code();
            try
            {
                Random rnd = new Random();
                verification_obj.verification_code = rnd.Next(10000000, 99999999);
                verification_obj.email = email;
                verification_obj.date = System.DateTime.Now;
                verification_obj.status = "created";

                /*
             * Verify Email and then send a verifiction this verifcation code to that email..
             * if everything done successfully then return true otherwise return false
             
             */

                db.tb_verification_code.Add(verification_obj);
                db.SaveChanges();
                int id = verification_obj.id;
               
            }
            catch (Exception e)
            {
                return Error(e);
            }
            return View("MyAccount");
        }
        #endregion
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
        public ActionResult MyAccount()
        {
            return View();
        }
        public ActionResult SignUpFailed(tb_user user)
        {
            return View("MyAccount", user);
        }
        public ActionResult ResetPassword()
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
       
       public ActionResult Error(Exception e)
       {
           tb_exception ex = new tb_exception();
           if(e!=null)
           {
               ViewBag.errorMessage = e.Message;
               ex.date = System.DateTime.Now;
               ex.exception_message = e.Message;
               ex.exception_stack_trace = e.StackTrace;
               db.tb_exception.Add(ex);
               db.SaveChanges();

               return View();
           }
           else
           {
               ViewBag.errorMessage = "We cannot show this page right now. You can go to Home";
               return View();
           }
       }
       public ActionResult NotFoundError()
       {
           return View();
       }
       public ActionResult DbError(Exception e)
       {
           return View();
       }
       public ActionResult GeneralError()
       {
           return View();
       }
    }
}
