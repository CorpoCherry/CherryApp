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
        private UserManager<User> userManager { get; set; }
        public User CurrentUser { get; set; }

        public IndexModel(UserManager<User> usmg)
        {
            userManager = usmg;
        }

        public async Task OnGet()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            CurrentUser = await userManager.FindByIdAsync(userId);
            _MenuModel = new AdditionalViews._MenuModel(CurrentUser);
        }
    }
}