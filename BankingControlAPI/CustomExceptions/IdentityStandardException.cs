using Microsoft.AspNetCore.Identity;

namespace BankingControlAPI.CustomExceptions
{
    [Serializable]
    public class IdentityStandardException : Exception
    {
        public readonly IEnumerable<IdentityError>? Errors;

        public IdentityStandardException()
        {
        }

        public IdentityStandardException(string message) : base(message)
        {
        }

        public IdentityStandardException(string message, IEnumerable<IdentityError> errors) : base(message)
        {
            Errors = errors;
        }

        public IdentityStandardException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
