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
            CreateMap<Manufacturer, ManufacturerViewModel>()
                .ReverseMap();
            CreateMap<Aircraft, AircraftViewModel>()
                .ReverseMap();

            CreateMap<CartLine, OrderLine>();
            CreateMap<OrderViewModel, Order>()
                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        }
    }
}
