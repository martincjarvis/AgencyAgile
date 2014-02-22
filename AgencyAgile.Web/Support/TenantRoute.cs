using AgencyAgile.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AgencyAgile.Web.Support
{
    /// <summary>
    /// http://stackoverflow.com/questions/278668/is-it-possible-to-make-an-asp-net-mvc-route-based-on-a-subdomain
    /// </summary>
    public class TenantRoute : Route
    {
        public TenantRoute(string url) : base(url, new MvcRouteHandler()) { }

        private TenantDbContext db = new TenantDbContext();

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);
            if (routeData == null) return null; // Only look at the subdomain if this route matches in the first place.
            string tenant = httpContext.Request.Params["tenant"]; // A subdomain specified as a query parameter takes precedence over the hostname.
            if (tenant == null)
            {
                string host = httpContext.Request.Headers["Host"];
                int index = host.IndexOf('.');
                if (index >= 0)
                    tenant = host.Substring(0, index);
            }
            if (tenant != null)
                routeData.Values["tenant"] = tenant;
            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            //object tenant = requestContext.HttpContext.Request.Params["tenantId"];
            //if (tenant != null)
            //    values["tenantId"] = tenant;
            //return base.GetVirtualPath(requestContext, values);
            return null;
        }
    }
}