using AutoMapper;
using BankingControlAPI.Domain.Entites;
using BankingControlAPI.Domain.Requests.Interfaces;
using BankingControlAPI.Features.Clients.DTOs;
using BankingControlAPI.Features.Clients.Events;

namespace BankingControlAPI.MappingProfiles
{
    public class ClientsMappingProfile : Profile
    {
        public ClientsMappingProfile()
        {
            CreateMap<Client, ClientPagedListDto>()
                .ForMember(x => x.PersonalID, opt => opt.MapFrom(x => x.PersonalID))
                .ForMember(x => x.ProfilePhoto, opt => opt.MapFrom(x => x.AvatarPath))
                .ForMember(x => x.MobileNumber, opt => opt.MapFrom(x => string.IsNullOrEmpty(x.PhoneNumber) ? string.Empty : x.PhoneNumber))
                .ForMember(x => x.Sex, opt => opt.MapFrom(x => x.IsMale ? "Male" : "Female"))
                .ForMember(x => x.Address, opt => opt.MapFrom(x => BuildAddress(x.Address)));

            CreateMap<IPagedClientsQueryParams, SavePagedClientsParamsEvent>();
        }

        private static string? BuildAddress(Address? address)
        {
            if (address is null)
            {
                return null;
            }

            return $"{address.Country}, {address.City}, {address.Street}, {address.ZipCode}";
        }
    }
}
