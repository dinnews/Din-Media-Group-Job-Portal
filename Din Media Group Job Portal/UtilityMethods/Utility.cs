using Din_Media_Group_Job_Portal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Din_Media_Group_Job_Portal.UtilityMethods
{
    public class Utility
    {
        DinJobPortalEntities db = new DinJobPortalEntities();
        public void SaveException_for_ExceptionLog(Exception e)
        {
            try
            {
            //ViewBag.Exception = e.Message;
            tb_exception ex = new tb_exception();
            ex.date = System.DateTime.Now;
            ex.exception_message = e.Message;
            ex.exception_stack_trace = e.StackTrace;
            db.tb_exception.Add(ex);;
            db.SaveChanges();
            }
            catch(Exception)
            {
                //throw;
            }
        }
        public bool SendVerificationEmail(string email, decimal radomNo)
        {
           

                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("dinnewsgroup@gmail.com");
                    mail.To.Add(email);
                    mail.Subject = "Verification Code";
                    mail.Body = "Your verification code is <b>" + radomNo + "</b>";

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("dinnewsgroup@gmail.com", "dinnews123");

                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);

                    return true;

                }
                catch (Exception ex)
                {
                    throw ex;
                    return false;
                }

                return true;

            }

        public List<tb_languages_employee> GetLanguageList(int id, String languageName, String languageProficiency)
        {
            List<tb_languages_employee> list = new List<tb_languages_employee>();
            String[] name = languageName.Split(',');
            String[] profs = languageProficiency.Split(',');

            for (int i = 0; i < name.Length - 1; i++)
            {
                tb_languages_employee langObj = new tb_languages_employee();
                langObj.profile_id = id;
                langObj.language_name = name[i];
                langObj.language_proficiency = profs[i];
                list.Add(langObj);
            }
            return list;

        }
        public List<tb_projects_employee> GetProjectList(int id, String ptitle, String position, String pUrl, String currProject, String startDate, String endDate, String notes)
        {
            List<tb_projects_employee> list = new List<tb_projects_employee>();

            String[] titles = ptitle.Split(',');
            String[] positions = position.Split(',');
            String[] urls = pUrl.Split(',');
            // String[] projs = currProject.ToString().Split(',');
            //    String[] sDates= startDate.ToString().Split(',');
            //   String[] eDates= endDate.ToString().Split(',');
            String[] pNotes = notes.Split(',');


            for (int i = 0; i < titles.Length - 1; i++)
            {
                tb_projects_employee projectObj = new tb_projects_employee();
                projectObj.profile_id = id;
                projectObj.position = positions[i];
                projectObj.project_title = titles[i];
                projectObj.project_url = urls[i];
                projectObj.project_currently_working = false;
                projectObj.project_start_date = null;
                projectObj.project_end_date = null;
                projectObj.project_notes = pNotes[i];
                list.Add(projectObj);


            }
            return list;

        }
        public List<tb_external_url_employee> GetUrlList(int id, String urlName, String url)
        {
            List<tb_external_url_employee> list = new List<tb_external_url_employee>();
            String[] uNames = urlName.Split(',');
            String[] urls = url.Split(',');

            for (int i = 0; i < uNames.Length - 1; i++)
            {
                tb_external_url_employee urlObj = new tb_external_url_employee();
                urlObj.profile_id = id;
                urlObj.url_name = uNames[i];
                urlObj.url = urls[i];
                list.Add(urlObj);
            }

            return list;


        }
        public List<tb_skills_employee> GetSkillList(int id, String skillName, String skillExperience)
        {
            List<tb_skills_employee> list = new List<tb_skills_employee>();
            String[] sNames = skillName.Split(',');
            String[] sExps = skillExperience.Split(',');

            for (int i = 0; i < sNames.Length - 1; i++)
            {
                tb_skills_employee skillObj = new tb_skills_employee();
                skillObj.profile_id = id;
                skillObj.skill_name = sNames[i];
                skillObj.skill_experience = sExps[i];
                list.Add(skillObj);
            }

            return list;


        }
        public List<tb_experience_employee> GetExperienceList(int id, String jTitle, String cLocation, String currExp, String sDate, String edate, String notes, String cName)
        {
            List<tb_experience_employee> list = new List<tb_experience_employee>();
            String[] titles = jTitle.Split(',');
            String[] locations = cLocation.Split(',');
            //   String[] exps = currExp.Split(',');
            // String[] projs = currProject.ToString().Split(',');
            //    String[] sDates= startDate.ToString().Split(',');
            //   String[] eDates= endDate.ToString().Split(',');
            String[] expNotes = notes.Split(',');
            String[] compNames = cName.Split(',');


            for (int i = 0; i < titles.Length - 1; i++)
            {
                tb_experience_employee expObj = new tb_experience_employee();


                expObj.profile_id = id;
                expObj.job_title = titles[i];
                expObj.company_location = locations[i];
                expObj.experience_currently_working = false;
                expObj.experience_start_date = null;
                expObj.experience_end_date = null;
                expObj.experience_notes = expNotes[i];
                expObj.company_name = compNames[i];
                list.Add(expObj);


            }
            return list;
        }
        public List<tb_education_employee> GetEducationList(int id, String degTitle, String instName, String studyField, String sDate, String edate, String notes)
        {
            List<tb_education_employee> list = new List<tb_education_employee>();
            String[] titles = degTitle.Split(',');
            String[] sFields = studyField.Split(',');
            // String[] projs = currProject.ToString().Split(',');
            //    String[] sDates= startDate.ToString().Split(',');
            //   String[] eDates= endDate.ToString().Split(',');
            String[] eduNotes = notes.Split(',');
            String[] instNames = instName.Split(',');


            for (int i = 0; i < titles.Length - 1; i++)
            {
                tb_education_employee eduObj = new tb_education_employee();
                eduObj.profile_id = id;
                eduObj.degree_title = titles[i];
                eduObj.institution_name = instNames[i];
                eduObj.field_of_study = sFields[i];
                eduObj.education_start_date = null;
                eduObj.education_end_date = null;
                eduObj.education_notes = eduNotes[i];
                list.Add(eduObj);


            }
            return list;
        }
        public List<tb_department_employer> GetDepartmentList(int id, String department_name, String no_of_employee_in_department, String department_intro)
        {
            List<tb_department_employer> list = new List<tb_department_employer>();
            String[] names = department_name.Split(',');
            String[] employees = no_of_employee_in_department.Split(',');
          
            String[] intro = department_intro.Split(',');



            for (int i = 0; i < employees.Length - 1; i++)
            {
                tb_department_employer deptObj = new tb_department_employer();
                deptObj.profile_id = id;
                deptObj.department_name = names[i];
                deptObj.no_of_employee_in_department = int.Parse(employees[i]);
                deptObj.department_intro = intro[i];
                list.Add(deptObj);


            }
            return list;
        }
        }
        
    }
