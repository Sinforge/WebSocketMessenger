using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSockerMessenger.Core.Exceptions
{
    public class SharedException : Exception
    {
        public readonly int StatusCode;

        public SharedException(string? Message, int statusCode): base (Message){ 
            StatusCode = statusCode;
        }
        

    }
}
