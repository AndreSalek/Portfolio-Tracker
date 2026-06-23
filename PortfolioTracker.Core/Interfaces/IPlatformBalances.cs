using PortfolioTracker.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Core.Interfaces
{
    public interface IPlatformBalances
    {
        Platform Platform { get; set; }
        Dictionary<string, object> Balances { get; }
    }
}
