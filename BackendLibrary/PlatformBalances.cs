using BackendLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackendLibrary
{
    public class PlatformBalances : IPlatformBalances
    {
        public Platform Platform { get; set; }
        public Dictionary<string, object> Balances { get; }
        public PlatformBalances(Dictionary<string, object> balances)
        {
            Balances = balances;
        }

        public PlatformBalances(Platform platform, Dictionary<string, object> balances)
        {
            Platform = platform;
            Balances = balances;
        }
    }
}
