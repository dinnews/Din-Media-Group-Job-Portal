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
    
    public partial class tb_department_employer
    {
        public tb_department_employer()
        {
            this.tb_jobs = new HashSet<tb_jobs>();
        }
    
        public int id { get; set; }
        public int profile_id { get; set; }
        public string department_name { get; set; }
        public int no_of_employee_in_department { get; set; }
        public string department_intro { get; set; }
    
        public virtual tb_profile_employer tb_profile_employer { get; set; }
        public virtual ICollection<tb_jobs> tb_jobs { get; set; }
    }
}