using Mango.Web.Models;
using Mango.Web.Models.Dtos;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductsService productService;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger, IProductsService productService , ICartService cartService)
        {
            _logger = logger;
            this.productService = productService;
            this._cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> ProductList = new();
            var Response = await productService.GetAllProductsAsync<ResponseDto>("");
            if (Response != null && Response.IsSucess)
            {
                ProductList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(Response.Result));
            }
            return View(ProductList);
        }
        [Authorize]
        public async Task<IActionResult> Details(int productId)
        {

            ProductDto productDto = new ProductDto();
            var Response = await productService.GetProductByIdAsync<ResponseDto>(productId, "");
            if (Response != null && Response.IsSucess)
            {
                productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(Response.Result));
                return View(productDto);
            }
            return NotFound();
        }
        [HttpPost]
        [ActionName("Details")]
        [Authorize]

        public async Task<IActionResult> DetailsPost(ProductDto productDto)
        {
            CartDto cartDto = new()
            {
                cartHeader = new CartHeaderDto
                {
                    UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value
                }
            };

            CartDetailsDto cartDetails = new CartDetailsDto()
            {
                Count = productDto.Count,
                ProductId = productDto.ProductId
            };

            var resp = await productService.GetProductByIdAsync<ResponseDto>(productDto.ProductId, "");
            if (resp != null && resp.IsSucess)
            {
                cartDetails.Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(resp.Result));
            }
            List<CartDetailsDto> cartDetailsDtos = new();
            cartDetailsDtos.Add(cartDetails);
            cartDto.cartDetails = cartDetailsDtos;

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var addToCartResp = await _cartService.AddCartAsync<ResponseDto>(cartDto, accessToken);
            if (addToCartResp != null && addToCartResp.IsSucess)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(productDto);
        }
        [Authorize]
        public IActionResult Login()
        {
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Logout()
        {
            return SignOut("Cookies","oidc");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}