using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cherry.Data.Administration;
using Cherry.Data.Globals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cherry.Web.DataContexts
{
    public interface IConfigurationContextDb
    {
        DbSet<School> Schools { get; set; }
        DbSet<City> Cities { get; set; }

        School GetSchool(string tag);
    }

    public class ConfigurationContextDb : DbContext, IConfigurationContextDb
    {
        public ConfigurationContextDb(DbContextOptions<ConfigurationContextDb> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<School>()
                .HasIndex(c => c.Tag)
                .IsUnique();
        }

        public School GetSchool(string tag)
        {
            return (from x in Schools
                    where x.Tag == tag
                    select x).Single();
        }

        public virtual DbSet<School> Schools { get; set; }
        public virtual DbSet<City> Cities { get; set; }
    }
}
