using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Core.Models
{
    public static class AuthErrors
    {
        public const string InvalidCredentials = "Invalid username or password.";
        public const string AccountLocked = "Account is locked. Try again later.";
        public const string EmailNotConfirmed = "Please confirm your email first.";

    }
}