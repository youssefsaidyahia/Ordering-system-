using Mango.Services.ShoppingCartApi.Models.Dtos;

namespace Mango.Services.ShoppingCartApi.Repository
{
    public interface ICartRepository
    {
       Task<CartDto> GetCartByUseId(string userId);
       Task<CartDto> CreateUpdateCart(CartDto cartDto);
       Task<bool> ApplyCoupon(string userId , string couponCode);
       Task<bool> RemoveCoupon(string userId);
       Task<bool> DeleteCart(int cartDetailsId);
       Task<bool> CleareCart(string userId);
    }
}
