namespace BankingControlAPI.Domain.System
{
    public abstract class PaginationParameters
    {
        public int? PageNumber { get; init; }
        public int? PageSize { get; init; }

        public PaginationParameters()
        {
            PageNumber ??= 1;
            PageSize ??= 20;

            if (PageSize > 50)
            {
                PageSize = 50;
            }
        }
    }
}
