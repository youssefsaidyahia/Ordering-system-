using Mango.Web.Models.Dtos;

namespace Mango.Web.Services.IServices
{
    public interface IProductsService :IBaseService
    {
        Task<T> GetAllProductsAsync<T>(string token);   
        Task<T> GetProductByIdAsync<T>(int ProductId , string token);   
        Task<T> CreateProductAsyn<T>(ProductDto Product, string token);   
        Task<T> UpdateProductAsyn<T>(ProductDto Product, string token);   
        Task<T> DeleteProductAsyn<T>(int ProductId, string token);   
    }
}
