using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cherry.Data.Administration;
using Cherry.Data.Globals;
using Cherry.Data.Schools;
using Cherry.Web.DataContexts;
using Cherry.Web.Search;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cherry.Web.Pages.Manage
{
    public class SchoolsModel : PageModel
    {
        private readonly IConfigurationContextDb configuration;

        public SchoolsModel(IConfigurationContextDb _configuration)
        {
            configuration = _configuration;
        }

        public Task<JsonResult> OnPostGetName(string count, string page, string name)
        {
            if(!int.TryParse(count, out int HowMany))
            {
                return Task.FromResult(new JsonResult(new { error = "Error" })); //TODO: Make errors more debug friendly
            }
            if (!int.TryParse(page, out int WhatPage))
            {
                return Task.FromResult(new JsonResult(new { error = "Error" })); //TODO: Make errors more debug friendly
            }

            ISearcher<School> search = new SchoolSearcher(configuration, ref HowMany, ref WhatPage, ref name);
            search.Search();
           
            var temp_list = new List<object>();
            foreach (School x in search.Result)
            {
                temp_list.Add(new { x.OfficialName, x.Tag });
            }

            return Task.FromResult(new JsonResult(new { schools = temp_list, allin = search.MaximumInQueue }));
        }

        public Task<JsonResult> OnPostGetItem(string tag)
        {
            School item = configuration.GetSchool(tag);
            item.ID = "secret";
            return Task.FromResult(new JsonResult(item));
        }
    }

}
