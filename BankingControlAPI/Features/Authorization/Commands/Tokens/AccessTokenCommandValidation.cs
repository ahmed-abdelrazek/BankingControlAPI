using FluentValidation;

namespace BankingControlAPI.Features.Authorization.Commands.Tokens
{
    public sealed class AccessTokenCommandValidation : AbstractValidator<AccessTokenCommand>
    {
        public AccessTokenCommandValidation()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
