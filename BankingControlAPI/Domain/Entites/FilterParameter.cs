using BankingControlAPI.Domain.Enums;

namespace BankingControlAPI.Domain.Entites
{
    public class FilterParameter
    {
        public Guid Id { get; set; }
        public required FiltersParamsId From { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
