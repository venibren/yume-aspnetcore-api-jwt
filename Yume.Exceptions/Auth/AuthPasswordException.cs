namespace Yume.Exceptions.Auth
{
    public class AuthPasswordException : Exception
    {
        public AuthPasswordException() : base("Invalid password.")
        {
        }

        public AuthPasswordException(string message) : base(message)
        {
        }

        public AuthPasswordException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
