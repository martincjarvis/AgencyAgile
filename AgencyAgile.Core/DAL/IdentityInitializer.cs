using AgencyAgile.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAgile.DAL
{
    public class IdentityInitializer :
        //System.Data.Entity.DropCreateDatabaseAlways<IdentityDbContext>
      System.Data.Entity.CreateDatabaseIfNotExists<IdentityDbContext>
    {
        protected override void Seed(IdentityDbContext context)
        {
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            CreateUser(um, "System");
            CreateUser(um, "DemoTechnicalDirector");
            CreateUser(um, "DemoTechnicalArchitect");
            CreateUser(um, "DemoTechnicalLead");
            CreateUser(um, "DemoDeveloperA");
            CreateUser(um, "DemoDeveloperB");
            CreateUser(um, "DemoDeveloperC");
            CreateUser(um, "DemoDesigner");
            CreateUser(um, "DemoUX");
            CreateUser(um, "DemoProjectManager");
            CreateUser(um, "DemoAccountManager");

            base.Seed(context);
        }

        private static void CreateUser(UserManager<ApplicationUser> um, string username)
        {
            var ir = um.Create(new ApplicationUser { UserName = username }, Guid.NewGuid().ToString("d"));
            if (!ir.Succeeded) throw new NotSupportedException(string.Join("\n", ir.Errors));
        }
    }
}
