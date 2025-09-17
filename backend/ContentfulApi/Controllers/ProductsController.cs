using Contentful.Core;
using ContentfulApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentfulApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IContentfulClient _contentfulClient;

    public ProductsController(IContentfulClient contentfulClient)
    {
        _contentfulClient = contentfulClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        try
        {
            var builder = new Contentful.Core.Search.QueryBuilder<Product>()
                .ContentTypeIs("testProduct");
                
            var products = await _contentfulClient.GetEntries(builder);
            
            // Chuyển đổi sản phẩm để đảm bảo cung cấp URL hình ảnh đúng
            var simplifiedProducts = products.Items.Select(p => new
            {
                p.Sys,
                p.ProductName,
                p.Price,
                ProductImage = p.ProductImageUrl // Sử dụng property đã tạo
            }).ToList();
            
            return Ok(simplifiedProducts);
        }
        catch (Exception ex)
        {
            // Log chi tiết lỗi
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
            var product = await _contentfulClient.GetEntry<Product>(id);
            if (product == null)
                return NotFound();

            // Chuyển đổi tương tự như trên
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