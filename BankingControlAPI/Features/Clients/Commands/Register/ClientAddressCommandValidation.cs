using FluentValidation;

namespace BankingControlAPI.Features.Clients.Commands.Register
{
    public class ClientAddressCommandValidation : AbstractValidator<ClientAddressCommand?>
    {
        public ClientAddressCommandValidation()
        {
            RuleFor(x => x!.Country).NotEmpty();
            RuleFor(x => x!.Country).MaximumLength(50);

            RuleFor(x => x!.City).NotEmpty();
            RuleFor(x => x!.City).MaximumLength(50);

            RuleFor(x => x!.Street).NotEmpty();
            RuleFor(x => x!.Street).MaximumLength(50);

            RuleFor(x => x!.ZipCode).NotEmpty();
            RuleFor(x => x!.ZipCode).MaximumLength(15);
        }
    }
}
