using AgencyAgile.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAgile.DAL
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser>, IDbModelCacheKeyProvider
    {

        private static string DefaultConnection = AgencyAgile.Properties.Settings.Default.DefaultConnectionStringName;

        public static IdentityDbContext Create(string tenantSchema)
        {
            return new IdentityDbContext(tenantSchema, DefaultConnection);
        }

        // Used by EF migrations
        public IdentityDbContext()
        {
            Database.SetInitializer<IdentityDbContext>(null);
        }

        internal IdentityDbContext(string tenantSchema, string connectionStringName)
            : base(connectionStringName)
        {
            //Database.SetInitializer<IdentityDbContext>(null);
            this.SchemaName = tenantSchema ?? "dbo";
        }

        public string SchemaName { get; private set; }

        public string CacheKey
        {
            get { return this.SchemaName; }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (this.SchemaName != null)
            {
                modelBuilder.HasDefaultSchema(this.SchemaName);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
