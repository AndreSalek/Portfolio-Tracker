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
using PortfolioTracker.Core.Models.Common;

namespace PortfolioTracker.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;


        public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }

        public async Task<AuthResult> RegisterAsync(string username, string email, string password, string displayName, DateTime dateOfBirth)
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
                    return AuthResult.Failure(errors);
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

            return AuthResult.Success();
        }

        public async Task<AuthResult> LoginAsync(string username, string password, bool rememberMe)
        {
            
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: true);

            if (result.IsLockedOut)
                return AuthResult.Failure(new[] { AuthErrors.AccountLocked });

            if (result.IsNotAllowed)
                return AuthResult.Failure(new[] { AuthErrors.EmailNotConfirmed });

            if (!result.Succeeded)
                return AuthResult.Failure(new[] { AuthErrors.InvalidCredentials });

            return AuthResult.Success();
        }

        public async Task<UserBioResult> GetUserBioDataAsync (string id)
        {

            var userBio = await _context.UserBio.FirstOrDefaultAsync(b => b.UserId == id);

            if (userBio == null)
                return UserBioResult.Failure(new[] { "User bio not found." });

            return UserBioResult.Success(userBio);
        }
        public async Task<UpdateUserResult> UpdateUserBioDataAsync(string userId, string username, string email, string displayName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (!string.IsNullOrEmpty(username))
            {
                user.UserName = username;
            }
            if (!string.IsNullOrEmpty(email))
            {
                user.Email = email;
            }
            


            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return UpdateUserResult.Failure(errors);
                }
                await _signInManager.RefreshSignInAsync(user);
                var userBio = await _context.UserBio.FirstOrDefaultAsync(b => b.UserId == userId);
                if (!string.IsNullOrEmpty(displayName))
                {
                    userBio.DisplayName = displayName;
                }
                _context.UserBio.Update(userBio);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return UpdateUserResult.Success();
        }
    }
        

}
    