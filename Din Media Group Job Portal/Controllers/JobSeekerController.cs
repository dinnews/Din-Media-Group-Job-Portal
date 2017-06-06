using Din_Media_Group_Job_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Din_Media_Group_Job_Portal.DBService;
using System.IO;
using Din_Media_Group_Job_Portal.SODailymotionUpload;
using System.Security.Cryptography;
namespace Din_Media_Group_Job_Portal.Controllers
{
    public class JobSeekerController : Controller
    {
        DinJobPortalEntities db = new DinJobPortalEntities();
        private UtilityMethods.Utility objUtility = new UtilityMethods.Utility();
       List<tb_master_masters> master_masters_data;
       private PasswordEncryption enc = new PasswordEncryption();
       
        //
       // GET: /JobSeeker/
       #region Update_Accoun_Settings_Employee
        [HttpPost]
       public ActionResult UpdateAccountSettings(tb_employee_registration_data changed_employee_data, string fulltime, string parttime, string internship, string govt)
        {
            db = new DinJobPortalEntities();
            string interests = fulltime + "," + parttime + "," + internship + "," + govt;
            changed_employee_data.job_interest = interests;
            tb_user existing_user_data;
            try
            {
               //tb_user user;
                bool result_for_page = db.tb_master_masters.Find(8).visibility;
               existing_user_data = (tb_user)Session["user"];
               if (existing_user_data != null && result_for_page == true)
                {
                    if (existing_user_data.user_type == "employee")
                    {
                        existing_user_data = db.tb_user.Where(tempuser => tempuser.id == existing_user_data.id).Include(tempuser => tempuser.tb_employee_registration_data).FirstOrDefault<tb_user>();
                        existing_user_data.email = changed_employee_data.email;
                        existing_user_data.tb_employee_registration_data.FirstOrDefault().email = changed_employee_data.email;
                        existing_user_data.tb_employee_registration_data.FirstOrDefault().first_name = changed_employee_data.first_name;
                        existing_user_data.tb_employee_registration_data.FirstOrDefault().last_name = changed_employee_data.last_name;
                        existing_user_data.tb_employee_registration_data.FirstOrDefault().gender = changed_employee_data.gender;
                        existing_user_data.tb_employee_registration_data.FirstOrDefault().location = changed_employee_data.location;
                        existing_user_data.tb_employee_registration_data.FirstOrDefault().job_interest = changed_employee_data.job_interest;
                        existing_user_data.tb_employee_registration_data.FirstOrDefault().job_title = changed_employee_data.job_title;
                        existing_user_data.tb_employee_registration_data.FirstOrDefault().job_catagory_field = changed_employee_data.job_catagory_field;
                        if(ModelState.IsValid)
                        {
                            db.Entry(existing_user_data).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            Session["user"] = existing_user_data;
                            return RedirectToAction("AccountSetting");
                        }
                        else
                        {
                            return View("AccountSetting",existing_user_data);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                objUtility = new UtilityMethods.Utility();
                objUtility.SaveException_for_ExceptionLog(e);
                ViewBag.Exception = e.Message;
                return RedirectToAction("DbError","User", e);
            }
            return View("AccountSetting");
        }
       #endregion
       #region Update_Password_Employee
        [HttpPost]
       public ActionResult UpdatePassword(string new_password, string old_password, string new_password_reenter)
        {
            db = new DinJobPortalEntities();

            tb_user existing_user_data;
            try
            {
                string encPassword;
                //tb_user user;
                existing_user_data = (tb_user)Session["user"];
                if (existing_user_data != null)
                {
                    if (existing_user_data.user_type == "employee")
                    {
                        ModelState.Clear();
                        existing_user_data = db.tb_user.Where(tempuser => tempuser.id == existing_user_data.id).Include(tempuser => tempuser.tb_employee_registration_data).FirstOrDefault<tb_user>();
                        TryValidateModel(existing_user_data);
                        string encOldPassword = enc.encryptPassword(old_password);
                        if (encOldPassword != existing_user_data.password)
                        {
                            ViewBag.ErrorMessage = "Old Password does not match";
                            return View("AccountSetting", existing_user_data);
                        }
                        encPassword = enc.encryptPassword(new_password);
                        if (encPassword == existing_user_data.password)
                        {
                            ViewBag.ErrorMessage = "New and old Passwords should not be same";
                            return View("AccountSetting", existing_user_data);
                        }
                        if (new_password != new_password_reenter)
                        {
                            ViewBag.ErrorMessage = "New Password and Re enter Password should be same";
                            return View("AccountSetting", existing_user_data);

                        }
                        if (new_password.Length < 6 || new_password.Length > 32)
                        {
                            ViewBag.ErrorMessage = "Password Length must be atleast 6 Characters";
                            return View("AccountSetting", existing_user_data);
                        }
                        existing_user_data.password = new_password;
                        TryValidateModel(existing_user_data);
                        if (ModelState.IsValid)
                        {
                            existing_user_data.password = encPassword;
                            existing_user_data.last_change_password = encOldPassword;
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
       #endregion
       #region Create_Profile_For_First_TIME
       [HttpPost]
        public ActionResult CreateProfileFirstTime(tb_profile_employee employee_profile, tb_education_employee education, tb_experience_employee experience, tb_external_url_employee employee_urls, tb_projects_employee projects, tb_skills_employee skills, tb_languages_employee languages, HttpPostedFileBase profile_picture, HttpPostedFileBase cover_picture, HttpPostedFileBase video)
        {
           tb_user existing_user_data;
           try
            {
                ModelState.Clear();
               existing_user_data = (tb_user)Session["user"];
               if (existing_user_data != null )
               {
                   employee_profile.email = existing_user_data.email;
                   if (profile_picture != null && profile_picture.ContentLength > 0)
                       try
                       {
                           Guid guidValue = Guid.NewGuid();
                           MD5 md5 = MD5.Create();
                           Guid hashed = new Guid(md5.ComputeHash(guidValue.ToByteArray()));

                           string new_profile_name = hashed.ToString() + Path.GetExtension(profile_picture.FileName);

                           string path = Path.Combine(Server.MapPath("~/Jobseeker_Profile_images"), new_profile_name );
                           profile_picture.SaveAs(path);
                           employee_profile.profile_picture = new_profile_name;
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
                           Guid guidValue = Guid.NewGuid();
                           MD5 md5 = MD5.Create();
                           Guid hashed = new Guid(md5.ComputeHash(guidValue.ToByteArray()));

                           string new_cover_name = hashed.ToString() + Path.GetExtension(cover_picture.FileName);
                           string path = Path.Combine(Server.MapPath("~/Jobseeker_Cover_images"), new_cover_name );
                           cover_picture.SaveAs(path);
                           employee_profile.cover_picture = new_cover_name;
                       }
                       catch (Exception ex)
                       {
                           ViewBag.Message = "ERROR:" + ex.Message.ToString();
                       }
                   else
                   {
                       ViewBag.Message = "Please select file";
                   }
                   if (video != null && video.ContentLength > 0)
                       try
                       {
                           MainUploader up = new MainUploader();
                           string video_id = up.startUplaod(video);
                           employee_profile.video_id = video_id;
                           
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
                
               
                List<tb_education_employee> employee_education_list = new  List<tb_education_employee>();
                List<tb_experience_employee> employee_experience_list = new  List<tb_experience_employee>();
                List<tb_external_url_employee> employee_url_list = new  List<tb_external_url_employee>();
                List<tb_skills_employee> employee_skill_list = new  List<tb_skills_employee>();
                List<tb_projects_employee> employee_project_list = new  List<tb_projects_employee>();
                List<tb_languages_employee> employee_languages_list = new  List<tb_languages_employee>();
                employee_experience_list.Add(experience);
               employee_education_list.Add(education);
               employee_url_list.Add(employee_urls);
               employee_skill_list.Add(skills);
               employee_project_list.Add(projects);
               employee_languages_list.Add(languages);
               TryValidateModel(employee_profile);
              
                if (ModelState.IsValid)
                {
                    try
                    {
                        db.tb_profile_employee.Add(employee_profile);
                        db.SaveChanges();
                        int id = employee_profile.id;
                        return RedirectToAction("Dashboard");
                        
                    }
                    catch (Exception e)
                    {
                        objUtility = new UtilityMethods.Utility();
                        objUtility.SaveException_for_ExceptionLog(e);
                        ViewBag.Exception = e.Message;
                        return View("DbError");
                    }
                }
                
            }
            catch (Exception e)
            {
                objUtility = new UtilityMethods.Utility();
                objUtility.SaveException_for_ExceptionLog(e);
                ViewBag.Exception = e.Message;
                return View("Error");
            }
           return RedirectToAction("CreateProfile");
        }
       #endregion
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
            tb_user session_user;
            tb_profile_employee profile;
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
                        profile = db.tb_profile_employee.Where(pro => pro.email == session_user.email).FirstOrDefault();
                        if (profile == null)
                        {
                            return RedirectToAction("CreateProfile");
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
        public ActionResult BrowseJob()
        {
            return View();
        }
        public ActionResult JobDetails()
        {
            return View();
        }
       
        public ActionResult CreateProfile()
        {
            tb_user session_user;
            tb_profile_employee profile;
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
                         profile = db.tb_profile_employee.Where(pro => pro.email == session_user.email).FirstOrDefault();
                         if(profile!=null)
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
            catch(Exception e)
            {
                objUtility = new UtilityMethods.Utility();
                objUtility.SaveException_for_ExceptionLog(e);
                ViewBag.Exception = e.Message;
                return RedirectToAction("DbError", "User", e);
            }
            return View();
        }
        public ActionResult ManageResume()
        {
            return View();
        }
        
        public ActionResult AccountSetting()
        {
            bool result_for_page = db.tb_master_masters.Find(8).visibility;
            tb_user session_user_data;
            try
            {
                session_user_data = (tb_user)Session["user"];
                if (session_user_data != null && result_for_page == true)
                {
                    return View(session_user_data);
                }
                else
                {
                    return RedirectToAction("MyAccount", "User");
                }
            }
            catch(Exception e)
            {

            }
            return View();
        }
        
        public ActionResult AppliedJobs()
        {
            return View();
        }
        public ActionResult SavedJobs()
        {
            return View();
        }
        public ActionResult EditProfile()
        {
            return View();
        }
    }
}
