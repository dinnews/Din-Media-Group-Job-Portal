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
    
    public partial class tb_employer_registration_data
    {
        public int id { get; set; }
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                             @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email is not valid")]
        public string email { get; set; }
        [Required]
        [RegularExpression(@"^(?![\W_]+$)(?!\d+$)[a-zA-Z0-9 .&',_-]+$", ErrorMessage = "First Name must contain atleast 1 Alphabets")]
        public string first_name { get; set; }
        [Required]
        [RegularExpression(@"^(?![\W_]+$)(?!\d+$)[a-zA-Z0-9 .&',_-]+$", ErrorMessage = "Last Name must contain atleast 1 Alphabets")]
        public string last_name { get; set; }
        [Required]
        [RegularExpression(@"^(?![\W_]+$)(?!\d+$)[a-zA-Z0-9 .&',_-]+$", ErrorMessage = "Company Name must contain atleast 1 Alphabets")]
        public string company_name { get; set; }
        [Required]
        [RegularExpression(@"^[\d-]+$", ErrorMessage = "Mobile Number format is 9999-1234567 (Only Digits Allowed)")]

        public string mobile { get; set; }
        [RegularExpression(@"^[\d-]+$", ErrorMessage = "CNIC format is 23202-1234567-6 (Only Digits Allowed)")]
        public string cnic { get; set; }
        public Nullable<System.DateTime> last_login { get; set; }
        public int user_id { get; set; }
    
        public virtual tb_user tb_user { get; set; }
    }
}
