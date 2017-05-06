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
    
    public partial class tb_experience_employee
    {
        public int id { get; set; }
        public int profile_id { get; set; }
        [Required]
        public string job_title { get; set; }
        [Required]
        [RegularExpression(@"^(?![\W_]+$)(?!\d+$)[a-zA-Z0-9 .&',_-]+$", ErrorMessage = "Company Name must contain atleast 1 Alphabets")]
        public string company_mame { get; set; }
        [Required]
        [RegularExpression(@"^(?![\W_]+$)(?!\d+$)[a-zA-Z0-9 .&',_-]+$", ErrorMessage = "Company Location must contain atleast 1 Alphabets")]
        public string company_location { get; set; }
        public bool currently_working { get; set; }
        public Nullable<System.DateTime> start_date { get; set; }
        public Nullable<System.DateTime> end_date { get; set; }
        public string notes { get; set; }
    
        public virtual tb_profile_employee tb_profile_employee { get; set; }
    }
}
