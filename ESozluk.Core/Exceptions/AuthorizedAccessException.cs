using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Core.Exceptions
{
    public class AuthorizedAccessException : Exception
    {
        public AuthorizedAccessException(string message) : base(message)
        {
       
        }
    }
}
