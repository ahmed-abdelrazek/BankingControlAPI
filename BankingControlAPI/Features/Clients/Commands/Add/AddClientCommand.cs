using BankingControlAPI.Features.Clients.DTOs;
using MediatR;
using System.ComponentModel;

namespace BankingControlAPI.Features.Clients.Commands.Add
{
    public record AddClientCommand : IRequest<ClientDetailsDto>
    {
        [DefaultValue("user@example.com")]
        public required string Email { get; init; }
        [DefaultValue("201231236658")]
        public required string MobileNumber { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        [DefaultValue("12345678911")]
        public required string PersonalID { get; init; }
        public IFormFile? Photo { get; init; }
        public bool IsMale { get; init; }
        public ClientAddressCommand? Address { get; init; }

        public required ICollection<string> Accounts { get; init; }
    }
}
