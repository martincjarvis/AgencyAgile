using AgencyAgile.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace AgencyAgile.Web.Support
{
    public class TenantRouteConstraint : IRouteConstraint
    {



        public bool Match(HttpContextBase httpContext
            , Route route
            , string parameterName
            , RouteValueDictionary values
            , RouteDirection routeDirection)
        {
            var ctx = new TenantDbContext();
            var value = (string)values[parameterName];
            return ctx.Tenants.Where(t => t.Slug == value).Any();

        }
    }
}