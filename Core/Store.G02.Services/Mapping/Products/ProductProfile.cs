using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.G02.Domain.Entities.Products;
using Store.G02.Shared.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Mapping.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile(IConfiguration configuration)
        {
            CreateMap<Product, ProductResponse>()
                .ForMember(D => D.Brand, O => O.MapFrom(S => S.Brand.Name))
                .ForMember(D => D.Type, O => O.MapFrom(S => S.Type.Name))
                //.ForMember(D => D.PictureUrl, o => o.MapFrom(s => $"c{configuration["BaseUrl"]}/{s.PictureUrl}"));
                .ForMember(D => D.PictureUrl, o => o.MapFrom(new ProductPictureUrlResolver(configuration)));

            CreateMap<ProductBrand, BrandTypeResponse>();
            CreateMap<ProductType, BrandTypeResponse>();

        }
    }
}
