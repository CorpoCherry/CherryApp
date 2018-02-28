using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cherry.Data.Administration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cherry.Web.Interfaces
{
    public interface IUserProfileLoader
    {
        Task<User> GetCurrentUser(ClaimsPrincipal user);
    }
    public class UserServices : IUserProfileLoader
    {
        private readonly UserManager<User> _userManager;
        private User _currentUser;

        public UserServices([FromServices]UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> GetCurrentUser(ClaimsPrincipal user)
        {
            if(_currentUser == null)
            {
                _currentUser = await _userManager.GetUserAsync(user);
            }
            return _currentUser;
        }
    }
}
