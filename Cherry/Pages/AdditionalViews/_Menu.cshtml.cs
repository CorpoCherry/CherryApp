using Cherry.Data.Administration;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cherry.Web.Pages.AdditionalViews
{
    public class _MenuModel : PageModel
    {
        public User CurrentUser { get; set; }

        public _MenuModel()
        {
            
        }
    }
}