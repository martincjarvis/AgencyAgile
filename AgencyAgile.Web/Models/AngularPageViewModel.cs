using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgencyAgile.Web.Models
{
    public class AngularPageViewModel
    {

        public AngularPageViewModel(string appId, TenantViewModel tenant)
        {
            AngularAppId = appId;
            Tenant = tenant;
        }

        public string AngularAppId { get; private set; }

        public TenantViewModel Tenant { get; private set; }

    }
}