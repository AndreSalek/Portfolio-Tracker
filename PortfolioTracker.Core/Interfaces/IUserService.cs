using Org.BouncyCastle.Bcpg.OpenPgp;
using PortfolioTracker.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;


namespace PortfolioTracker.Core.Interfaces
{
    public interface IUserService
    {
        Task<AuthResult> RegisterAsync(string username, string email, string password, string displayName, DateTime dateOfBirth);
        Task<AuthResult> LoginAsync(string username, string password, bool isLogged);

        Task<UserBioResult> GetUserBioDataAsync(string id);
        Task<UpdateUserResult> UpdateUserBioDataAsync(string id, string displayName, string email, string username);

    }
}   
