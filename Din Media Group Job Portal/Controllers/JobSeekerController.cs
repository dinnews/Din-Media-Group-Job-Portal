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
using System.Text.RegularExpressions;
using Din_Media_Group_Job_Portal.UtilityMethods;
namespace Din_Media_Group_Job_Portal.Controllers
{
    public class JobSeekerController : Controller
    {
        DinJobPortalEntities db = new DinJobPortalEntities();
        private UtilityMethods.Utility objUtility = new UtilityMethods.Utility();
       List<tb_master_masters> master_masters_data;
       private PasswordEncryption enc = new PasswordEncryption();
       
        public JobSeekerController()
       {
           Master_Master.Master_fill();
       }
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
                        var previous_email = existing_user_data.email;
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
                        existing_user_data.is_verified = false;
                        if(ModelState.IsValid)
                        {
                            db.Entry(existing_user_data).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            Session["user"] = existing_user_data;
                            if(previous_email != existing_user_data.email)
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
                           string new_profile_name = existing_user_data.email + Path.GetExtension(profile_picture.FileName);

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
                           string new_cover_name = existing_user_data.email + Path.GetExtension(cover_picture.FileName);
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
                           System.Net.ServicePointManager.Expect100Continue = false;
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

               employee_profile.user_id = existing_user_data.id;
               List<tb_skills_employee> employee_skill_list = objUtility.GetSkillList(existing_user_data.id, Request["skill_name"], Request["skill_experience"]);
               List<tb_projects_employee> employee_project_list = objUtility.GetProjectList(existing_user_data.id, Request["project_title"], Request["position"], Request["project_url"], Request["project_currently_working"], Request["project_start_date"], Request["project_end_date"], Request["project_notes"]);
               List<tb_experience_employee> employee_experience_list = objUtility.GetExperienceList(existing_user_data.id, Request["job_title"], Request["company_location"], Request["experience_currently_working"], Request["experience_start_date"], Request["experience_end_date"], Request["experience_notes"], Request["company_name"]);
               List<tb_education_employee> employee_education_list = objUtility.GetEducationList(existing_user_data.id, Request["degree_title"], Request["institution_name"], Request["field_of_study"], Request["education_start_date"], Request["education_end_date"], Request["education_notes"]);
               List<tb_external_url_employee> employee_url_list = objUtility.GetUrlList(existing_user_data.id, Request["url_name"], Request["url"]);
               List<tb_languages_employee> employee_languages_list = objUtility.GetLanguageList(existing_user_data.id, Request["language_name"], Request["language_proficiency"]);

               employee_profile.tb_education_employee = employee_education_list;
               employee_profile.tb_experience_employee = employee_experience_list;
               employee_profile.tb_external_url_employee = employee_url_list;
               employee_profile.tb_languages_employee = employee_languages_list;
               employee_profile.tb_projects_employee = employee_project_list;
               employee_profile.tb_skills_employee = employee_skill_list;

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
            return View();
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
            List<tb_master_masters> master= Master_Master.master_list;
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
                        profile = db.tb_profile_employee.Where(pro => pro.email == session_user.email)
                            .Include(pro=>pro.tb_education_employee)
                            .Include(pro=>pro.tb_experience_employee)
                            .Include(pro=>pro.tb_external_url_employee)
                            .Include(pro=>pro.tb_languages_employee)
                            .Include(pro=>pro.tb_projects_employee)
                            .Include(pro=>pro.tb_skills_employee)
                            .FirstOrDefault();
                        if (profile == null)
                        {
                            return RedirectToAction("CreateProfile");
                        }
                        else
                        {
                            session_user.tb_profile_employee.Add(profile);
                            
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
        [HttpPost]
        public ActionResult UpdateSummary(string summary) 
        {
            try
            {
                if (string.IsNullOrEmpty(summary))
                {
                    return RedirectToAction("EditProfile");
                }
                summary = Regex.Replace(summary, @"\r\n?|\n", "<br />");
                db = new DinJobPortalEntities();
                tb_user session_user;
                tb_profile_employee profile;
                session_user = (tb_user)Session["user"];
                if (session_user != null)
                {
                    profile = db.tb_profile_employee.Where(pro => pro.email == session_user.email).FirstOrDefault();
                    //profile = session_user.tb_profile_employee;
                    profile.summary = summary;
                    db.Entry(profile).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    session_user.tb_profile_employee.FirstOrDefault().summary = summary;
                    Session["user"] = session_user;
                    return RedirectToAction("EditProfile");

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
            
        }
        #region UpdateEducationEmployee
        [HttpPost]
        public ActionResult UpdateEducation(tb_education_employee education, string education_request)
        {
            try
            {
                db = new DinJobPortalEntities();
                tb_education_employee existing_education;
                tb_user session_user;
                session_user = (tb_user)Session["user"];
                if (session_user != null)
                {
                    if (education_request == "update")
                    {
                        existing_education = db.tb_education_employee.Where(pro => pro.id == education.id).FirstOrDefault();
                        if (existing_education != null)
                        {
                            if (ModelState.IsValid)
                            {
                                existing_education.degree_title = education.degree_title;
                                existing_education.education_end_date = education.education_end_date;
                                existing_education.education_notes = education.education_notes;
                                existing_education.education_start_date = education.education_start_date;
                                existing_education.field_of_study = education.field_of_study;
                                existing_education.institution_name = education.institution_name;
                                db.Entry(existing_education).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges(); 

                            }

                        }

                    }
                    else if (education_request=="add")
                    {
                       education.profile_id = session_user.tb_profile_employee.FirstOrDefault().id;
                       if (ModelState.IsValid)
                       {
                           db.tb_education_employee.Add(education);
                           db.SaveChanges();
                       }
                    }
                    tb_profile_employee pr = db.tb_profile_employee.Where(pro => pro.email == session_user.email)
                                                 .Include(pro => pro.tb_education_employee)
                                                 .Include(pro => pro.tb_experience_employee)
                                                 .Include(pro => pro.tb_external_url_employee)
                                                 .Include(pro => pro.tb_languages_employee)
                                                 .Include(pro => pro.tb_projects_employee)
                                                 .Include(pro => pro.tb_skills_employee).FirstOrDefault();
                    session_user.tb_profile_employee.Clear();
                    session_user.tb_profile_employee.Add(pr);
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
            return RedirectToAction("EditProfile");
        }
        #endregion
        #region UpdateExperienceEmployee
        [HttpPost]
        public ActionResult UpdateExperience(tb_experience_employee experience, string experience_request)
        {
            try
            {
                db = new DinJobPortalEntities();
                tb_experience_employee existing_experience;
                tb_user session_user;
                session_user = (tb_user)Session["user"];
                if (session_user != null)
                {
                    if (experience_request == "update")
                    {
                        existing_experience = db.tb_experience_employee.Where(pro => pro.id == experience.id).FirstOrDefault();
                        if (existing_experience != null)
                        {
                            if (ModelState.IsValid)
                            {
                                existing_experience.job_title = experience.job_title;
                                existing_experience.experience_end_date = experience.experience_end_date;
                                existing_experience.experience_notes = experience.experience_notes;
                                existing_experience.experience_start_date = experience.experience_start_date;
                                existing_experience.experience_currently_working = experience.experience_currently_working;
                                existing_experience.company_name = experience.company_name;
                                existing_experience.company_location = experience.company_location;
                                
                                db.Entry(existing_experience).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }

                        }

                    }
                    else if (experience_request == "add")
                    {
                        experience.profile_id = session_user.tb_profile_employee.FirstOrDefault().id;
                        if (ModelState.IsValid)
                        {
                            db.tb_experience_employee.Add(experience);
                            db.SaveChanges();
                        }
                    }
                    tb_profile_employee pr = db.tb_profile_employee.Where(pro => pro.email == session_user.email)
                                               .Include(pro => pro.tb_experience_employee)
                                               .Include(pro => pro.tb_experience_employee)
                                               .Include(pro => pro.tb_external_url_employee)
                                               .Include(pro => pro.tb_languages_employee)
                                               .Include(pro => pro.tb_projects_employee)
                                               .Include(pro => pro.tb_skills_employee).FirstOrDefault();
                    session_user.tb_profile_employee.Clear();
                    session_user.tb_profile_employee.Add(pr);
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
            return RedirectToAction("EditProfile");
        }
        #endregion
        #region UpdateProjectsEmployee
        [HttpPost]
        public ActionResult UpdateProject(tb_projects_employee projects, string project_request)
        {
            try
            {
                db = new DinJobPortalEntities();
                tb_projects_employee existing_projects;
                tb_user session_user;
                session_user = (tb_user)Session["user"];
                if (session_user != null)
                {
                    if (project_request == "update")
                    {
                        existing_projects = db.tb_projects_employee.Where(pro => pro.id == projects.id).FirstOrDefault();
                        if (existing_projects != null)
                        {
                            if (ModelState.IsValid)
                            {
                                existing_projects.project_title = projects.project_title;
                                existing_projects.project_end_date = projects.project_end_date;
                                existing_projects.project_notes = projects.project_notes;
                                existing_projects.project_start_date = projects.project_start_date;
                                existing_projects.project_currently_working = projects.project_currently_working;
                                existing_projects.project_url = projects.project_url;
                                existing_projects.position = projects.position;
                                db.Entry(existing_projects).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }

                        }

                    }
                    else if (project_request == "add")
                    {
                        projects.profile_id = session_user.tb_profile_employee.FirstOrDefault().id;
                        if (ModelState.IsValid)
                        {
                            db.tb_projects_employee.Add(projects);
                            db.SaveChanges();
                        }
                    }

                    tb_profile_employee pr = db.tb_profile_employee.Where(pro => pro.email == session_user.email)
                                     .Include(pro => pro.tb_education_employee)
                                     .Include(pro => pro.tb_experience_employee)
                                     .Include(pro => pro.tb_external_url_employee)
                                     .Include(pro => pro.tb_languages_employee)
                                     .Include(pro => pro.tb_projects_employee)
                                     .Include(pro => pro.tb_skills_employee).FirstOrDefault();
                    session_user.tb_profile_employee.Clear();
                    session_user.tb_profile_employee.Add(pr);
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
            return RedirectToAction("EditProfile");
        }
        #endregion
        #region UpdateSkillsEmployee
        [HttpPost]
        public ActionResult UpdateSkill(tb_skills_employee skills, string skill_request)
        {
            try
            {
                db = new DinJobPortalEntities();
                tb_skills_employee existing_skills;
                tb_user session_user;
                session_user = (tb_user)Session["user"];
                if (session_user != null)
                {
                    if (skill_request == "update")
                    {
                        existing_skills = db.tb_skills_employee.Where(pro => pro.id == skills.id).FirstOrDefault();
                        if (existing_skills != null)
                        {
                            if (ModelState.IsValid)
                            {
                                existing_skills.skill_name = skills.skill_name;
                                existing_skills.skill_experience = skills.skill_experience;
                                db.Entry(existing_skills).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }

                        }

                    }
                    else if (skill_request == "add")
                    {
                        skills.profile_id = session_user.tb_profile_employee.FirstOrDefault().id;
                        if (ModelState.IsValid)
                        {
                            db.tb_skills_employee.Add(skills);
                            db.SaveChanges();
                        }
                    }
                    tb_profile_employee pr = db.tb_profile_employee.Where(pro => pro.email == session_user.email)
                                                .Include(pro => pro.tb_education_employee)
                                                .Include(pro => pro.tb_experience_employee)
                                                .Include(pro => pro.tb_external_url_employee)
                                                .Include(pro => pro.tb_languages_employee)
                                                .Include(pro => pro.tb_projects_employee)
                                                .Include(pro => pro.tb_skills_employee).FirstOrDefault();
                    session_user.tb_profile_employee.Clear();
                    session_user.tb_profile_employee.Add(pr);
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
            return RedirectToAction("EditProfile");
        }
        #endregion
        #region UpdateLanguagesEmployee
        [HttpPost]
        public ActionResult UpdateLanguage(tb_languages_employee languages, string language_request)
        {
            try
            {
                db = new DinJobPortalEntities();
                tb_languages_employee existing_languages;
                tb_user session_user;
                session_user = (tb_user)Session["user"];
                if (session_user != null)
                {
                    if (language_request == "update")
                    {
                        existing_languages = db.tb_languages_employee.Where(pro => pro.id == languages.id).FirstOrDefault();
                        if (existing_languages != null)
                        {
                            if (ModelState.IsValid)
                            {
                                existing_languages.language_name = languages.language_name;
                                existing_languages.language_proficiency = languages.language_proficiency;
                                db.Entry(existing_languages).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }

                        }

                    }
                    else if (language_request == "add")
                    {
                        languages.profile_id = session_user.tb_profile_employee.FirstOrDefault().id;
                        if (ModelState.IsValid)
                        {
                            db.tb_languages_employee.Add(languages);
                            db.SaveChanges();
                        }
                    }
                    tb_profile_employee pr = db.tb_profile_employee.Where(pro => pro.email == session_user.email)
                                                .Include(pro => pro.tb_education_employee)
                                                .Include(pro => pro.tb_experience_employee)
                                                .Include(pro => pro.tb_external_url_employee)
                                                .Include(pro => pro.tb_languages_employee)
                                                .Include(pro => pro.tb_projects_employee)
                                                .Include(pro => pro.tb_skills_employee).FirstOrDefault();
                    session_user.tb_profile_employee.Clear();
                    session_user.tb_profile_employee.Add(pr);
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
            return RedirectToAction("EditProfile");
        }
        #endregion
        #region UpdateCoverEmployee
        [HttpPost]
        public ActionResult UpdateCover(HttpPostedFileBase cover_picture)
        {
            try
            {
                db = new DinJobPortalEntities();
                tb_profile_employee profile;
                tb_user session_user;
                session_user = (tb_user)Session["user"];
                if (cover_picture != null)
                {
                    if (session_user != null)
                    {

                        profile = db.tb_profile_employee.Where(pro => pro.user_id == session_user.id).FirstOrDefault();
                        if (profile != null)
                        {

                            string new_cover_name = session_user.email + Path.GetExtension(cover_picture.FileName);

                            string path = Path.Combine(Server.MapPath("~/Jobseeker_Cover_images"), new_cover_name);
                            cover_picture.SaveAs(path);
                            profile.cover_picture = new_cover_name;
                            db.Entry(profile).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            tb_profile_employee pr = db.tb_profile_employee.Where(pro => pro.email == session_user.email)
                                             .Include(pro => pro.tb_education_employee)
                                             .Include(pro => pro.tb_experience_employee)
                                             .Include(pro => pro.tb_external_url_employee)
                                             .Include(pro => pro.tb_skills_employee)
                                             .Include(pro => pro.tb_languages_employee)
                                             .Include(pro => pro.tb_projects_employee).FirstOrDefault();
                            session_user.tb_profile_employee.Clear();
                            session_user.tb_profile_employee.Add(pr);
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
                tb_profile_employee profile;
                tb_user session_user;
                session_user = (tb_user)Session["user"];
                if (profile_picture != null)
                {
                    if (session_user != null)
                    {

                        profile = db.tb_profile_employee.Where(pro => pro.user_id == session_user.id).FirstOrDefault();
                        if (profile != null)
                        {

                            string new_profile_name = session_user.email + Path.GetExtension(profile_picture.FileName);

                            string path = Path.Combine(Server.MapPath("~/Jobseeker_Profile_images"), new_profile_name);
                            profile_picture.SaveAs(path);
                            profile.profile_picture = new_profile_name;
                            db.Entry(profile).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            tb_profile_employee pr = db.tb_profile_employee.Where(pro => pro.email == session_user.email)
                                             .Include(pro => pro.tb_education_employee)
                                             .Include(pro => pro.tb_experience_employee)
                                             .Include(pro => pro.tb_external_url_employee)
                                             .Include(pro => pro.tb_skills_employee)
                                             .Include(pro => pro.tb_languages_employee)
                                             .Include(pro => pro.tb_projects_employee).FirstOrDefault();
                            session_user.tb_profile_employee.Clear();
                            session_user.tb_profile_employee.Add(pr);
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
        #region UpdateVideoEmployee
        [HttpPost]
        public ActionResult UpdateVideo(HttpPostedFileBase profile_video)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            try
            {
                db = new DinJobPortalEntities();
                tb_profile_employee profile;
                tb_user session_user;
                session_user = (tb_user)Session["user"];
                if (profile_video != null)
                {
                    if (session_user != null)
                    {

                        profile = db.tb_profile_employee.Where(pro => pro.user_id == session_user.id).FirstOrDefault();
                        if (profile != null)
                        {

                            MainUploader up = new MainUploader();
                            profile.video_id = up.startUplaod(profile_video);

                            db.Entry(profile).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            tb_profile_employee pr = db.tb_profile_employee.Where(pro => pro.email == session_user.email)
                                             .Include(pro => pro.tb_education_employee)
                                             .Include(pro => pro.tb_experience_employee)
                                             .Include(pro => pro.tb_external_url_employee)
                                             .Include(pro => pro.tb_skills_employee)
                                             .Include(pro => pro.tb_languages_employee)
                                             .Include(pro => pro.tb_projects_employee).FirstOrDefault();
                            session_user.tb_profile_employee.Clear();
                            session_user.tb_profile_employee.Add(pr);
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
        public ActionResult BrowseJob()
        {
            List<tb_master_masters> master = Master_Master.master_list;
            //tb_user session_user;
            model_for_browse_jobs view_model = new model_for_browse_jobs();
            try
            {

                if (master.Find(e => e.id == 10).visibility == false)
                {
                    return RedirectToAction("Home", "User");
                }
                view_model.list_of_jobs = db.tb_jobs.Include(j => j.tb_user).Include(j=>j.tb_user.tb_profile_employer).Include(j=>j.tb_user.tb_employer_registration_data).ToList();
                
                return View(view_model);
            }
            catch (Exception e)
            {
                objUtility = new UtilityMethods.Utility();
                objUtility.SaveException_for_ExceptionLog(e);
                ViewBag.Exception = e.Message;
                return RedirectToAction("DbError", "User", e);
            }
        }
        public ActionResult JobDetails(int id)
        {
            List<tb_master_masters> master = Master_Master.master_list;
            tb_user session_user;
            tb_profile_employee profile;
            try
            {
                
                session_user = (tb_user)Session["user"];
                if(master.Find(e=>e.id==10).visibility==false)
                {
                    return RedirectToAction("Home", "User");
                }
                if (session_user != null)
                {
                    if (session_user.is_active == false || session_user.is_verified == false)
                    {
                        ViewBag.ErrorMessage = "You have not verified your account yet Kindly CHeck your email";
                        return RedirectToAction("VerifyEmail", "User");
                    }
                    else
                    {
                        // Logic for CVs////////////////
                        profile = db.tb_profile_employee.Where(pro => pro.email == session_user.email)
                            .Include(pro => pro.tb_education_employee)
                            .Include(pro => pro.tb_experience_employee)
                            .Include(pro => pro.tb_external_url_employee)
                            .Include(pro => pro.tb_languages_employee)
                            .Include(pro => pro.tb_projects_employee)
                            .Include(pro => pro.tb_skills_employee)
                            .Include(pro=>pro.tb_cvs_employee)
                            .FirstOrDefault();
                        if (profile == null)
                        {
                            return RedirectToAction("CreateProfile");
                        }
                        else if(profile.tb_cvs_employee.Count<1)
                        {
                            return RedirectToAction("ManageResume");
                        }
                        else
                        {
                            session_user.tb_profile_employee.Clear();
                            session_user.tb_profile_employee.Add(profile);
                            tb_jobs clicked_job;
                            clicked_job = db.tb_jobs.Include(j => j.tb_user).Where(j => j.id == id).Include(j => j.tb_apply_job).Include(j => j.tb_user.tb_profile_employer).Include(j => j.tb_user.tb_employer_registration_data).FirstOrDefault();
                            return View(clicked_job);
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
       public ActionResult ApplyforJob(tb_apply_job application)
        {
            List<tb_master_masters> master = Master_Master.master_list;
            tb_user session_user;
            tb_profile_employee profile;
            try
            {

                session_user = (tb_user)Session["user"];
                if (master.Find(e => e.id == 10).visibility == false)
                {
                    return RedirectToAction("Home", "User");
                }
                if (session_user != null)
                {
                    if (session_user.is_active == false || session_user.is_verified == false)
                    {
                        ViewBag.ErrorMessage = "You have not verified your account yet Kindly CHeck your email";
                        return RedirectToAction("VerifyEmail", "User");
                    }
                    else
                    {
                        // Logic for CVs////////////////
                        profile = db.tb_profile_employee.Where(pro => pro.email == session_user.email)
                            .Include(pro => pro.tb_education_employee)
                            .Include(pro => pro.tb_experience_employee)
                            .Include(pro => pro.tb_external_url_employee)
                            .Include(pro => pro.tb_languages_employee)
                            .Include(pro => pro.tb_projects_employee)
                            .Include(pro => pro.tb_skills_employee)
                            .Include(pro => pro.tb_cvs_employee)
                            .FirstOrDefault();
                        if (profile == null)
                        {
                            return RedirectToAction("CreateProfile");
                        }
                        else if (profile.tb_cvs_employee.Count < 1)
                        {
                            return RedirectToAction("ManageResume");
                        }
                        else
                        {
                            application.profile_id = profile.id;
                            application.application_time = DateTime.Now;
                            if (ModelState.IsValid)
                            {

                                db.tb_apply_job.Add(application);
                                db.SaveChanges();
                                int id = application.id;
                            }
                            session_user.tb_profile_employee.Clear();
                            session_user.tb_profile_employee.Add(profile);
                            tb_jobs clicked_job;
                            clicked_job = db.tb_jobs.Include(j => j.tb_user).Where(j => j.id == application.job_id).Include(j=>j.tb_apply_job).Include(j => j.tb_user.tb_profile_employer).Include(j => j.tb_user.tb_employer_registration_data).FirstOrDefault();
                            return View("JobDetails",clicked_job);
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
                         profile = db.tb_profile_employee.Where(pro => pro.email == session_user.email && pro.user_id==session_user.id).FirstOrDefault();
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
            List<tb_master_masters> master = Master_Master.master_list;
            tb_user session_user;
            tb_profile_employee profile;
            try
            {

                session_user = (tb_user)Session["user"];
                if (master.Find(e => e.id == 1).visibility == false)
                {
                    return RedirectToAction("Home", "User");
                }
                if (session_user != null)
                {
                    if (session_user.is_active == false || session_user.is_verified == false)
                    {
                        ViewBag.ErrorMessage = "You have not verified your account yet Kindly CHeck your email";
                        return RedirectToAction("VerifyEmail", "User");
                    }
                    else
                    {
                        // Logic for CVs////////////////
                        profile = db.tb_profile_employee.Where(pro => pro.email == session_user.email)
                            .Include(pro => pro.tb_education_employee)
                            .Include(pro => pro.tb_experience_employee)
                            .Include(pro => pro.tb_external_url_employee)
                            .Include(pro => pro.tb_languages_employee)
                            .Include(pro => pro.tb_projects_employee)
                            .Include(pro => pro.tb_skills_employee)
                            .Include(pro => pro.tb_cvs_employee)
                            .FirstOrDefault();
                        if (profile == null)
                        {
                            return RedirectToAction("CreateProfile");
                        }
                        else
                        {
                            session_user.tb_profile_employee.Clear();
                            session_user.tb_profile_employee.Add(profile);
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
        [HttpPost]
        public ActionResult UpdateResume(tb_cvs_employee cv, HttpPostedFileBase cv_file_name)
        {
            List<tb_master_masters> master = Master_Master.master_list;
            tb_user session_user;
            tb_profile_employee profile;
            try
            {

                session_user = (tb_user)Session["user"];
                if (master.Find(e => e.id == 1).visibility == false)
                {
                    return RedirectToAction("Home", "User");
                }
                if (session_user != null)
                {
                    if (session_user.is_active == false || session_user.is_verified == false)
                    {
                        ViewBag.ErrorMessage = "You have not verified your account yet Kindly CHeck your email";
                        return RedirectToAction("VerifyEmail", "User");
                    }
                    else
                    {
                        // Logic for CVs////////////////
                        profile = db.tb_profile_employee.Where(pro => pro.email == session_user.email)
                            .Include(pro => pro.tb_education_employee)
                            .Include(pro => pro.tb_experience_employee)
                            .Include(pro => pro.tb_external_url_employee)
                            .Include(pro => pro.tb_languages_employee)
                            .Include(pro => pro.tb_projects_employee)
                            .Include(pro => pro.tb_skills_employee)
                            .Include(pro => pro.tb_cvs_employee)
                            .FirstOrDefault();
                        if (profile == null)
                        {
                            return RedirectToAction("CreateProfile");
                        }
                        else
                        {
                            Guid guidValue = Guid.NewGuid();
                            MD5 md5 = MD5.Create();
                            Guid hashed = new Guid(md5.ComputeHash(guidValue.ToByteArray()));
                            string new_file_name = hashed.ToString() + Path.GetExtension(cv_file_name.FileName);
                            var fileToUpload = Path.Combine(Server.MapPath("~/Jobseeker_CV_file"), new_file_name);
                            cv_file_name.SaveAs(fileToUpload);
                            cv.cv_file_name = new_file_name;
                            cv.profile_id = profile.id;
                            cv.cv_upload_date = DateTime.Now;
                            if(ModelState.IsValid)
                            {
                                
                                db.tb_cvs_employee.Add(cv);
                                db.SaveChanges();
                                int id = cv.id;
                            }
                            session_user.tb_profile_employee.Clear();
                            session_user.tb_profile_employee.Add(profile);
                            return View("ManageResume", session_user);
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
                        profile = db.tb_profile_employee.Where(pro => pro.email == session_user.email)
                            .Include(pro => pro.tb_education_employee)
                            .Include(pro => pro.tb_experience_employee)
                            .Include(pro => pro.tb_external_url_employee)
                            .Include(pro => pro.tb_languages_employee)
                            .Include(pro => pro.tb_projects_employee)
                            .Include(pro => pro.tb_skills_employee)
                            .FirstOrDefault();
                        if (profile == null)
                        {
                            return RedirectToAction("CreateProfile");
                        }
                        else
                        {
                            session_user.tb_profile_employee.Clear();
                            session_user.tb_profile_employee.Add(profile);
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
    }
}
