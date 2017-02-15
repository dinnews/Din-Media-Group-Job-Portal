using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Din_Media_Group_Job_Portal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                  name: "Employer",
                  url: "Employer/{id}",
                  defaults: new { controller = "Employer", action = "EmployerHome", id = UrlParameter.Optional }
       );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "User", action = "Home", id = UrlParameter.Optional }
            );
            
        }
    }
}