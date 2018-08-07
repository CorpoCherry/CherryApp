using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cherry.Data.Identity
{
    public static class Seed
    {
        public static async Task Users(UserManager<User> userManager)
        {
            var root = new User
            {
                FirstName = "Alan (ROOT)",
                LastName = "Borowy",
                UserName = "root",
                Email = "root@cherryapp.pl",
                EmailConfirmed = true
            };

            var manager = new User
            {
                FirstName = "Alan (MGR)",
                LastName = "Borowy",
                UserName = "manager",
                Email = "manager@cherryapp.pl",
                EmailConfirmed = true
            };

            var user = new User
            {
                FirstName = "Alan (USER)",
                LastName = "Borowy",
                UserName = "user",
                Email = "user@cherryapp.pl",
                EmailConfirmed = true
            };

            if(await userManager.FindByNameAsync(root.UserName) == null)
                await userManager.CreateAsync(root, "rootBasic98");
            if (await userManager.FindByNameAsync(manager.UserName) == null)
                await userManager.CreateAsync(manager, "managerBasic98");
            if (await userManager.FindByNameAsync(user.UserName) == null)
                await userManager.CreateAsync(user, "userBasic98");
        }
    }
}
