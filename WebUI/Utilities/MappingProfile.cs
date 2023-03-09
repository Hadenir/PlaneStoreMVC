using PlaneStore.Application.Models;
using PlaneStore.Domain.Entities;
using PlaneStore.WebUI.Models;

namespace PlaneStore.WebUI.Utilities
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<CartLine, OrderLine>();
            CreateMap<OrderViewModel, Order>()
                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        }
    }
}
