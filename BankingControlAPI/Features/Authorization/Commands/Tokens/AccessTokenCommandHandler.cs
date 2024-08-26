using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BankingControlAPI.Features.Authorization.Commands.Tokens
{
    internal sealed class AccessTokenCommandHandler(UserManager<IdentityUser> UserManager, SignInManager<IdentityUser> SignInManager) : IRequestHandler<AccessTokenCommand>
    {
        public async Task Handle(AccessTokenCommand request, CancellationToken cancellationToken)
        {
            // Could be true if you use cookie and not bearer
            bool isPersistent = false;

            var user = await UserManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                throw new InvalidOperationException("Email or Password is wrong.");
            }

            var result = await SignInManager.CheckPasswordSignInAsync(user, request.Password, true);

            if (result.RequiresTwoFactor)
            {
                if (!string.IsNullOrEmpty(request.TwoFactorCode))
                {
                    result = await SignInManager.TwoFactorAuthenticatorSignInAsync(request.TwoFactorCode, isPersistent: isPersistent, rememberClient: isPersistent);
                }
                else if (!string.IsNullOrEmpty(request.TwoFactorRecoveryCode))
                {
                    result = await SignInManager.TwoFactorRecoveryCodeSignInAsync(request.TwoFactorRecoveryCode);
                }
            }

            if (!result.Succeeded)
            {
                if (result.IsNotAllowed)
                {
                    throw new InvalidOperationException("User is not allowed maybe you need to confirm email first.");
                }

                if (result.IsLockedOut)
                {
                    throw new InvalidOperationException("User is locked out try again later.");
                }

                throw new InvalidOperationException("Email or Password is wrong.");
            }

            SignInManager.AuthenticationScheme = IdentityConstants.BearerScheme;
            await SignInManager.SignInAsync(user, isPersistent);
        }
    }
}
