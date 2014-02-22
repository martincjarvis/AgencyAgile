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
            um.Create(new ApplicationUser { UserName = "System" }, Guid.NewGuid().ToString("d"));
            um.Create(new ApplicationUser { UserName = "Demo_Technical_Director" }, Guid.NewGuid().ToString("d"));
            um.Create(new ApplicationUser { UserName = "Demo_Technical_Architect" }, Guid.NewGuid().ToString("d"));
            um.Create(new ApplicationUser { UserName = "Demo_Technical_Lead" }, Guid.NewGuid().ToString("d"));
            um.Create(new ApplicationUser { UserName = "Demo_DeveloperA" }, Guid.NewGuid().ToString("d"));
            um.Create(new ApplicationUser { UserName = "Demo_DeveloperB" }, Guid.NewGuid().ToString("d"));
            um.Create(new ApplicationUser { UserName = "Demo_DeveloperC" }, Guid.NewGuid().ToString("d"));
            um.Create(new ApplicationUser { UserName = "Demo_Designer" }, Guid.NewGuid().ToString("d"));
            um.Create(new ApplicationUser { UserName = "Demo_UX" }, Guid.NewGuid().ToString("d"));
            um.Create(new ApplicationUser { UserName = "Demo_Project_Manager" }, Guid.NewGuid().ToString("d"));
            um.Create(new ApplicationUser { UserName = "Demo_Account_Manager" }, Guid.NewGuid().ToString("d"));
            base.Seed(context);
        }
    }
}
