using System;
using System.Collections.Generic;
using System.Text;


namespace PortfolioTracker.Core.Models
{
    public class UserBioResult
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; } = [];

        public UserBio? Data { get; set; }


        public static UserBioResult Success(UserBio userBio)
            => new UserBioResult { Succeeded = true, Data = userBio };

        public static UserBioResult Failure(IEnumerable<string> errors)
            => new UserBioResult { Succeeded = false, Errors = errors };
    }
}
