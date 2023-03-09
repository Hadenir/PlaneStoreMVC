using AutoMapper;
using PlaneStore.WebUI.Utilities;

namespace PlaneStore.WebUI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile<MappingProfile>();
            });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
