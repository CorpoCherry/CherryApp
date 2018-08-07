using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cherry.Data.Configuration;
using Cherry.Data.Identity.Interfaces;
using Cherry.Data.School;
using Cherry.Data.School.Persons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cherry.Data.Identity.Managers
{
    public class StudentManager : IStudentManager
    {
        private readonly UserManager<User> _userManager;
        private User _currentUser;
        private IHttpContextAccessor _httpContextAccessor;
        private ConfigurationContext _configurationContext;

        public StudentManager([FromServices]UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, ConfigurationContext configurationContext)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _configurationContext = configurationContext;
        }

        public async Task<User> GetCurrentUser(ClaimsPrincipal user)
        {
            if (_currentUser == null)
            {
                _currentUser = await _userManager.GetUserAsync(user);
            }
            return _currentUser;
        }

        public Student CurrentStudent
        {
            get
            {
                var user = Task.Run(() => GetCurrentUser(_httpContextAccessor.HttpContext.User)).Result;
                var role = _userManager.IsInRoleAsync(user, "Student").Result;
                if (role == true)
                {
                    var apendedSchool = _configurationContext.GetTenant(user.LinkedTag);
                    var data = new SchoolContext(apendedSchool, _configurationContext);
                    return data.Students.Find(user.UserName);
                }
                
                //If user isn't linked with student account return null
                return null;
            }
        }
    }
}
