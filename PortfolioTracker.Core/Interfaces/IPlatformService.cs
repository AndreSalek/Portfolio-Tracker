using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Core.Interfaces
{
    public interface IPlatformService
    {
        Task<IPlatformBalances> GetAccountBalances();
    }
}
