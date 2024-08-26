using MediatR;
using System.ComponentModel;

namespace BankingControlAPI.Features.Authorization.Commands.Register
{
    public record RegisterUserCommand : IRequest
    {
        [DefaultValue("user@example.com")]
        public required string Email { get; init; }
        [DefaultValue("pass123")]
        public required string Password { get; init; }
        [DefaultValue("pass123")]
        public required string PasswordConfirmation { get; init; }
        public int RoleId { get; init; }
    }
}
