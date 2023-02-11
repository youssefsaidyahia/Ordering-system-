using Mango.Web.Models.Dtos;
using Mango.Web.Services;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductsService productService;

        public ProductController(IProductsService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            List<ProductDto> ProductList = new();
            var Response = await productService.GetAllProductsAsync<ResponseDto>(accessToken);
            if (Response != null && Response.IsSucess)
            {
                ProductList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(Response.Result));
            }
            return View(ProductList);
        }
        
        public async Task<IActionResult> CreateProduct()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var Response = await productService.CreateProductAsyn<ResponseDto>(productDto, accessToken);
                if (Response != null && Response.IsSucess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(productDto);
        }
        public async Task<IActionResult> EditProduct(int Productid)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            ProductDto productDto = new ProductDto();
                var Response = await productService.GetProductByIdAsync<ResponseDto>(Productid, accessToken);
                if (Response != null && Response.IsSucess)
                {
                    productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(Response.Result));
                    return View(productDto);
                }
            return NotFound();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var Response = await productService.UpdateProductAsyn<ResponseDto>(productDto, accessToken);
                if (Response != null && Response.IsSucess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(productDto);
        }
        public async Task<IActionResult> DeleteProduct(int Productid)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            ProductDto productDto = new ProductDto();
            var Response = await productService.GetProductByIdAsync<ResponseDto>(Productid, accessToken);
            if (Response != null && Response.IsSucess)
            {
                productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(Response.Result));
                return View(productDto);
            }
            return NotFound();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var Response = await productService.DeleteProductAsyn<ResponseDto>(productDto.ProductId, accessToken);
                if ( Response.IsSucess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(productDto);
        }
    }
}
