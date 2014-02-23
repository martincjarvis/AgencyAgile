using AgencyAgile.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.History;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAgile.DAL
{
    public class AgencyDbContext : DbContext, IDbModelCacheKeyProvider
    {

        private static string DefaultConnection = AgencyAgile.Properties.Settings.Default.DefaultConnectionStringName;

        public static AgencyDbContext Create(string tenantSchema)
        {
            var connectionString = DefaultConnection;
            var tenantDataMigrationsConfiguration = new DbMigrationsConfiguration<AgencyDbContext>();
            tenantDataMigrationsConfiguration.AutomaticMigrationsEnabled = false;
            tenantDataMigrationsConfiguration.SetSqlGenerator("System.Data.SqlClient", new SqlServerSchemaAwareMigrationSqlGenerator(tenantSchema));
            tenantDataMigrationsConfiguration.SetHistoryContextFactory("System.Data.SqlClient", (existingConnection, defaultSchema) => new HistoryContext(existingConnection, tenantSchema));
            tenantDataMigrationsConfiguration.TargetDatabase = new System.Data.Entity.Infrastructure.DbConnectionInfo(connectionString, "System.Data.SqlClient");
            tenantDataMigrationsConfiguration.MigrationsAssembly = typeof(AgencyDbContext).Assembly;
            tenantDataMigrationsConfiguration.MigrationsNamespace = "AgencyAgile.Migrations.Agency";

            DbMigrator tenantDataCtxMigrator = new DbMigrator(tenantDataMigrationsConfiguration);
            tenantDataCtxMigrator.Update();
            return new AgencyDbContext(tenantSchema, connectionString);
        }

        // Used by EF migrations
        public AgencyDbContext()
        {
            Database.SetInitializer<IdentityDbContext>(null);
        }

        internal AgencyDbContext(string tenantSchema, string connectionStringName)
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

        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<CopyBlock> CopyBlocks { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Fragment> Fragments { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Response> Responses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (this.SchemaName != null)
            {
                modelBuilder.HasDefaultSchema(this.SchemaName);
            }
            // modelBuilder.ComplexType<TimingPoint>();
            modelBuilder.ComplexType<AuditedAction>()
                .Property(a => a.ById).IsRequired().HasMaxLength(128);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<Client>().HasKey(a => a.ClientId).Property(a => a.ClientId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Client>().Property(c => c.Slug).IsRequired();
            modelBuilder.Entity<Client>().Property(c => c.Name).IsRequired();


            modelBuilder.Entity<Document>().HasKey(a => a.DocumentId).Property(a => a.DocumentId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Document>().HasOptional(a => a.Client);
            modelBuilder.Entity<Document>().HasOptional(a => a.Job);
            modelBuilder.Entity<Document>().HasRequired(a => a.DocumentType);
            modelBuilder.Entity<Document>().HasMany(a => a.Sections);
            modelBuilder.Entity<Document>().HasMany(a => a.Questions);
            modelBuilder.Entity<Document>().HasMany(a => a.Features);
            modelBuilder.Entity<Document>().Property(d => d.Title).IsRequired();
            modelBuilder.Entity<Document>().Property(d => d.Slug).IsRequired();

            modelBuilder.Entity<DocumentType>().HasKey(a => a.DocumentTypeId).Property(a => a.DocumentTypeId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<DocumentType>().Property(dt => dt.Name).IsRequired();
            modelBuilder.Entity<DocumentType>().Property(dt => dt.Slug).IsRequired();

            modelBuilder.Entity<CopyBlock>().HasKey(a => a.CopyBlockId).Property(a => a.CopyBlockId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<CopyBlock>().HasRequired(a => a.Latest);
            modelBuilder.Entity<CopyBlock>().HasMany(a => a.History);
            modelBuilder.Entity<CopyBlock>().HasMany(a => a.SubSections);


            modelBuilder.Entity<Feature>().HasKey(a => a.FeatureId).Property(a => a.FeatureId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Feature>().HasRequired(a => a.Introduction);
            modelBuilder.Entity<Feature>().HasMany(a => a.SubFeatures).WithRequired(a => a.Parent);
            
            modelBuilder.Entity<Fragment>().HasKey(a => a.FragmentId).Property(a => a.FragmentId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Job>().HasKey(a => a.JobId).Property(a => a.JobId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Job>().Property(j => j.Slug).IsRequired();
            modelBuilder.Entity<Job>().Property(j => j.Name).IsRequired();
            modelBuilder.Entity<Job>().Property(j => j.Reference).IsRequired();
            modelBuilder.Entity<Job>().HasMany(a => a.Documents).WithOptional(d => d.Job);
            
            modelBuilder.Entity<Question>().HasKey(a => a.QuestionId).Property(a => a.QuestionId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Question>().HasRequired(a => a.Text);
            modelBuilder.Entity<Question>().HasMany(a => a.Responses).WithRequired(r => r.Question);
            modelBuilder.Entity<Question>().HasOptional(a => a.Answer);
            
            modelBuilder.Entity<Response>().HasKey(a => a.ResponseId).Property(a => a.ResponseId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Response>().HasRequired(a => a.Question).WithMany(q => q.Responses);
            modelBuilder.Entity<Response>().HasRequired(a => a.Text);
            

            base.OnModelCreating(modelBuilder);
        }
    }
}
