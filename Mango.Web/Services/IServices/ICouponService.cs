using Mango.Web.Models.Dtos;

namespace Mango.Web.Services.IServices
{
    public interface ICouponService
    {
        Task<T> GetCouponc<T>(string CouponCode, string token = null);
    }
}
