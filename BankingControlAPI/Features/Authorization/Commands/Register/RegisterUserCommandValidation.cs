using BankingControlAPI.Domain.Enums;
using FluentValidation;

namespace BankingControlAPI.Features.Authorization.Commands.Register
{
    public class RegisterUserCommandValidation : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidation()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();

            RuleFor(x => x.Password).Length(6, 25);
            RuleFor(x => x.Password).Equal(x => x.PasswordConfirmation)
                .WithMessage("'Password' does not match.");

            RuleFor(x => x.RoleId).InclusiveBetween((int)RoleEnum.Client, (int)RoleEnum.Admin);
        }
    }
}
