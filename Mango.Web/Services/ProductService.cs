using Mango.Web.Models;
using Mango.Web.Models.Dtos;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services
{
    public class ProductService : BaseService, IProductsService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        public async Task<T> DeleteProductAsyn<T>(int ProductId, string token)
        {
            return await this.SendASync<T>(new APIRequest()
            {
                apiType = SD.ApiType.DELETE,
                url = SD.ProductApiBase + "/api/Products/"+ProductId,
                AccessToken = token
            });
        }

        public async Task<T> CreateProductAsyn<T>(ProductDto Product, string token)
        {
            return await this.SendASync<T>(new APIRequest()
            {
                apiType = SD.ApiType.POST,
                Data = Product,
                url = SD.ProductApiBase + "/api/Products",
                AccessToken = token
            });
        }

        public async Task<T> GetAllProductsAsync<T>(string token)
        {
            return await this.SendASync<T>(new APIRequest()
            {
                apiType = SD.ApiType.GET,
                url = SD.ProductApiBase + "/api/Products",
                AccessToken = token
            });
        }

        public async Task<T> GetProductByIdAsync<T>(int ProductId, string token)
        {
           return await this.SendASync<T>(new APIRequest()
            {
                apiType = SD.ApiType.GET,
                url = SD.ProductApiBase + "/api/Products/" +ProductId,
                AccessToken = token
            });
        }

        public async Task<T> UpdateProductAsyn<T>(ProductDto Product, string token)
        {
            return await this.SendASync<T>(new APIRequest()
            {
                apiType = SD.ApiType.PUT,
                Data = Product,
                url = SD.ProductApiBase + "/api/Products",
                AccessToken = token
            });
        }
    }
}
