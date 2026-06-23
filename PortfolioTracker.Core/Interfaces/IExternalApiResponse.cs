using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Core.Interfaces
{
    public interface IExternalApiResponse
    {
        public object Result { get; set; }
        public string[] Error { get; set; }
    }
}
