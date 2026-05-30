using System;
using System.Collections.Generic;
using System.Text;

namespace BackendLibrary.Interfaces
{
    public interface IExternalApiResponse
    {
        public object Result { get; set; }
        public string[] Error { get; set; }
    }
}
