using AutoMapper;
using Challenge.API.ViewModels;
using Challenge.Domain.DTOs;
using Microsoft.Extensions.DependencyInjection;

namespace Challenge.API.Configuration
{
    public static class AutoMapperConfig
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(mce =>
            {
                mce.CreateMap<DependentDTO, DependentViewModel>().ReverseMap();
                mce.CreateMap<FamilyDataDTO, FamilyDataViewModel>().ReverseMap();
                mce.CreateMap<PersonDTO, PersonViewModel>().ReverseMap();
                mce.CreateMap<SpouseDTO, SpouseViewModel>().ReverseMap();
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}