using AutoMapper;
using BankingControlAPI.CustomExceptions;
using BankingControlAPI.Data;
using BankingControlAPI.Features.Clients.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingControlAPI.Features.Clients.Queries.One
{
    internal class GetClientDetailsByIdQueryHandler(BankingDbContext dbContext, IMapper mapper) : IRequestHandler<GetClientDetailsByIdQuery, ClientDetailsDto>
    {
        public async Task<ClientDetailsDto> Handle(GetClientDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await dbContext.Clients.AsNoTracking()
                .Include(x => x.Accounts)
                .FirstOrDefaultAsync(x => x.Id == request.ClientId, cancellationToken: cancellationToken);

            if (data is null)
            {
                throw new NotFoundException($"Client with Id '{request.ClientId}' does not exist.");
            }

            return mapper.Map<ClientDetailsDto>(data);
        }
    }
}
