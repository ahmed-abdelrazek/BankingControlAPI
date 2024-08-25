using BankingControlAPI.Data;
using BankingControlAPI.Domain.Enums;
using BankingControlAPI.Features.Clients.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingControlAPI.Features.Clients.Queries.List
{
    internal class GetPagedClientsSuggestionsQueryHandler(BankingDbContext dbContext) : IRequestHandler<GetPagedClientsSuggestionsQuery, IEnumerable<ClientSuggestionDto>>
    {
        public async Task<IEnumerable<ClientSuggestionDto>> Handle(GetPagedClientsSuggestionsQuery request, CancellationToken cancellationToken)
        {
            var queryData = await dbContext.FiltersParameters.AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Take(3)
                .Where(x => x.From == FiltersParamsId.ClientPagedFilter)
                .Select(x => x.Content)
                .ToListAsync(cancellationToken: cancellationToken);

            List<ClientSuggestionDto> data = [];

            foreach (var jsonString in queryData)
            {
                data.Add(System.Text.Json.JsonSerializer.Deserialize<ClientSuggestionDto>(jsonString)!);
            }

            return data;
        }
    }
}
