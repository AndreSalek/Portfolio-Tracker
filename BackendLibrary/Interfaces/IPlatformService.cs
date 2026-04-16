using System;
using System.Collections.Generic;
using System.Text;

namespace BackendLibrary.Interfaces
{
    public interface IPlatformService
    {
        Task<IPlatformBalances> GetAccountBalances();
    }
}
