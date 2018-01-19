using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cherry.Data.Administration;
using Cherry.Data.Schools;
using Cherry.Web.DataContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cherry.Web.Pages.Admin
{
    public class ManageModel : PageModel
    {
        private readonly IConfigurationContextDb configuration;
        public ManageModel(IConfigurationContextDb _configuration)
        {
            configuration = _configuration;
        }
        public void OnGet()
        {
            DbSet<School> schools = configuration.Schools;
            foreach (School school in schools)
            {
                SchoolDb schooldb = new SchoolDb(school);
                SchoolClass schoolClass = new SchoolClass(school)
                {
                    FullName = "Class"
                };
                schooldb.Add(schoolClass);
                schooldb.SaveChanges();
            }
        }
    }
}