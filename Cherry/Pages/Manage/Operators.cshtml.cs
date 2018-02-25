using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cherry.Data.Administration;
using Cherry.Data.Schools;
using Cherry.Web.DataContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cherry.Web.Pages.Manage
{
    public class OperatorsModel : PageModel
    {
        private readonly IConfigurationContextDb configuration;
        private UserManager<User> UserManager { get; set; }
        public SignInManager<User> SignManager { get; }
        public AdditionalViews._MenuModel _MenuModel { get; set; }
        public User CurrentUser { get; set; }

        public DbSet<School> Schools { get; set; }

        public OperatorsModel(IConfigurationContextDb _configuration, UserManager<User> _usmg, SignInManager<User> _signManager)
        {
            configuration = _configuration;
            Schools = configuration.Schools;
            UserManager = _usmg;
            SignManager = _signManager;
        }

        public async Task OnGet()
        {
            if (ModelState.IsValid)
            {
                CurrentUser = await UserManager.GetUserAsync(User);
                _MenuModel = new AdditionalViews._MenuModel(CurrentUser);
            }
        }
        public async Task<IActionResult> OnPostLogOutAsync()
        {
            await SignManager.SignOutAsync();
            return RedirectToPage("/Login");
        }
    }
}