using AutoMapper;
using PlaneStore.Application.Models;
using PlaneStore.Domain.Entities;
using PlaneStore.WebUI.Areas.Admin.Models;
using PlaneStore.WebUI.Models;

namespace PlaneStore.WebUI.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ManufacturerViewModel, Manufacturer>()
                .ReverseMap();
            CreateMap<AircraftViewModel, Aircraft>()
                .ReverseMap()
                    .ForMember(
                        dest => dest.Price,
                        opt => opt.MapFrom(src => src.Manufacturer.Id));

            CreateMap<CartLine, OrderLine>();
            CreateMap<OrderViewModel, Order>()
                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        }
    }
}
