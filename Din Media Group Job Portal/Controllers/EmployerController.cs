using Din_Media_Group_Job_Portal.DBService;
using Din_Media_Group_Job_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using Din_Media_Group_Job_Portal.SODailymotionUpload;

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
        [HttpPost]
        public ActionResult SaveJob(tb_jobs job)
        {
            tb_user session_user;
            try
            {
                session_user = (tb_user)Session["user"];
                if (session_user != null)
                {
                    if (session_user.is_active == false || session_user.is_verified == false)
                    {
                        ViewBag.ErrorMessage = "You have not verified your account yet Kindly CHeck your email";
                        return RedirectToAction("VerifyEmail", "User");
                    }
                    else
                    {
                        ModelState.Clear();
                        job.job_location = Request.Form["job_location"];
                        job.job_catagory = Request.Form["job_catagory"];
                        job.required_skill = Request.Form["required_skill"];
                       string dept_id = Request.Form["deapartment_id"];
                      // job.deapartment_id = int.Parse(dept_id);
                        TryValidateModel(job);
                        
                        if(ModelState.IsValid)
                        {
                            job.user_id = session_user.id;
                            db.tb_jobs.Add(job);
                            db.SaveChanges();
                            return RedirectToAction("ManageJobs");
                        }
                        else
                        {
                            session_user.tb_jobs.Clear();
                            session_user.tb_jobs.Add(job);
                            return View("PostJob",session_user);
                        }
                    }
                }
                else
                {
                    return RedirectToAction("MyAccount", "User");
                }
            }
            catch (Exception e)
            {
                objUtility = new UtilityMethods.Utility();
                objUtility.SaveException_for_ExceptionLog(e);
                ViewBag.Exception = e.Message;
                return RedirectToAction("DbError", "User", e);
            }
            return RedirectToAction("MyAccount", "User");
        }
        public ActionResult PostJob()
        {
            tb_user session_user;
            tb_profile_employer profile;
            
            try{

            session_user = (tb_user)Session["user"];
                if (session_user != null)
                {
                    if (session_user.is_active == false || session_user.is_verified == false)
                    {
                        ViewBag.ErrorMessage = "You have not verified your account yet Kindly CHeck your email";
                        return RedirectToAction("VerifyEmail", "User");
                    }
                    else
                    {
                        profile = db.tb_profile_employer.Where(pro => pro.email == session_user.email)
                            .Include(pro=>pro.tb_department_employer)
                            .FirstOrDefault();
                        if (profile == null)
                        {
                            return RedirectToAction("CompleteProfile");
                        }
                        if (profile.tb_department_employer.Count < 1)
                        {
                            return RedirectToAction("ViewDepartment");
                        }
                        else
                        {
                            session_user.tb_profile_employer.Clear();
                            session_user.tb_profile_employer.Add(profile);
                            return View(session_user);
                        }
                    }
                }
                else
                {
                    return RedirectToAction("MyAccount", "User");
                }
            }
            catch (Exception e)
            {
                objUtility = new UtilityMethods.Utility();
                objUtility.SaveException_for_ExceptionLog(e);
                ViewBag.Exception = e.Message;
                return RedirectToAction("DbError", "User", e);
            }
            return RedirectToAction("MyAccount", "User");
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
                        var previous_email = existing_user_data.email;
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
                            if (previous_email != existing_user_data.email)
                            {
                                Random rnd = new Random();
                                decimal randomNo = rnd.Next(10000000, 99999999);
                                objUtility = new UtilityMethods.Utility();
                                bool isEmailSent = objUtility.SendVerificationEmail(existing_user_data.email, randomNo);
                                if (isEmailSent)
                                {
                                    Session["user_email"] = existing_user_data.email;
                                    Session["verification_code"] = randomNo;
                                    return RedirectToAction("VerifyEmail", "User");
                                }
                            }
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
            catch (Exception)
            {

            }
            return View();
        }
        public ActionResult Dashboard()
        {
            tb_user session_user;
            tb_profile_employer profile;
            try
            {
                session_user = (tb_user)Session["user"];
                if (session_user != null)
                {
                    if (session_user.is_active == false || session_user.is_verified == false)
                    {
                        ViewBag.ErrorMessage = "You have not verified your account yet Kindly CHeck your email";
                    }
                    else
                    {
                        profile = db.tb_profile_employer.Where(pro => pro.email == session_user.email)
                            .Include(pro => pro.tb_department_employer)
                            .FirstOrDefault();
                        if (profile == null)
                        {
                            return RedirectToAction("CompleteProfile");
                        }
                        else
                        {
                            session_user.tb_profile_employer.Add(profile);
                            return View(session_user);
                        }
                    }
                }
                else
                {
                    return RedirectToAction("MyAccount", "User");
                }
            }
            catch (Exception e)
            {
                objUtility = new UtilityMethods.Utility();
                objUtility.SaveException_for_ExceptionLog(e);
                ViewBag.Exception = e.Message;
                return RedirectToAction("DbError", "User", e);
            }
            return RedirectToAction("MyAccount", "User");
        }
        public ActionResult CompleteProfile()
        {
            tb_user session_user;
            tb_profile_employer profile;
            try
            {
                session_user = (tb_user)Session["user"];
                if (session_user != null)
                {
                    if (session_user.is_active == false || session_user.is_verified == false)
                    {
                        ViewBag.ErrorMessage = "You have not verified your account yet Kindly CHeck your email";
                    }
                    else
                    {
                        profile = db.tb_profile_employer.Where(pro => pro.email == session_user.email && pro.user_id == session_user.id).FirstOrDefault();
                        if (profile != null)
                        {
                            return RedirectToAction("EditProfile");
                        }
                        else
                        {
                            return View();
                        }
                    }
                }
                else
                {
                    return RedirectToAction("MyAccount", "User");
                }
            }
            catch (Exception e)
            {
                objUtility = new UtilityMethods.Utility();
                objUtility.SaveException_for_ExceptionLog(e);
                ViewBag.Exception = e.Message;
                return RedirectToAction("DbError", "User", e);
            }
            return View();
        }
        #region Create_Profile_For_First_TIME_Employer
        [HttpPost]
        public ActionResult CreateProfileFirstTime(tb_profile_employer employer_profile,HttpPostedFileBase profile_picture, HttpPostedFileBase cover_picture)
        {
            tb_user existing_user_data;
            try
            {

                ModelState.Clear();
                existing_user_data = (tb_user)Session["user"];
                if (existing_user_data != null)
                {
                    employer_profile.email = existing_user_data.email;
                    if (profile_picture != null && profile_picture.ContentLength > 0)
                        try
                        {
                            string new_profile_name = existing_user_data.email + Path.GetExtension(profile_picture.FileName);

                            string path = Path.Combine(Server.MapPath("~/Employer_Profile_images"), new_profile_name);
                            profile_picture.SaveAs(path);
                            employer_profile.profile_picture = new_profile_name;
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        }
                    else
                    {
                        ViewBag.Message = "Please select file";
                    }
                    if (cover_picture != null && cover_picture.ContentLength > 0)
                        try
                        {
                            string new_cover_name = existing_user_data.email + Path.GetExtension(cover_picture.FileName);
                            string path = Path.Combine(Server.MapPath("~/Employer_Cover_images"), new_cover_name);
                            cover_picture.SaveAs(path);
                            employer_profile.cover_picture = new_cover_name;
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        }
                    else
                    {
                        ViewBag.Message = "Please select file";
                    }
                    
                }
                objUtility = new UtilityMethods.Utility();
                employer_profile.user_id = existing_user_data.id;
                List<tb_department_employer> employer_department_list = objUtility.GetDepartmentList(existing_user_data.id, Request["department_name"], Request["no_of_employee_in_department"], Request["department_intro"]);

                employer_profile.tb_department_employer = employer_department_list;
                TryValidateModel(employer_profile);
                if (ModelState.IsValid)
                {
                    try
                    {
                        db.tb_profile_employer.Add(employer_profile);
                        db.SaveChanges();
                        int id = employer_profile.id;
                        return RedirectToAction("Dashboard");

                    }
                    catch (Exception e)
                    {
                        objUtility = new UtilityMethods.Utility();
                        objUtility.SaveException_for_ExceptionLog(e);
                        ViewBag.Exception = e.Message;
                        return RedirectToAction("DbError", "User", e);
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
            return View("CompleteProfile");
        }
        #endregion
        public ActionResult EditProfile()
        {
            tb_user session_user;
            tb_profile_employer profile;
            try
            {
                session_user = (tb_user)Session["user"];
                if (session_user != null)
                {
                    if (session_user.is_active == false || session_user.is_verified == false)
                    {
                        ViewBag.ErrorMessage = "You have not verified your account yet Kindly CHeck your email";
                    }
                    else
                    {
                        profile = db.tb_profile_employer.Where(pro => pro.email == session_user.email)
                            .Include(pro=>pro.tb_department_employer)
                            .FirstOrDefault();
                        if (profile == null)
                        {
                            return RedirectToAction("CompleteProfile");
                        }
                        else
                        {
                            session_user.tb_profile_employer.Clear();
                            session_user.tb_profile_employer.Add(profile);
                            return View(session_user);
                        }
                    }
                }
                else
                {
                    return RedirectToAction("MyAccount", "User");
                }
            }
            catch (Exception e)
            {
                objUtility = new UtilityMethods.Utility();
                objUtility.SaveException_for_ExceptionLog(e);
                ViewBag.Exception = e.Message;
                return RedirectToAction("DbError", "User", e);
            }
            return RedirectToAction("MyAccount", "User");
        }
        #region Update_profile_Employer
        [HttpPost]
        public ActionResult UpdateProfile(tb_profile_employer employer)
        {
            try
            {
                
                db = new DinJobPortalEntities();
                tb_user session_user;
                tb_profile_employer profile;
                session_user = (tb_user)Session["user"];
                if (session_user != null)
                {
                    if (string.IsNullOrEmpty(employer.summary))
                    {
                        ViewBag.Summary = "Kindly write Summary of your company";
                        return View("EditProfile", session_user);
                    }
                    if (employer.no_of_employees == null)
                    {
                        ViewBag.Employees = "Provide No of Employees";
                        return View("EditProfile", session_user);
                    }
                    if (employer.company_establishment_date == null)
                    {
                        ViewBag.Date = "Provide Company EStablishment Date";
                        return View("EditProfile", session_user);
                    }
                    if (employer.company_location == null)
                    {
                        ViewBag.Location = "Provide Company Location";
                        return View("EditProfile", session_user);
                    }
                    employer.summary = Regex.Replace(employer.summary, @"\r\n?|\n", "<br />");
                    profile = db.tb_profile_employer.Where(pro => pro.email == session_user.email).FirstOrDefault();
                    //profile = session_user.tb_profile_employee;
                    profile.summary = employer.summary;
                    profile.company_location = employer.company_location;
                    profile.no_of_employees = employer.no_of_employees;
                    profile.company_establishment_date = employer.company_establishment_date;
                    db.Entry(profile).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    Session["user"] = session_user;
                    return RedirectToAction("EditProfile");

                }
                else
                {
                    return RedirectToAction("MyAccount", "User");
                }
            }
            catch (Exception e)
            {
                objUtility = new UtilityMethods.Utility();
                objUtility.SaveException_for_ExceptionLog(e);
                ViewBag.Exception = e.Message;
                return RedirectToAction("DbError", "User", e);
            }
            //return View();
        }
        #endregion
        #region UpdateCoverEmployer
        [HttpPost]
        public ActionResult UpdateCover(HttpPostedFileBase cover_picture)
        {
            try
            {
                db = new DinJobPortalEntities();
                tb_profile_employer profile;
                tb_user session_user;
                session_user = (tb_user)Session["user"];
                if (cover_picture != null)
                {
                    if (session_user != null)
                    {

                        profile = db.tb_profile_employer.Where(pro => pro.user_id == session_user.id).FirstOrDefault();
                        if (profile != null)
                        {

                            string new_cover_name = session_user.email + Path.GetExtension(cover_picture.FileName);

                            string path = Path.Combine(Server.MapPath("~/Employer_Cover_images"), new_cover_name);
                            cover_picture.SaveAs(path);
                            profile.cover_picture = new_cover_name;
                            db.Entry(profile).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            tb_profile_employer pr = db.tb_profile_employer.Where(pro => pro.email == session_user.email)
                                             .Include(pro => pro.tb_department_employer)
                                             .FirstOrDefault();
                            session_user.tb_profile_employer.Clear();
                            session_user.tb_profile_employer.Add(pr);
                            Session["user"] = session_user;
                            return RedirectToAction("EditProfile");

                        }

                    }
                    else
                    {
                        return RedirectToAction("MyAccount", "User");
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
            return RedirectToAction("EditProfile");
        }
        #endregion
        #region UpdateProfilePictureEmployee
        [HttpPost]
        public ActionResult UpdateProfilePicture(HttpPostedFileBase profile_picture)
        {
            try
            {
                db = new DinJobPortalEntities();
                tb_profile_employer profile;
                tb_user session_user;
                session_user = (tb_user)Session["user"];
                if (profile_picture != null)
                {
                    if (session_user != null)
                    {

                        profile = db.tb_profile_employer.Where(pro => pro.user_id == session_user.id).FirstOrDefault();
                        if (profile != null)
                        {

                            string new_profile_name = session_user.email + Path.GetExtension(profile_picture.FileName);

                            string path = Path.Combine(Server.MapPath("~/Employer_Profile_images"), new_profile_name);
                            profile_picture.SaveAs(path);
                            profile.profile_picture = new_profile_name;
                            db.Entry(profile).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            tb_profile_employer pr = db.tb_profile_employer.Where(pro => pro.email == session_user.email)
                                             .Include(pro => pro.tb_department_employer)
                                             .FirstOrDefault();
                            session_user.tb_profile_employer.Clear();
                            session_user.tb_profile_employer.Add(pr);
                            Session["user"] = session_user;
                            return RedirectToAction("DashBoard");

                        }

                    }
                    else
                    {
                        return RedirectToAction("MyAccount", "User");
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
            return RedirectToAction("Dashboard");
        }
        #endregion
        #region UpdateVideoEmployee
        /*[HttpPost]
        public ActionResult UpdateVideo(HttpPostedFileBase profile_video)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            try
            {
                db = new DinJobPortalEntities();
                tb_profile_employer profile;
                tb_user session_user;
                session_user = (tb_user)Session["user"];
                if (profile_video != null)
                {
                    if (session_user != null)
                    {

                        profile = db.tb_profile_employer.Where(pro => pro.user_id == session_user.id).FirstOrDefault();
                        if (profile != null)
                        {

                            MainUploader up = new MainUploader();
                            profile.video_id = up.startUplaod(profile_video);

                            db.Entry(profile).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            tb_profile_employer pr = db.tb_profile_employer.Where(pro => pro.email == session_user.email)
                                             .Include(pro => pro.tb_department_employer)
                                             .FirstOrDefault();
                            session_user.tb_profile_employer.Clear();
                            session_user.tb_profile_employer.Add(pr);
                            Session["user"] = session_user;
                            return RedirectToAction("DashBoard");

                        }

                    }
                    else
                    {
                        return RedirectToAction("MyAccount", "User");
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
            return RedirectToAction("EditProfile");
        }*/
        #endregion

        public ActionResult ViewDepartment()
        {
            tb_user session_user;
            tb_profile_employer profile;
            try
            {
                session_user = (tb_user)Session["user"];
                if (session_user != null)
                {
                    if (session_user.is_active == false || session_user.is_verified == false)
                    {
                        ViewBag.ErrorMessage = "You have not verified your account yet Kindly CHeck your email";
                    }
                    else
                    {
                        profile = db.tb_profile_employer.Where(pro => pro.email == session_user.email)
                            .Include(pro => pro.tb_department_employer)
                            .FirstOrDefault();
                        if (profile == null)
                        {
                            return RedirectToAction("CompleteProfile");
                        }
                        else
                        {
                            session_user.tb_profile_employer.Clear();
                            session_user.tb_profile_employer.Add(profile);
                            return View(session_user);
                        }
                    }
                }
                else
                {
                    return RedirectToAction("MyAccount", "User");
                }
            }
            catch (Exception e)
            {
                objUtility = new UtilityMethods.Utility();
                objUtility.SaveException_for_ExceptionLog(e);
                ViewBag.Exception = e.Message;
                return RedirectToAction("DbError", "User", e);
            }
            return RedirectToAction("MyAccount", "User");
        }
        public ActionResult DepartmentDetails()
        {
            return View();
        }
    }
}
