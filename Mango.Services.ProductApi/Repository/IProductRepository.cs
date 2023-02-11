using Mango.Services.ProductApi.Dtos;
using Mango.Services.ProductApi.Models;

namespace Mango.Services.ProductApi.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<ProductDto> GetProductById(int Productid);
        Task<ProductDto> createUpdateProduct(ProductDto product);
        Task<bool> DeleteProduct(int Productid);
    }
}
