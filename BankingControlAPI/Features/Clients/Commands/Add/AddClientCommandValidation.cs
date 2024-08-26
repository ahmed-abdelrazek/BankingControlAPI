using FluentValidation;
using PhoneNumbers;

namespace BankingControlAPI.Features.Clients.Commands.Add
{
    public class AddClientCommandValidation : AbstractValidator<AddClientCommand>
    {
        public AddClientCommandValidation()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();

            RuleFor(x => x.MobileNumber).NotEmpty();
            RuleFor(x => x.MobileNumber).Must(IsValidPhoneNumber)
                .WithMessage("'Phone Number' is not valid number.");

            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.FirstName).MaximumLength(60);

            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.LastName).MaximumLength(60);

            RuleFor(x => x.PersonalID).NotEmpty();
            RuleFor(x => x.PersonalID).Length(11);

            RuleFor(x => x.Address).SetValidator(new ClientAddressCommandValidation())
                .When(x => x.Address is { }
                && (!string.IsNullOrEmpty(x.Address.Country)
                || !string.IsNullOrEmpty(x.Address.City)
                || !string.IsNullOrEmpty(x.Address.Street)
                || !string.IsNullOrEmpty(x.Address.ZipCode)));

            RuleFor(x => x.Accounts).Must(AccountsHaveNames).When(x => x.Accounts is { })
                .WithMessage("'Accounts' must have name with length from 1 to 60.").DependentRules(() =>
                {
                    RuleFor(x => x.Accounts).Must(x => x.Count > 0).When(x => x.Accounts is { })
                        .WithMessage("'Accounts' must have at least one.").DependentRules(() =>
                        {
                            RuleFor(x => x.Accounts).Must(x => x is { })
                                .WithMessage("'Accounts' must have at least one.");
                        }); ;
                });
        }

        private bool IsValidPhoneNumber(string number)
        {
            if (!number.StartsWith('+'))
            {
                number = $"+{number}";
            }

            try
            {
                var phoneNumberUtil = PhoneNumberUtil.GetInstance();
                var phoneNumber = phoneNumberUtil.Parse(number, null);
                return phoneNumberUtil.IsValidNumber(phoneNumber);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool AccountsHaveNames(ICollection<string> accounts)
        {
            return accounts.All(x => x != null && x.Length > 0 && x.Length <= 60);
        }
    }
}
