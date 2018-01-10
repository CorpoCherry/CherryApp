using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cherry.Data.Administration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cherry.Web.Pages
{
    public class IndexModel : PageModel
    {
        public AdditionalViews._MenuModel _MenuModel { get; set; }
        private UserManager<User> UserManager { get; set; }
        public User CurrentUser { get; set; }

        public IndexModel(UserManager<User> usmg)
        {
            UserManager = usmg;
        }

        public async Task OnGet()
        {
            if (ModelState.IsValid)
            {
                CurrentUser = await UserManager.GetUserAsync(User);
                _MenuModel = new AdditionalViews._MenuModel(CurrentUser);
            }
        }
    }
}