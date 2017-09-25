using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCBootstrapDemo
{
    public class RouteConfig
    {
        //void Application_Start(object sender, EventArgs e)
        //{
        //    RegisterRoutes(RouteTable.Routes);

        //}

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Login", id = UrlParameter.Optional }               
            );

            //routes.MapPageRoute("InventoryList", "InventoryList", "~/Views/InventoryList.aspx");
        }
    }
}
