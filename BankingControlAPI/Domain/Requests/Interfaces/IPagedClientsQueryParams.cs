using BankingControlAPI.Domain.Enums;

namespace BankingControlAPI.Domain.Requests.Interfaces
{
    public interface IPagedClientsQueryParams : IOrderParams<ClientOrderParams>
    {
        string? Email { get; init; }
        string? MobileNumber { get; init; }
        string? FirstName { get; init; }
        string? LastName { get; init; }
        string? PersonalID { get; init; }
        bool? IsMale { get; init; }
    }
}
