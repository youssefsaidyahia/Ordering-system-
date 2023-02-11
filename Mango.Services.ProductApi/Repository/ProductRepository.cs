using AutoMapper;
using Mango.Services.ProductApi.DbContexts;
using Mango.Services.ProductApi.Dtos;
using Mango.Services.ProductApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductApi.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;

        public ProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductDto> createUpdateProduct(ProductDto product)
        {
            var Product=_mapper.Map<Product>(product);
            if (Product.ProductId > 0)
            {
                _context.products.Update(Product);
            }
            else 
            {
                _context.products.Add(Product);
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductDto>(Product);
        }

        public async Task<bool> DeleteProduct(int Productid)
        {
            try
            {
                var Product = await _context.products.FirstOrDefaultAsync(p => p.ProductId == Productid);
                if (Product == null) { return false; }
                _context.products.Remove(Product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception )
            {
                return false;   
            }
           
        }

        public async Task<ProductDto> GetProductById(int Productid)
        {
            var Product = await _context.products.Where(p => p.ProductId==Productid).FirstOrDefaultAsync();
            return _mapper.Map<ProductDto>(Product);
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var ProductList = await _context.products.ToListAsync();
           return _mapper.Map<IEnumerable<ProductDto>>(ProductList);
        }
    }
}
