using BankingControlAPI.Features.Clients.DTOs;
using MediatR;

namespace BankingControlAPI.Features.Clients.Queries.One
{
    public record GetClientDetailsByIdQuery(Guid ClientId) : IRequest<ClientDetailsDto>;
}
