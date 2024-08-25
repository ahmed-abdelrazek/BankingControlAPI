using BankingControlAPI.Features.Clients.DTOs;
using MediatR;

namespace BankingControlAPI.Features.Clients.Queries.List
{
    public class GetPagedClientsSuggestionsQuery : IRequest<IEnumerable<ClientSuggestionDto>>;
}
