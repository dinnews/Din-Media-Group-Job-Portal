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
    
    public partial class tb_employer_registration_data
    {
        public int id { get; set; }
        public string email { get; set; }

        public string first_name { get; set; }
        public string last_name { get; set; }
        public string company_name { get; set; }
        public string mobile { get; set; }
        public string cnic { get; set; }
        public Nullable<System.DateTime> last_login { get; set; }
        public int user_id { get; set; }
    
        public virtual tb_user tb_user { get; set; }
    }
}
