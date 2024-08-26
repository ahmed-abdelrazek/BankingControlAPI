using MediatR;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BankingControlAPI.Features.Authorization.Commands.Tokens
{
    internal sealed class RefreshTokenCommandHandler(SignInManager<IdentityUser> SignInManager, IOptionsMonitor<BearerTokenOptions> BearerTokenOptions, TimeProvider TimeProvider) : IRequestHandler<RefreshTokenCommand, EmptyHttpResult>
    {
        public async Task<EmptyHttpResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshTokenProtector = BearerTokenOptions.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
            var refreshTicket = refreshTokenProtector.Unprotect(request.RefreshToken);

            if (refreshTicket is null)
            {
                throw new UnauthorizedAccessException("Couldn't understand the refresh token.");
            }

            // Reject the /refresh attempt with a 401 if the token expired or the security stamp validation fails
            if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc || TimeProvider.GetUtcNow() >= expiresUtc ||
                await SignInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not IdentityUser user)
            {
                throw new UnauthorizedAccessException("Refresh token expired or couldn't be validated.");
            }

            SignInManager.AuthenticationScheme = IdentityConstants.BearerScheme;
            await SignInManager.SignInWithClaimsAsync(user, false, refreshTicket.Principal.Claims);

            return TypedResults.Empty;
        }
    }
}
