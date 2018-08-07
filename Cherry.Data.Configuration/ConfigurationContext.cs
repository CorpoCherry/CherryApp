using System.Linq;
using Cherry.Data.Configuration.Customers;
using Cherry.Data.Configuration.Global;
using Cherry.Data.Configuration.Locales;
using Cherry.Data.Configuration.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cherry.Data.Configuration
{
    public class ConfigurationContext : DbContext, IConfigurationContext
    {
        public ConfigurationContext()
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
            .HasMany(c => c.Tenants)
            .WithOne(t => t.City);

            modelBuilder.Entity<Tenant>()
            .HasOne(t => t.City)
            .WithMany(c => c.Tenants);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Load key and generate connection string
            Key.LoadKey(out string key);
            ConnectionStringBuilder connectionStringBuilder = new ConnectionStringBuilder("config", "cherry", key, true);
            ConnectionString = connectionStringBuilder.ConnectionString;

            //Login to database
            optionsBuilder.UseMySql(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<Tenant> Tenants { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<DatabaseLogin> DatabaseLogins { get; set; }

        public string ConnectionString { get; private set; }

        public Tenant GetTenant(string tag)
        {
            return Tenants.Find(tag);
        }

    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ConfigurationContext>
    {
        public ConfigurationContext CreateDbContext(string[] args)
        {
            return new ConfigurationContext();
        }
    }
}
