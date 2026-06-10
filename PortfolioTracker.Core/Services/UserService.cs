using Frontend.Data.Models;
using Microsoft.AspNetCore.Identity;
using PortfolioTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RegistrationResult> RegisterAsync(string username, string email, string password)
        {
            var user = new ApplicationUser
            {
                UserName = username,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return RegistrationResult.Failure(errors);
            }

            return RegistrationResult.Success();
        }
    }

}
