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
       private PasswordEncryption encryption = new PasswordEncryption();
        private UtilityMethods.Utility objUtility = new UtilityMethods.Utility();
        List<tb_master_masters> master_masters_data;
        #endregion
        
        public ActionResult Home()
        {
            return View();
        }

        #region Signup, Validation and verification
        [HttpPost]
        public ActionResult ValidateEmployeeSignUp(tb_user user, tb_employee_registration_data employee, string password_reenter, string fulltime, string parttime, string internship, string govt)
        {
            #region Employee Registration Validation
            try
            {
                string interests = fulltime + "," + parttime + "," + internship + "," + govt;

                //harcoded job_interest///////
                /// didn't get any idea to vlaidate
                employee.job_interest = interests;
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
                #region commented
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
                        
                        string encPassword = encryption.encryptPassword(user.password);
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
                            return RedirectToAction("VerifyEmail");
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
        }
        
        #endregion

        [HttpPost]
         public ActionResult MyAccount(string email, string password, string rememberme)
        {
            
            string encPassword = encryption.encryptPassword(password);
            tb_user user_from_db_to_verify;
            try
            {
                user_from_db_to_verify = db.tb_user.Where(user => (user.email == email && user.password == encPassword)).FirstOrDefault<tb_user>();
               
                if (user_from_db_to_verify!=null)
                {
                    if (user_from_db_to_verify.is_active == true && user_from_db_to_verify.is_verified == true)
                    {
                        user_from_db_to_verify.last_login = System.DateTime.Now;
                        db.Entry(user_from_db_to_verify).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        if (user_from_db_to_verify.user_type == "employee")
                        {
                            user_from_db_to_verify.tb_employee_registration_data.Add(db.tb_employee_registration_data.Where(employee => employee.user_id == user_from_db_to_verify.id).FirstOrDefault<tb_employee_registration_data>());
                        }
                        else if (user_from_db_to_verify.user_type == "employer")
                        {
                            user_from_db_to_verify.tb_employer_registration_data.Add(db.tb_employer_registration_data.Where(employer => employer.user_id == user_from_db_to_verify.id).FirstOrDefault<tb_employer_registration_data>());
                        }
                        if (rememberme != null)
                        {
                            ///cookies
                        }
                        Session["user"] = user_from_db_to_verify;
                        Session["type"] = user_from_db_to_verify.user_type;
                        return RedirectToAction("Home", "User");
                    }
                    else if (user_from_db_to_verify.is_verified == false) 
                    {
                        if (Session["verification_code"]==null)
                        {
                            Random rnd = new Random();
                            decimal randomNo = rnd.Next(10000000, 99999999);
                            objUtility = new UtilityMethods.Utility();
                            bool isEmailSent = objUtility.SendVerificationEmail(email, randomNo);
                            if (isEmailSent)
                            {
                                Session["user_email"] = email;
                                Session["verification_code"] = randomNo;
                                //return RedirectToAction("VerifyEmail");
                            }
                        }
                        
                    }
                    ViewBag.SuccessMessage = "You have not verified your account. Check your email and verify";
                    return View("VerifyEmail");
                    
                }
                ViewBag.ErrorLogin = "Kindly Check Username or Password";
                return View("MyAccount");

            }
            catch(Exception e)
            {
                objUtility = new UtilityMethods.Utility();
                objUtility.SaveException_for_ExceptionLog(e);
                ViewBag.Exception = e.Message;
                return View("DbError");
                
            }
            //return View("MyAccount");
           
        }
        public ActionResult ValidateVerificationCode(string verification_code)
        {
            tb_user get_user_to_update ;
            try
            {
                decimal verification_code_in_session=decimal.Parse(Session["verification_code"].ToString());
                decimal verification_code_by_user = decimal.Parse(verification_code);
                if (verification_code_by_user == verification_code_in_session)
                {
                    string email = Session["user_email"].ToString();
                    get_user_to_update= db.tb_user.Where(user => user.email == email).FirstOrDefault<tb_user>();
                    if(get_user_to_update!=null)
                    {
                        try
                        {
                            get_user_to_update.is_active = true;
                            get_user_to_update.is_verified = true;
                            db.Entry(get_user_to_update).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            ViewBag.SuccesMessage = "Verified Successfully Login to your Account.";
                            return View("MyAccount");
                        }
                        catch(Exception e)
                        {
                            objUtility = new UtilityMethods.Utility();
                            objUtility.SaveException_for_ExceptionLog(e);
                            ViewBag.Exception = e.Message;
                            return View("DbError",e);
                        }
                        
                    }
                }
                ViewBag.ErrorMessage = "You entered invalid code";

            }
            catch(Exception e)
            {
                ViewBag.ErrorMessage = "You entered invalid code";
            }
            return View("VerifyEmail");
        }
        
        public ActionResult VerifyEmail()
        {
            return View();
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
           ViewBag.Exception = e.Message;
           return View();
       }
       public ActionResult GeneralError()
       {
           return View();
       }
    }
}
