using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Cherry.Data.Identity
{
    public class User : IdentityUser
    {
        [StringLength(255)]
        public string FirstName { get; set; }
        [StringLength(255)]
        public string LastName { get; set; }

        public string LinkedTag { get; set; }
    }
}
