using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MoneyReport
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Cedentes",
                "Relatorios/Cedentes",
                new { controller = "Relatorios", action = "Cedentes", id = 0 }
            );

            routes.MapRoute(
                "RelAberto",
                "Relatorios/RelAberto",
                new { controller = "Relatorios", action = "RelAberto", id = 0 }
            );

            routes.MapRoute(
                "ExecAberto",
                "Relatorios/ExecAberto",
                new { controller = "Relatorios", action = "ExecAberto", id = 0 }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
