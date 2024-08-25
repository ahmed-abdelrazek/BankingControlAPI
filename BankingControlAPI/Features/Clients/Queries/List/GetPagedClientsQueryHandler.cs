using AutoMapper;
using AutoMapper.QueryableExtensions;
using BankingControlAPI.Data;
using BankingControlAPI.Domain.Entites;
using BankingControlAPI.Domain.Responses;
using BankingControlAPI.Features.Clients.DTOs;
using BankingControlAPI.Features.Clients.Events;
using BankingControlAPI.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingControlAPI.Features.Clients.Queries.List
{
    internal class GetPagedClientsQueryHandler(IHttpContextAccessor HttpContextAccessor, BankingDbContext dbContext, IMapper mapper, IBackgroundTaskQueue TaskQueue) : IRequestHandler<GetPagedClientsQuery, PagedList<ClientPagedListDto>>
    {
        public async Task<PagedList<ClientPagedListDto>> Handle(GetPagedClientsQuery request, CancellationToken cancellationToken)
        {
            var query = dbContext.Users.AsNoTracking();
            query = BuildPredicate(query, request);

            if (request.OrderProperty is not Domain.Enums.ClientOrderParams.None)
            {
                if (request.OrderIsDesc)
                {
                    query = query.OrderByDescending(x => EF.Property<object>(x, request.OrderProperty.ToString()));
                }
                else
                {
                    query = query.OrderBy(x => EF.Property<object>(x, request.OrderProperty.ToString()));
                }
            }

            var finalQuery = query.ProjectTo<ClientPagedListDto>(mapper.ConfigurationProvider);

            // publish the filtering and sorting params to save and retrive for admin later
            await TaskQueue.QueueEventAsync(mapper.Map<SavePagedClientsParamsEvent>(request));

            // page number and size won't be null here because it will get set with default values in ctor if they were null
            var pagedData = await PagedList<ClientPagedListDto>.ToPagedListAsync(finalQuery, request.PageNumber!.Value, request.PageSize!.Value);

            // add pagniation meta data to response headers
            HttpContextAccessor.HttpContext!.Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(new
            {
                pagedData.TotalCount,
                pagedData.PageSize,
                pagedData.CurrentPage,
                pagedData.TotalPages
            }));

            return pagedData;
        }

        private static IQueryable<Client> BuildPredicate(IQueryable<Client> query, GetPagedClientsQuery request)
        {
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                query = query.Where(x => EF.Functions.Like(x.Email, $"%{request.Email}%"));
            }

            if (!string.IsNullOrWhiteSpace(request.PersonalID))
            {
                query = query.Where(x => EF.Functions.Like(x.PhoneNumber, $"%{request.PersonalID}%"));
            }

            if (!string.IsNullOrWhiteSpace(request.FirstName))
            {
                query = query.Where(x => EF.Functions.Like(x.FirstName, $"%{request.FirstName}%"));
            }

            if (!string.IsNullOrWhiteSpace(request.LastName))
            {
                query = query.Where(x => EF.Functions.Like(x.LastName, $"%{request.LastName}%"));
            }

            if (!string.IsNullOrWhiteSpace(request.PersonalID))
            {
                query = query.Where(x => EF.Functions.Like(x.PersonalID, $"%{request.PersonalID}%"));
            }

            if (request.IsMale.HasValue)
            {
                query = query.Where(x => x.IsMale == request.IsMale);
            }

            return query;
        }
    }
}
