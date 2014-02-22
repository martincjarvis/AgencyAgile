using AgencyAgile.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAgile.DAL
{
    public class TenantDbContext : DbContext
    {

        private static string DefaultConnection = AgencyAgile.Properties.Settings.Default.DefaultConnectionStringName;

        public TenantDbContext()
            : base(DefaultConnection)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ComplexType<AuditedAction>();
            modelBuilder.Entity<Tenant>().HasKey(t => t.TenantId);
            modelBuilder.Entity<Tenant>().Property(t => t.Name).IsRequired();
            modelBuilder.Entity<Tenant>().Property(t => t.Slug).IsRequired();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Tenant> Tenants { get; set; }

    }
}
