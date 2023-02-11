using Mango.Services.CopounApi.Models.Dtos;

namespace Mango.Services.CopounApi.Repository
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCouponByCode(string copounCode);
    }
}
