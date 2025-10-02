using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yume.Exceptions.Auth
{
    public class AuthMfaException : Exception
    {
        public AuthMfaException() : base("Mfa threw an error.")
        {
        }

        public AuthMfaException(string message) : base(message)
        {
        }

        public AuthMfaException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
