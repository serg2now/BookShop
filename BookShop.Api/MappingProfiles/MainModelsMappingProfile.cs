using AutoMapper;
using BookShop.Api.DAL.Models;
using BookShop.Api.DTOs.Book;

namespace BookShop.Api.MappingProfiles
{
    public class MainModelsMappingProfile : Profile
    {
        public MainModelsMappingProfile()
        {
            CreateMap<Book, BookDto>()
                .ForMember(m => m.BookId, opt => opt.MapFrom(s => $"{s.Id}"))
                .ForMember(m => m.Cost, opt => opt.MapFrom(s => $"{s.Cost} uah."));

            CreateMap<BookDto, Book>()
                .ForMember(m => m.Cost, opt => opt.MapFrom(s => decimal.Parse(s.Cost)));

            CreateMap<Order, OrderDto>()
                .ForMember(m => m.UserFullName, opt => opt.MapFrom(s => $"{s.User.Surname} {s.User.Name} {s.User.MiddleName}".TrimEnd()))
                .ForMember(m => m.OrderDate, opt => opt.MapFrom(s => s.OrderDate.ToShortDateString()))
                .ForMember(m => m.OrderCost, opt => opt.MapFrom(s => $"{s.OrderCost} uah."))
                .ForMember(m => m.BookFullName, opt => opt.MapFrom(s => $"{s.Book.Author} {s.Book.Name}"));
        }
    }
}
