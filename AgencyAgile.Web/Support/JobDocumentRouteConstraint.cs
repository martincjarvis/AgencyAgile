using AgencyAgile.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace AgencyAgile.Web.Support
{
    public class JobDocumentRouteConstraint : IRouteConstraint
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
            var job = (string)values["job"];
            var value = (string)values[parameterName];
            var ctx = AgencyDbContext.Create(tenant);
            var cl = ctx.Clients.Where(c => c.Slug == client).First();
            var j = ctx.Jobs.Where(c => c.Slug == job).First();
            return ctx.Documents.Where(d => d.Client == cl)
                .Where(d => d.Job == j)
                .Where(d => d.Slug == value)
                .Any();

        }
    }
}