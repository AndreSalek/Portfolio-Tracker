using System;
using System.Collections.Generic;
using System.Text;

namespace BackendLibrary.Interfaces
{
    public interface IPlatformBalances
    {
        Platform Platform { get; set; }
        Dictionary<string, object> Balances { get; }
    }
}
