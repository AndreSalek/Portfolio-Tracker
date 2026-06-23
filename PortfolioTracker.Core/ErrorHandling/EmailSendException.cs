using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Core.ErrorHandling
{
    public class EmailSendException : Exception
    {
        public EmailSendException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
