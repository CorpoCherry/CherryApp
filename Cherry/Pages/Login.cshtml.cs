using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Cherry.Data.Administration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Cherry.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public LoginModel(SignInManager<User> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public class InputModel
        {
            [Required]
            public string Login { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Zapamiêtaæ?")]
            public bool Remember { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IActionResult OnGet(string returnUrl = null)
        {
            return Page();
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Login, Input.Password, Input.Remember, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToPage("./Index");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid login attempt");
            return Page();
        }
    }
}
