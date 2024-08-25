using BankingControlAPI.Domain.Enums;
using BankingControlAPI.Domain.Requests.Interfaces;
using BankingControlAPI.Domain.Responses;
using BankingControlAPI.Domain.System;
using BankingControlAPI.Features.Clients.DTOs;
using MediatR;

namespace BankingControlAPI.Features.Clients.Queries.List
{
    public class GetPagedClientsQuery : PaginationParameters, IPagedClientsQueryParams, IRequest<PagedList<ClientPagedListDto>>
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
