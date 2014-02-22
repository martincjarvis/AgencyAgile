using AgencyAgile.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace AgencyAgile.Web.Support
{
    public class JobRouteConstraint : IRouteConstraint
    {

        public bool Match(HttpContextBase httpContext
            , Route route
            , string parameterName
            , RouteValueDictionary values
            , RouteDirection routeDirection)
        {
            var tenant = (string)values["tenant"];
            if (string.IsNullOrEmpty(tenant)) return false;
            var client = (string)values["client"];
            var value = (string)values[parameterName];
            var ctx = AgencyDbContext.Create(tenant);
            var cl = ctx.Clients.Where(c => c.Slug == client).First();
            return ctx.Jobs.Where(j => j.Client == cl).Where(j => j.Slug == value).Any();

        }
    }
}