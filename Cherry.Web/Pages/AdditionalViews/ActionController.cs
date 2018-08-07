using System.Threading.Tasks;
using Cherry.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cherry.Web.Pages.AdditionalViews
{
    public class ActionController : Controller
    {
        private readonly SignInManager<User> _signInManager;

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Login");
        }

        public ActionController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }
    }
}