using Mango.Web.Models.Dtos;

namespace Mango.Web.Services.IServices
{
    public interface ICartService
    {
        Task<T> GetCartBYUserIdAsync<T>(string userId, string token = null);
        Task<T> AddCartAsync<T>(CartDto cartDto, string token = null);
        Task<T> UpdateCartAsync<T>(CartDto cartDto, string token = null);
        Task<T> DeleteCartAsync<T>(int cartId, string token = null);
        Task<T> ApplyCoupon<T>(CartDto cartDto, string token = null);
        Task<T> RemoveCoupon<T>(string userId, string token = null);
        Task<T> Checkout<T>(CartHeaderDto cart, string token = null);
    }
}
