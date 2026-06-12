using PortfolioTracker.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;  


namespace PortfolioTracker.Core.Interfaces
{
    public interface IUserService
    {
        Task<RegistrationResult> RegisterAsync(string username, string email, string password, string displayName, DateTime dateOfBirth);
    }
}
