using Cherry.Data.Configuration.Customers;
using Cherry.Data.Configuration.Locales;
using Cherry.Data.Configuration.Security;
using Microsoft.EntityFrameworkCore;

namespace Cherry.Data.Configuration
{
    public interface IConfigurationContext
    {
        DbSet<Tenant> Tenants { get; set; }
        DbSet<City> Cities { get; set; }
        DbSet<DatabaseLogin> DatabaseLogins { get; set; }

        string ConnectionString { get; }

        Tenant GetTenant(string tag);
    }
}
