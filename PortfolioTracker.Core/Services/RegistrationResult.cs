using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Core.Services
{
    public class RegistrationResult
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; } = [];

        
        public static RegistrationResult Success()
            => new RegistrationResult { Succeeded = true };

        public static RegistrationResult Failure(IEnumerable<string> errors)
            => new RegistrationResult { Succeeded = false, Errors = errors };
    }
}
