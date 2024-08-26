using AutoMapper;
using BankingControlAPI.Features.Authorization.Commands.Register;
using Microsoft.AspNetCore.Identity;

namespace BankingControlAPI.MappingProfiles
{
    public class IdentityUsersMappingProfile : Profile
    {
        public IdentityUsersMappingProfile()
        {
            CreateMap<RegisterUserCommand, IdentityUser>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email))
                .ForMember(x => x.EmailConfirmed, opt => opt.MapFrom(x => true));
        }
    }
}
