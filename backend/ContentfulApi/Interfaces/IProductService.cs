using ContentfulApi.Models;

namespace ContentfulApi;

public interface IProductService
{
    Task<List<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(string id);
}