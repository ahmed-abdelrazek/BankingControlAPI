namespace BankingControlAPI.CustomExceptions
{
    public class ForbiddenActionException : Exception
    {
        public ForbiddenActionException()
        {
        }

        public ForbiddenActionException(string message)
            : base(message)
        {
        }
    }
}
