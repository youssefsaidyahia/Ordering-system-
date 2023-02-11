using Mango.Web.Models.Dtos;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductsService _producService;
        private readonly ICartService _cartService;
        private readonly ICouponService _couponService;

        public CartController(IProductsService producService, ICartService cartService, ICouponService couponService)
        {
            _producService = producService;
            _cartService = cartService;
            _couponService = couponService; 
        }

        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartForALoggedInUser());
        }
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartForALoggedInUser());
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CartDto cart)
        {
            try{
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var Response = await _cartService.Checkout<ResponseDto>(cart.cartHeader, accessToken);
                if (!Response.IsSucess)
                {
                    TempData["Error"] = Response.DisplayMessage;
                    return RedirectToAction(nameof(Checkout));
                }
                return RedirectToAction(nameof(Confirmation));
            }
            catch(Exception )
            {
                return View(cart);
            }
        }
        public  IActionResult Confirmation()
        {
            return View();
        }
        [HttpPost]
        [ActionName("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {

            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var Response = await _cartService.ApplyCoupon<ResponseDto>(cartDto, accessToken);

            if (Response != null && Response.IsSucess)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }
        [HttpPost]
        [ActionName("RemoveCoupon")]
        public async Task<IActionResult> RemoveCoupon()
        {

            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var Response = await _cartService.RemoveCoupon<ResponseDto>(userId, accessToken);

            if (Response != null && Response.IsSucess)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }
        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var Response = await _cartService.DeleteCartAsync<ResponseDto>(cartDetailsId, accessToken);

            if (Response != null && Response.IsSucess)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }
        private async Task<CartDto> LoadCartForALoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var Response = await _cartService.GetCartBYUserIdAsync<ResponseDto>(userId, accessToken);
            CartDto cart = new CartDto(); 
            if(Response !=null && Response.IsSucess)
            {
                cart = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(Response.Result));
            }
            if(cart.cartHeader != null)
            {
                if (!string.IsNullOrEmpty(cart.cartHeader.CouponCode))
                {
                    var result = await _couponService.GetCouponc<ResponseDto>(cart.cartHeader.CouponCode, accessToken);
                    if (result != null && result.IsSucess)
                    {
                        var code = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(result.Result));
                        cart.cartHeader.DiscountTotal = code.DiscountAmount;

                    }
                }
                foreach(var item in cart.cartDetails)
                {
                    cart.cartHeader.OrderTotal += item.Count * item.Product.Price;
                }
                cart.cartHeader.OrderTotal -= cart.cartHeader.DiscountTotal;
            }
            return cart;
        }
    }
}
