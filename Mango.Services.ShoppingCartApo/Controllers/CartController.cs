using Mango.MessageBus;
using Mango.Services.ShoppingCartApi.Messages;
using Mango.Services.ShoppingCartApi.Models.Dtos;
using Mango.Services.ShoppingCartApi.RabbitMQSender;
using Mango.Services.ShoppingCartApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ShoppingCartApi.Controllers
{
    [Route("api/Cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICouponRepository  _couponRepository;
        private readonly IMessageBus  _messageBus;
        protected ResponseDto _response;
        private readonly IRabbitMQCartMessageSender _rabbitMQCartMessageSender;

        public CartController(ICartRepository cartRepository , IMessageBus messageBus, ICouponRepository couponRepository , IRabbitMQCartMessageSender rabbitMQCartMessageSender )
        {
            _cartRepository = cartRepository;
            _rabbitMQCartMessageSender = rabbitMQCartMessageSender;
            _messageBus = messageBus;
            this._response = new ResponseDto();
            _couponRepository = couponRepository;
        }
        [HttpGet("{userId}")]
        public async Task<object> GetCaret(string userId)
        {
            try
            {
                CartDto cart = await _cartRepository.GetCartByUseId(userId);
                _response.Result = cart;
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
        [HttpPost("AddCart")]
        public async Task<object> AddCart(CartDto cartDto)
        {
            try
            {
                CartDto cart = await _cartRepository.CreateUpdateCart(cartDto);
                _response.Result = cart;
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
        [HttpPut]
        public async Task<object> UpdateCaret(CartDto cartDto)
        {
            try
            {
                CartDto cart = await _cartRepository.CreateUpdateCart(cartDto);
                _response.Result = cart;
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
        [HttpDelete("{cartDetailsId}")]
        public async Task<object> RemoveCart(int cartDetailsId)
        {
            try
            {
                var isRemoved = await _cartRepository.DeleteCart(cartDetailsId);
                _response.Result = isRemoved;
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
        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody]CartDto  cart)
        {
            try
            {
                var isApplied = await _cartRepository.ApplyCoupon(cart.CartHeader.UserId , cart.CartHeader.CouponCode);
                _response.Result = isApplied;
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
        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] string userId)
        {
            try
            {
                var isRemoved = await _cartRepository.RemoveCoupon(userId);
                _response.Result = isRemoved;
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
        [HttpPost("Checkout")]
        public async Task<object> Checkout(CheckoutHeaferDto checkoutHeaderDto)
        {
            try
           {
                var cart = await _cartRepository.GetCartByUseId(checkoutHeaderDto.UserId);
                if (cart == null)
                {
                    return BadRequest();
                }
                if (!string.IsNullOrEmpty(checkoutHeaderDto.CouponCode))
                {
                    CouponDto coupon = await _couponRepository.GetCoupon(checkoutHeaderDto.CouponCode);
                    if(coupon.DiscountAmount != checkoutHeaderDto.DiscountTotal)
                    {
                        _response.IsSucess = false;
                        _response.ErrorMessages = new List<string>() { "Coupon price has changed, please review that" };
                        _response.DisplayMessage = "Coupon price has changed, please review that";
                        return _response;
                    }
                }
                checkoutHeaderDto.cartDetails = cart.CartDetails;
                // aZure Service Bus
                // await _messageBus.PublishMessage(checkoutHeaderDto, "checkoutqueue");

                //rabbitMQ
                _rabbitMQCartMessageSender.SendMessage(checkoutHeaderDto, "checkoutqueue");


                await _cartRepository.CleareCart(checkoutHeaderDto.UserId);
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
