using Application.UseCase.Customers;
using AutoMapper;
using Domain.Entities;

namespace Application.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Customer, CustomerResponseDto>().ReverseMap();
        }
    }
}
