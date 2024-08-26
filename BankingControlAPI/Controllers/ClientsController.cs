using BankingControlAPI.Domain.Enums;
using BankingControlAPI.Domain.Responses;
using BankingControlAPI.Features.Clients.Commands.Add;
using BankingControlAPI.Features.Clients.DTOs;
using BankingControlAPI.Features.Clients.Queries.List;
using BankingControlAPI.Features.Clients.Queries.One;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingControlAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Admin))]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController(IMediator Mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedList<ClientPagedListDto>>> GetPaged([FromQuery] GetPagedClientsQuery query)
        {
            var pagedData = await Mediator.Send(query);
            return Ok(pagedData);
        }

        [HttpGet("PagedSuggestions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ClientSuggestionDto>>> GetPagedSuggestions()
        {
            var data = await Mediator.Send(new GetPagedClientsSuggestionsQuery());
            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClientDetailsDto>> GetById(Guid id)
        {
            var data = await Mediator.Send(new GetClientDetailsByIdQuery(id));
            return Ok(data);
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClientDetailsDto>> Add([FromForm] AddClientCommand command)
        {
            var data = await Mediator.Send(command);
            return Ok(data);
        }
    }
}
