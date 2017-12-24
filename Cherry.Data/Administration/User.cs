using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Cherry.Data.Administration
{
    public class User : IdentityUser
    {
        [StringLength(255)]
        public string Class { get; set; }  
         
        [StringLength(255)]
        public string FirstName { get; set; }
        [StringLength(255)]
        public string LastName { get; set; }
    }
}
