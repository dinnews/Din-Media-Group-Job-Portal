//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Din_Media_Group_Job_Portal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    
    public partial class tb_profile_employee
    {
        public tb_profile_employee()
        {
            this.tb_education_employee = new HashSet<tb_education_employee>();
            this.tb_experience_employee = new HashSet<tb_experience_employee>();
            this.tb_external_url_employee = new HashSet<tb_external_url_employee>();
            this.tb_languages_employee = new HashSet<tb_languages_employee>();
            this.tb_projects_employee = new HashSet<tb_projects_employee>();
            this.tb_skills_employee = new HashSet<tb_skills_employee>();
            this.tb_apply_job = new HashSet<tb_apply_job>();
            this.tb_cvs_employee = new HashSet<tb_cvs_employee>();
        }
    
        public int id { get; set; }
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                             @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email is not valid")]
        public string email { get; set; }

        public string profile_picture { get; set; }
        public string cover_picture { get; set; }
        [Required]
        [AllowHtml]
        [RegularExpression(@"^(?![\W_]+$)(?!\d+$)[a-zA-Z0-9 .&',_-]+$", ErrorMessage = "Summary must contain atleast 1 Alphabets")]
        public string summary { get; set; }
        [Required]
        [RegularExpression(@"^[\d-]+$", ErrorMessage = "Mobile Number format is 9999-1234567 (Only Digits Allowed)")]
        public string mobile { get; set; }
        public string video_id { get; set; }
        public int user_id { get; set; }
    
        public virtual ICollection<tb_education_employee> tb_education_employee { get; set; }
        public virtual ICollection<tb_experience_employee> tb_experience_employee { get; set; }
        public virtual ICollection<tb_external_url_employee> tb_external_url_employee { get; set; }
        public virtual ICollection<tb_languages_employee> tb_languages_employee { get; set; }
        public virtual tb_user tb_user { get; set; }
        public virtual ICollection<tb_projects_employee> tb_projects_employee { get; set; }
        public virtual ICollection<tb_skills_employee> tb_skills_employee { get; set; }
        public virtual ICollection<tb_apply_job> tb_apply_job { get; set; }
        public virtual ICollection<tb_cvs_employee> tb_cvs_employee { get; set; }
    }
}
