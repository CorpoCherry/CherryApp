using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Cherry.Data.Configuration;
using Cherry.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Cherry.Web.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly ConfigurationContext _configurationContext;

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public LoginModel(SignInManager<User> signInManager, ILogger<LoginModel> logger, UserManager<User> userManager, ConfigurationContext configurationContext)
        {
            _configurationContext = configurationContext;
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        public class InputModel
        {
            [Required]
            public string Login { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Zapamiętać?")]
            public bool Remember { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGet(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("Index");
            }
            await Data.School.Seed.School(_configurationContext);
            await Data.Identity.Seed.Users(_userManager);
            ErrorMessage = "";
            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostLoginAsync(string returnUrl)
        {
            ReturnUrl = returnUrl;
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
                else
                {
                    ErrorMessage = "Podano złe dane logowania...";
                }
            }
            return Page();
        }
    }
}