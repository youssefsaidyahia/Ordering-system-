using AutoMapper;
using Mango.Services.ShoppingCartApi.Models;
using Mango.Services.ShoppingCartApi.Models.Dtos;

namespace Mango.Services.ShoppingCartApi.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartDto, Cart>();
            CreateMap<Cart, CartDto>();
            CreateMap<CartHeaderDto, CartHeader>();
            CreateMap<CartHeader, CartHeaderDto>();
            CreateMap<CartDetails, CartDetailsDto>();
            CreateMap<CartDetailsDto, CartDetails>();
        }
    }
}
