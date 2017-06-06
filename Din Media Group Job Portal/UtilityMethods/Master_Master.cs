using Din_Media_Group_Job_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Din_Media_Group_Job_Portal.UtilityMethods
{
    public static class Master_Master
    {
       public  static List<tb_master_masters> master_list;
        public static void Master_fill()
        {
            var db = new DinJobPortalEntities();

            master_list = db.tb_master_masters.ToList();
            
        }
    }
}