using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cherry.Web.DataContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cherry.Web.Pages.Admin
{
    public class ManageModel : PageModel
    {
        public void OnGet(IServiceCollection services)
        {
            services.AddDbContext<SchoolsDb>
            (options => options.UseMySQL("server=localhost;database=cherry_schools2;uid=root;pwd=x7ppwe;pooling=true;"));
            

        }
    }
}