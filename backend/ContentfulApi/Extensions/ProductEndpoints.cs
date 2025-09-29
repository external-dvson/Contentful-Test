using Microsoft.AspNetCore.Mvc;

namespace ContentfulApi.Extensions;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/products")
            .WithTags("Products")
            .WithOpenApi();

        group.MapGet("/", GetAllProducts)
            .WithName("GetAllProducts")
            .WithSummary("Get all products")
            .Produces<IEnumerable<object>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("/{id}", GetProductById)
            .WithName("GetProductById")
            .WithSummary("Get a product by ID")
            .Produces<object>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        return group;
    }

    private static async Task<IResult> GetAllProducts(IProductService productService)
    {
        try
        {
            var products = await productService.GetAllProductsAsync();
            
            var simplifiedProducts = products.Select(p => new
            {
                p.Sys,
                p.ProductName,
                p.Price,
                ProductImage = p.ProductImageUrl
            }).ToList();
            
            return Results.Ok(simplifiedProducts);
        }
        catch (Exception ex)
        {
            // Log detailed error
            Console.WriteLine($"Error getting products: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return Results.Problem(
                title: "Internal Server Error",
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> GetProductById(string id, IProductService productService)
    {
        try
        {
            var product = await productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return Results.NotFound($"Product with ID {id} not found.");
            }

            var simplifiedProduct = new
            {
                product.Sys,
                product.ProductName,
                product.Price,
                ProductImage = product.ProductImageUrl
            };
            
            return Results.Ok(simplifiedProduct);
        }
        catch (Exception ex)
        {
            // Log detailed error
            Console.WriteLine($"Error getting product {id}: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return Results.Problem(
                title: "Internal Server Error",
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}