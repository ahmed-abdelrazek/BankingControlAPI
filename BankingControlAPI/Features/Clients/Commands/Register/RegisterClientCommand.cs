using BankingControlAPI.Domain.Enums;
using MediatR;

namespace BankingControlAPI.Features.Clients.Commands.Register
{
    public record RegisterClientCommand : IRequest<string>
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required string PasswordConfirmation { get; init; }
        public required string MobileNumber { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string PersonalID { get; init; }
        public IFormFile? Photo { get; init; }
        public bool IsMale { get; init; }
        public RoleEnum Role { get; init; }
        public ClientAddressCommand? Address { get; init; }

        public required ICollection<string> Accounts { get; init; }
    }
}
