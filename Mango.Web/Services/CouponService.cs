using Mango.Web.Models;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services
{
    public class CouponService : BaseService, ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        public async Task<T> GetCouponc<T>(string CouponCode, string token = null)
        {

            return await this.SendASync<T>(new APIRequest()
            {
                apiType = SD.ApiType.GET,
                url = SD.CouponApiBase + "/api/Coupon/" + CouponCode,
                AccessToken = token
            });
        }
    }
}
