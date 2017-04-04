using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Necropolis
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Home", "", new { controller = "Home", action = "Index" });

            routes.MapRoute("Catalog", "Catalog", new { controller = "Catalog", action = "Index" });

            routes.MapRoute("Services", "Services", new { controller = "Services", action = "Index" });

            routes.MapRoute("Infrmation", "Information", new { controller = "Information", action = "Information" });

            routes.MapRoute("Contact", "Contact", new { controller = "Home", action = "Contact" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                );
        }
    }
}
