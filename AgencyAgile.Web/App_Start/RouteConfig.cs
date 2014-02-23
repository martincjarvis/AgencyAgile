using AgencyAgile.Web.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AgencyAgile.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapTenantRoute("Tenant Job Doc", "{client}/{job}/{document}"
            //    , new { controller = "Agency", action = "Document" }
            //    , new { document = new JobDocumentRouteConstraint() });

            //routes.MapTenantRoute("Tenant Job", "{client}/{job}"
            //    , new { controller = "Agency", action = "Job" }
            //    , new { job = new JobRouteConstraint() });

            //routes.MapTenantRoute("Tenant Client", "{client}"
            //    , new { controller = "Agency", action = "Client" }
            //    , new { client = new ClientRouteConstraint() });

            //routes.MapTenantRoute("Tenant Home", "{action}"
            //    , new { controller = "Agency", action = "Home" }
            //    , new { tenant = new TenantRouteConstraint() });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
