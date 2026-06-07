using PortfolioTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Core
{
    public class ExternalApiResponse : IExternalApiResponse
    {
        public object Result { get; set; }
        public string[] Error { get; set; }
        public ExternalApiResponse(object result, string[] error)
        {
            Result = result;
            Error = error;
        }
    }
}
