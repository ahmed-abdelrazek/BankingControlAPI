using BankingControlAPI.Domain.Enums;
using BankingControlAPI.Domain.Requests.Interfaces;
using MediatR;

namespace BankingControlAPI.Features.Clients.Events
{
    public class SavePagedClientsParamsEvent : IPagedClientsQueryParams, INotification
    {
        public string? Email { get; init; }
        public string? MobileNumber { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? PersonalID { get; init; }
        public bool? IsMale { get; init; }
        public ClientOrderParams OrderProperty { get; init; }
        public bool OrderIsDesc { get; init; }
    }
}
