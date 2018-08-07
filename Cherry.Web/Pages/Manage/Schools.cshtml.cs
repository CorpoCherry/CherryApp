using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cherry.Data.Configuration;
using Cherry.Data.Configuration.Customers;
using Cherry.Web.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cherry.Web.Pages.Manage
{
    public class SchoolsModel : PageModel
    {
        private readonly IConfigurationContext configuration;

        public SchoolsModel(IConfigurationContext _configuration)
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

            ISearcher<Tenant> search = new SchoolSearcher(configuration, ref HowMany, ref WhatPage, ref name);
            search.Search();
           
            var temp_list = new List<object>();
            foreach (Tenant x in search.Result)
            {
                temp_list.Add(new { x.OfficialName, x.Tag });
            }

            return Task.FromResult(new JsonResult(new { schools = temp_list, allin = search.MaximumInQueue }));
        }

        public Task<JsonResult> OnPostGetItem(string tag)
        {
            Tenant item = configuration.GetTenant(tag);
            return Task.FromResult(new JsonResult(item));
        }
    }

}
