using AutoMapper;
using Mango.Services.ShoppingCartApi.Models;
using Mango.Services.ShoppingCartApi.Models.Dtos;

namespace Mango.Services.ShoppingCartApi.Profiles
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
