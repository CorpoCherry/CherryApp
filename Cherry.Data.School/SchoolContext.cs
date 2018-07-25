using Cherry.Data.Configuration;
using Cherry.Data.Configuration.Customers;
using Cherry.Data.Configuration.Global;
using Cherry.Data.School.Groups;
using Cherry.Data.School.Persons;
using Microsoft.EntityFrameworkCore;

namespace Cherry.Data.School
{
    public class SchoolContext : DbContext, ISchoolContext 
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        private readonly Tenant _tenant;
        private readonly ConfigurationContext _configurationContext;
        private readonly LoginManager loginManager;

        public SchoolContext(Tenant tenant, ConfigurationContext configurationContext)
        {
            loginManager = new LoginManager(_tenant, _configurationContext);
            if(!loginManager.Exists)
            {
                loginManager.Create();
            }
            _tenant = tenant;
            _configurationContext = configurationContext;
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Load key and generate connection string
            
            ConnectionStringBuilder connectionStringBuilder = new ConnectionStringBuilder(loginManager.Login);

            //Login to database
            optionsBuilder.UseMySql(connectionStringBuilder.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Class> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
    }
}
