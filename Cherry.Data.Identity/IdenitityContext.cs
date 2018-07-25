using Cherry.Data.Configuration;
using Cherry.Data.Configuration.Global;
using Cherry.Data.Configuration.Security;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cherry.Data.Identity
{
    public class IdentityContext : IdentityDbContext<User>
    {
        private readonly ConfigurationContext _configurationContext;
        private readonly LoginManager loginManager;

        public IdentityContext(ConfigurationContext configurationContext, DbContextOptions<IdentityContext> options) : base(options)
        {
            _configurationContext = configurationContext;
            loginManager = new LoginManager("cherry_identity", "identity", _configurationContext);
            if (!loginManager.Exists)
            {
                loginManager.Create();
            }
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConnectionStringBuilder connectionStringBuilder = new ConnectionStringBuilder(loginManager.Login);
            var ConnectionString = connectionStringBuilder.ConnectionString;

            //Login to database
            optionsBuilder.UseMySql(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }

    //public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext<User>>
    //{
    //    public IdentityContext CreateDbContext(string[] args)
    //    {
    //        return new IdentityContext();
    //    }
    //}
}
