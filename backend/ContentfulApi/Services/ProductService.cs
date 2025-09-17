using Contentful.Core;
using ContentfulApi.Models;

namespace ContentfulApi;

public class ProductService : IProductService
{
    private readonly IContentfulClient _contentfulClient;
    public ProductService(IContentfulClient contentfulClient)
    {
        _contentfulClient = contentfulClient;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        var builder = new Contentful.Core.Search.QueryBuilder<Product>()
                .ContentTypeIs("testProduct");

        var products = await _contentfulClient.GetEntries(builder);
        return [.. products.Items];
    }

    public async Task<Product?> GetProductByIdAsync(string id)
    {
        try
        {
            var product = await _contentfulClient.GetEntry<Product>(id);
            return product;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving product with ID {id}: {ex.Message}");
            return null;
        }
    }
}
