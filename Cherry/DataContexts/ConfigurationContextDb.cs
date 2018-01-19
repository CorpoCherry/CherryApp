using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cherry.Data.Administration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cherry.Web.DataContexts
{
    public interface IConfigurationContextDb
    {
        DbSet<School> Schools { get; set; }
    }

    public class ConfigurationContextDb : DbContext, IConfigurationContextDb
    {
        public ConfigurationContextDb(DbContextOptions<ConfigurationContextDb> options) : base(options)
        {

        }

        public virtual DbSet<School> Schools { get; set; }

    }
}
