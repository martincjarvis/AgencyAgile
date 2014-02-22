using AgencyAgile.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace AgencyAgile.Web.Support
{
    public class ClientRouteConstraint : IRouteConstraint
    {



        public bool Match(HttpContextBase httpContext
            , Route route
            , string parameterName
            , RouteValueDictionary values
            , RouteDirection routeDirection)
        {
            var tenant = (string)values["tenant"];
            if (string.IsNullOrEmpty(tenant)) return false;
            var value = (string)values[parameterName];
            var ctx = AgencyDbContext.Create(tenant);
            return ctx.Clients.Where(c => string.Equals(c.Slug, value, StringComparison.OrdinalIgnoreCase)).Any();

        }
    }
}