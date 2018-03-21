using AutoMapper;
using PrayashStore.Models;
using PrayashStore.ViewModels;

namespace PrayashStore.App_Start
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Address, AddressViewModel>().ReverseMap();


                //cfg.CreateMap<Address, ReviewOrderViewModel>()
                //         .ForMember(x => x.ShippingAddress, y => y.MapFrom(src => src))
                //         .ForMember(x => x.BillingAddress, y => y.MapFrom(src => src));

            });
        }
    }

}