using BankingControlAPI.Features.Authorization.Commands.Register;
using BankingControlAPI.Features.Authorization.Commands.Tokens;
using MediatR;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BankingControlAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController(IMediator Mediator) : ControllerBase
    {
        [HttpPost("~/Connect/Token")]
        [ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<EmptyHttpResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ValidationProblem>(StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<AccessTokenResponse>, ValidationProblem, EmptyHttpResult>> Auth([FromBody] AccessTokenCommand command, CancellationToken cancellationToken)
        {
            await Mediator.Send(command, cancellationToken);

            // The signInManager already produced the needed response in the form of a cookie or bearer token.
            return TypedResults.Empty;
        }

        [HttpPost("~/Connect/Refresh")]
        [ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<EmptyHttpResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ValidationProblem>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<UnauthorizedHttpResult>(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult, ValidationProblem, EmptyHttpResult>> Refresh(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            await Mediator.Send(command, cancellationToken);

            return TypedResults.Empty;
        }

        [HttpPost("~/Users/Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<ValidationProblem>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            await Mediator.Send(command, cancellationToken);

            return Ok();
        }
    }
}
