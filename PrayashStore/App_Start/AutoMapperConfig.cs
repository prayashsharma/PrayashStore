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
                cfg.CreateMap<Product, ProductDetailViewModel>();
                cfg.CreateMap<ProductAddFormViewModel, Product>()
                    .ForMember(dest => dest.Category, opt => opt.Ignore())
                    .ForMember(dest => dest.Thumbnail, opt => opt.Ignore())
                    .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                    .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category))
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => decimal.Parse(src.Price)));

                cfg.CreateMap<Product, ProductEditFormViewModel>()
                    .ForMember(dest => dest.Categories, opt => opt.Ignore())
                    .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                    .ForMember(dest => dest.Thumbnail, opt => opt.Ignore())
                    .ForMember(dest => dest.Images, opt => opt.Ignore())
                    .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.CategoryId));

                cfg.CreateMap<ProductEditFormViewModel, Product>()
                    .ForMember(dest => dest.Category, opt => opt.Ignore())
                    .ForMember(dest => dest.Thumbnail, opt => opt.Ignore())
                    .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                    .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category));

                cfg.CreateMap<Product, ProductDetailViewModel>()
                    .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                    .ForMember(dest => dest.CartItemCount, opt => opt.Ignore())
                    .ForMember(dest => dest.CartItemRecordId, opt => opt.Ignore());


                // This is one way to pass and map properties that are not present in the source
                //cfg.CreateMap<Product, ProductDetailViewModel>()
                //    .ForMember(dest => dest.CartItemCount,
                //               opt => opt.ResolveUsing((src, dest, destMember, resContext) =>
                //                    dest.CartItemCount = (int)resContext.Items["CartItemCount"]))
                //    .ForMember(dest => dest.ProductImages,
                //               opt => opt.ResolveUsing((src, dest, destMember, resContext) =>
                //                    dest.ProductImages = (IEnumerable<ProductImage>)resContext.Items["ProductImages"]))
                //    .ForMember(dest => dest.CartItemRecordId,
                //               opt => opt.ResolveUsing((src, dest, destMember, resContext) =>
                //                    dest.CartItemRecordId = (int)resContext.Items["CartItemRecordId"]));

            });
        }
    }

}