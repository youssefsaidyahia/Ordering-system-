using Mango.Web.Models;
using Mango.Web.Models.Dtos;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services
{
    public class CartService : BaseService ,ICartService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CartService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }
        public  async Task<T> AddCartAsync<T>(CartDto cartDto, string token = null)
        {
            return await this.SendASync<T>(new APIRequest()
            {
                apiType = SD.ApiType.POST,
                Data = cartDto,
                url = SD.ShoppingCartApiBase + "/api/Cart/AddCart",
                AccessToken = token
            });
        }

        public async Task<T> ApplyCoupon<T>(CartDto cartDto, string token = null)
        {
            return await this.SendASync<T>(new APIRequest()
            {
                apiType = SD.ApiType.POST,
                Data = cartDto,
                url = SD.ShoppingCartApiBase + "/api/Cart/ApplyCoupon",
                AccessToken = token
            });
        }

        public async Task<T> Checkout<T>(CartHeaderDto cart, string token = null)
        {
            return await this.SendASync<T>(new APIRequest()
            {
                apiType = SD.ApiType.POST,
                Data = cart,
                url = SD.ShoppingCartApiBase + "/api/Cart/Checkout",
                AccessToken = token
            });
        }

        public async Task<T> DeleteCartAsync<T>(int cartId, string token = null)
        {
            return await this.SendASync<T>(new APIRequest()
            {
                apiType = SD.ApiType.DELETE,
                url = SD.ShoppingCartApiBase + "/api/Cart/" + cartId,
                AccessToken = token
            });
        }

        public async Task<T> GetCartBYUserIdAsync<T>(string userId, string token = null)
        {
            return await this.SendASync<T>(new APIRequest()
            {
                apiType = SD.ApiType.GET,
                url = SD.ShoppingCartApiBase + "/api/Cart/" + userId,
                AccessToken = token
            });
        }

        public async Task<T> RemoveCoupon<T>(string userId, string token = null)
        {
            return await this.SendASync<T>(new APIRequest()
            {
                apiType = SD.ApiType.POST,
                Data = userId,
                url = SD.ShoppingCartApiBase + "/api/Cart/RemoveCoupon",
                AccessToken = token
            });
        }

        public async Task<T> UpdateCartAsync<T>(CartDto cartDto, string token = null)
        {
            return await this.SendASync<T>(new APIRequest()
            {
                apiType = SD.ApiType.PUT,
                Data = cartDto,
                url = SD.ShoppingCartApiBase + "/api/Cart",
                AccessToken = token
            });
        }
    }
}
