using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Cherry.Web.Pages.AdditionalViews
{
    public class ActionController : Controller
    {
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}