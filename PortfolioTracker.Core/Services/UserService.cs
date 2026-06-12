using Frontend.Data;
using Frontend.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Core.Interfaces;
using PortfolioTracker.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using PortfolioTracker.Core.Data;

namespace PortfolioTracker.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;


        public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;

        }

        public async Task<RegistrationResult> RegisterAsync(string username, string email, string password, string displayName, DateTime dateOfBirth)
        {
            var user = new ApplicationUser
            {
                UserName = username,
                Email = email
            };  
               
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                
                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return RegistrationResult.Failure(errors);
                }
                var bio = new UserBio
                {
                    UserId = user.Id,
                    DisplayName = displayName,
                    DateOfBirth = dateOfBirth
                };

                await _context.UserBio.AddAsync(bio);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return RegistrationResult.Success();
        }
    }

}
