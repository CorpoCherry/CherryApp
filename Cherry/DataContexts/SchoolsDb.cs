using Cherry.Data.Administration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cherry.Data;
using System.Collections.Generic;

namespace CherryAppManagment.DataContexts
{
    public class SchoolsDb : DbContext
    {
        public SchoolsDb(DbContextOptions<SchoolsDb> options) : base(options)
        {

        }

        public DbSet<School> Schools { get; set; }
    }
}
