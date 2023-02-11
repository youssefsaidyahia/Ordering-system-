using Mango.Services.ShoppingCartApi.Models.Dtos;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartApi.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private HttpClient httpClient;

        public CouponRepository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<CouponDto> GetCoupon(string code) {
            var response = await httpClient.GetAsync($"api/Coupon/{code}");
            var apicontent=await response.Content.ReadAsStringAsync();  
            var result=JsonConvert.DeserializeObject<ResponseDto>(apicontent);
            if(result!=null && result.IsSucess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(result.Result));
            }
            return new CouponDto();
        }
    }
}
