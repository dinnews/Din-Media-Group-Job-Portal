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
    
    public partial class tb_external_url_employee
    {
        public int id { get; set; }
        public int profile_id { get; set; }
        public string url_name { get; set; }
        public string url { get; set; }
    
        public virtual tb_profile_employee tb_profile_employee { get; set; }
    }
}
