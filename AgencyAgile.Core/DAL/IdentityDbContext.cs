using AgencyAgile.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.History;
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
            var connectionString = DefaultConnection;
            var tenantDataMigrationsConfiguration = new DbMigrationsConfiguration<IdentityDbContext>();
            tenantDataMigrationsConfiguration.AutomaticMigrationsEnabled = false;
            tenantDataMigrationsConfiguration.SetSqlGenerator("System.Data.SqlClient", new SqlServerSchemaAwareMigrationSqlGenerator(tenantSchema));
            tenantDataMigrationsConfiguration.SetHistoryContextFactory("System.Data.SqlClient", (existingConnection, defaultSchema) => new HistoryContext(existingConnection, tenantSchema));
            tenantDataMigrationsConfiguration.TargetDatabase = new System.Data.Entity.Infrastructure.DbConnectionInfo(connectionString, "System.Data.SqlClient");
            tenantDataMigrationsConfiguration.MigrationsAssembly = typeof(IdentityDbContext).Assembly;
            tenantDataMigrationsConfiguration.MigrationsNamespace = "AgencyAgile.Migrations.Identity";

            DbMigrator tenantDataCtxMigrator = new DbMigrator(tenantDataMigrationsConfiguration);
            tenantDataCtxMigrator.Update();
            return new IdentityDbContext(tenantSchema, connectionString);
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
