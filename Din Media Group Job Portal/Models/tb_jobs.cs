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
    
    public partial class tb_jobs
    {
        public tb_jobs()
        {
            this.tb_apply_job = new HashSet<tb_apply_job>();
        }
    
        public int id { get; set; }
        public int user_id { get; set; }
        [Required]
        public string job_description { get; set; }
        [Required]
        public string job_catagory { get; set; }
        [Required]
        public string required_career_level { get; set; }
        [Required]
        public string required_skill { get; set; }
        public Nullable<bool> hide_salary { get; set; }
        public Nullable<double> salary_from { get; set; }
        public Nullable<double> salary_to { get; set; }
        public int deapartment_id { get; set; }
        [Required]
        public string job_location { get; set; }
        [Required]
        public int num_of_positions { get; set; }
        [Required]
        public string job_type { get; set; }
        [Required]
        public string gender_preference { get; set; }
        public System.DateTime closing_date { get; set; }
        public string required_qualification { get; set; }
        public string specific_degree_title { get; set; }
        public string experience_min { get; set; }
        public string experience_max { get; set; }
        public string age_min { get; set; }
        public string age_max { get; set; }
        public Nullable<bool> is_active { get; set; }
        public Nullable<bool> is_deleted { get; set; }
        public Nullable<bool> is_filled { get; set; }
        [Required]
        public string job_title { get; set; }
    
        public virtual tb_department_employer tb_department_employer { get; set; }
        public virtual tb_user tb_user { get; set; }
        public virtual ICollection<tb_apply_job> tb_apply_job { get; set; }
    }
}
