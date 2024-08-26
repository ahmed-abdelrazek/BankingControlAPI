using AutoMapper;
using BankingControlAPI.Domain.Entites;
using BankingControlAPI.Domain.Requests.Interfaces;
using BankingControlAPI.Features.Clients.Commands.Add;
using BankingControlAPI.Features.Clients.DTOs;
using BankingControlAPI.Features.Clients.Events;
using PhoneNumbers;

namespace BankingControlAPI.MappingProfiles
{
    public class ClientsMappingProfile : Profile
    {
        public ClientsMappingProfile()
        {
            CreateMap<Client, ClientPagedListDto>()
                .ForMember(x => x.PhotoRelativeUrl, opt => opt.MapFrom(x => x.PhotoPath))
                .ForMember(x => x.MobileNumber, opt => opt.MapFrom(x => FormatNumber(x.MobileNumber)))
                .ForMember(x => x.Sex, opt => opt.MapFrom(x => x.IsMale ? "Male" : "Female"))
                .ForMember(x => x.Address, opt => opt.MapFrom(x => BuildAddress(x.Address)));

            CreateMap<ClientAccount, ClientAccountDto>();

            CreateMap<Client, ClientDetailsDto>()
                .ForMember(x => x.PhotoRelativeUrl, opt => opt.MapFrom(x => x.PhotoPath))
                .ForMember(x => x.MobileNumber, opt => opt.MapFrom(x => FormatNumber(x.MobileNumber)))
                .ForMember(x => x.Sex, opt => opt.MapFrom(x => x.IsMale ? "Male" : "Female"))
                .ForMember(x => x.Address, opt => opt.MapFrom(x => BuildAddress(x.Address)));

            CreateMap<IPagedClientsQueryParams, SavePagedClientsParamsEvent>();

            CreateMap<AddClientCommand, Client>();

            CreateMap<string, ClientAccount>()
                .ConvertUsing((src, dest, context) => new ClientAccount { Name = src });

            CreateMap<ClientAddressCommand?, Address?>()
                .ConvertUsing((src, dest, context) => src == null || (string.IsNullOrWhiteSpace(src.Country) && string.IsNullOrWhiteSpace(src.City) && string.IsNullOrWhiteSpace(src.Street) && string.IsNullOrWhiteSpace(src.ZipCode)) ? null : new Address { Country = src.Country!, City = src.City!, Street = src.Street!, ZipCode = src.ZipCode! });
        }

        private static string? BuildAddress(Address? address)
        {
            if (address is null)
            {
                return null;
            }

            return $"{address.Country}, {address.City}, {address.Street}, {address.ZipCode}";
        }

        private static string FormatNumber(string? number)
        {
            if (number is null)
            {
                return string.Empty;
            }

            try
            {
                var phoneNumberUtil = PhoneNumberUtil.GetInstance();
                var phoneNumber = phoneNumberUtil.Parse(number, null);
                return phoneNumberUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL).Replace(" ", "-");
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
