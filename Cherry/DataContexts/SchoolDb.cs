using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Cherry.Data.Administration;
using Cherry.Data.Schools;
using Microsoft.EntityFrameworkCore;

namespace Cherry.Web.DataContexts
{
    public class SchoolDb : DbContext
    {


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        private readonly School tenant;

        public SchoolDb(School tenant)
        {
            this.tenant = tenant;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            string build = Globals.DatabaseParamaters.DBtypebool ? Properties.Keys.ConnectionStringLocal.Replace("<tag_here>", tenant.Tag) : Properties.Keys.ConnectionStringOnline.Replace("<tag_here>", tenant.Tag);
            optionsBuilder.UseMySQL(build);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<SchoolClass> Class { get; set; }
    }
}
