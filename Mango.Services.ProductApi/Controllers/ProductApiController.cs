using Mango.Services.ProductApi.Dtos;
using Mango.Services.ProductApi.Models.Dtos;
using Mango.Services.ProductApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductApi.Controllers
{
    [Route("api/Products")]
    [Authorize]
    public class ProductApiController : Controller
    {
        protected ResponseDto _response;
        private IProductRepository _productRepository;

        public ProductApiController(IProductRepository productRepository)
        {
            this._response = new ResponseDto();
            _productRepository = productRepository;
        }
        [AllowAnonymous]
        [HttpGet]
       
        public async Task<object> GetProducts()
        {
            try
            {
                var ProductList=await _productRepository.GetProducts();
                _response.Result= ProductList;
            }
            catch(Exception ex)
            {
                 _response.IsSucess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }
            return _response;   
        }
        [AllowAnonymous]
        [HttpGet("{productId}")]
        public async Task<object> GetProduct(int productId)
        {
            try
            {
                var Product = await _productRepository.GetProductById(productId);
                _response.Result = Product;
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
        [HttpPost]
        public async Task<object> CreateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                var Product = await _productRepository.createUpdateProduct(productDto);
                _response.Result = Product;
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
        public async Task<object> UpdateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                var Product = await _productRepository.createUpdateProduct(productDto);
                _response.Result = Product;
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
        [HttpDelete("{productId}")]
        [Authorize(Roles ="Admin")]
        public async Task<object> CreateProduct(int productId)
        {
            try
            {
                var idDeleted = await _productRepository.DeleteProduct(productId);
                _response.Result = idDeleted;
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
