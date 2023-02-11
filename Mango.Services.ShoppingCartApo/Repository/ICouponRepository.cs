using Mango.Services.ShoppingCartApi.Models.Dtos;

namespace Mango.Services.ShoppingCartApi.Repository
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCoupon(string couponName);
    }
}
