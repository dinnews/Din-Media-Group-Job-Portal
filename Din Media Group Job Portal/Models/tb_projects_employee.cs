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
    
    public partial class tb_projects_employee
    {
        public int id { get; set; }
        public int profile_id { get; set; }
        public string project_title { get; set; }
        public string position { get; set; }
        public string project_url { get; set; }
        public bool currently_working { get; set; }
        public Nullable<System.DateTime> start_date { get; set; }
        public Nullable<System.DateTime> end_date { get; set; }
        public string notes { get; set; }
    
        public virtual tb_profile_employee tb_profile_employee { get; set; }
    }
}
