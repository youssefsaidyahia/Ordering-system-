using Mango.Services.CopounApi.Models.Dtos;
using Mango.Services.CopounApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CopounApi.Controllers
{
    [Route("api/Coupon")]
    [ApiController]
    public class CopounController : Controller
    {
        private readonly ICouponRepository _couponRepository;
        protected ResponseDto _response;

        public CopounController(ICouponRepository  couponRepository)
        {
            _couponRepository = couponRepository;
            this._response = new ResponseDto();
        }
        [HttpGet("{code}")]
        public async Task<object> GetDiscount(string code)
        {
            try
            {
                CouponDto coupon = await _couponRepository.GetCouponByCode(code);
                _response.Result = coupon;
            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }
            return _response;
        }
    }
}
