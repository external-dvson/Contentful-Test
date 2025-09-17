using Microsoft.AspNetCore.Mvc;

namespace ContentfulApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        try
        {
            var products = await _productService.GetAllProductsAsync();
            
            var simplifiedProducts = products.Select(p => new
            {
                p.Sys,
                p.ProductName,
                p.Price,
                ProductImage = p.ProductImageUrl
            }).ToList();
            
            return Ok(simplifiedProducts);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting products: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(string id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            var simplifiedProduct = new
            {
                product.Sys,
                product.ProductName,
                product.Price,
                ProductImage = product.ProductImageUrl
            };
            
            return Ok(simplifiedProduct);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}