using FluentValidation;

namespace BankingControlAPI.Features.Authorization.Commands.Tokens
{
    public sealed class RefreshTokenCommandValidation : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidation()
        {
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
