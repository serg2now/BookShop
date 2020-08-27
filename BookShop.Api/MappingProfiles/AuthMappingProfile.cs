using AutoMapper;
using BookShop.Api.DAL.Models.Auth;
using BookShop.Api.DTOs.Auth;
using System;

namespace BookShop.Api.MappingProfiles
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<UserRegisterForm, User>();

            CreateMap<User, UserProfileDto>()
                .ForMember(m => m.FullName, opt => opt.MapFrom(s => $"{s.Surname} {s.Name} {s.MiddleName}".TrimEnd()))
                .ForMember(m => m.FullDeliveryAdress, opt => opt.MapFrom(s => $"{s.City} {s.DeliveryAdress}"))
                .ForMember(m => m.Age, opt => opt.MapFrom(s => CalculateAge(s)));
        }

        private int CalculateAge(User user)
        {
            var currentDate = DateTime.Today;
            var age = DateTime.Today.Year - user.BirthDate.Year;

            return (currentDate.DayOfYear >= user.BirthDate.DayOfYear) ? age : age - 1;
        }
    }
}
