using AutoMapper;
using BankingControlAPI.CustomExceptions;
using BankingControlAPI.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BankingControlAPI.Features.Authorization.Commands.Register
{
    internal sealed class RegisterUserCommandHandler(UserManager<IdentityUser> UserManager, IMapper Mapper) : IRequestHandler<RegisterUserCommand>
    {
        public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = Mapper.Map<RegisterUserCommand, IdentityUser>(request);

            var userRslt = await UserManager.CreateAsync(user, request.Password);

            if (!userRslt.Succeeded)
            {
                throw new IdentityStandardException("Could not create new user.", userRslt.Errors);
            }

            var roleRslt = await UserManager.AddToRoleAsync(user, ((RoleEnum)request.RoleId).ToString());
            if (!roleRslt.Succeeded)
            {
                throw new IdentityStandardException("Could not add role to the new user.", roleRslt.Errors);
            }
        }
    }
}
