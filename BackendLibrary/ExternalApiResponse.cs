using System;
using System.Collections.Generic;
using System.Text;

namespace BackendLibrary
{
    public class ExternalApiResponse
    {
        public object Result { get; set; }
        public string[] error { get; set; }
        public ExternalApiResponse(object result, string[] error)
        {
            Result = result;
            this.error = error;
        }
    }
}
