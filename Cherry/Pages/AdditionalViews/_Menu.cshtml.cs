using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cherry.Data.Administration;
using Cherry.Web.DataContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cherry.Web.Pages.AdditionalViews
{
    public class _MenuModel : PageModel
    {
        public User CurrentUser { get; set; }

        public _MenuModel(User usmg)
        {
            CurrentUser = usmg;
        }
    }
}