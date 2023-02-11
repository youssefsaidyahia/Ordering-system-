using AutoMapper;
using Mango.Services.ProductApi.Dtos;
using Mango.Services.ProductApi.Models;

namespace Mango.Services.ProductApi.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}
