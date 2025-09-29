using Microsoft.AspNetCore.Mvc;

namespace ContentfulApi.Extensions;

public static class BlogPostEndpoints
{
    public static RouteGroupBuilder MapBlogPostEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/blogposts")
            .WithTags("BlogPosts")
            .WithOpenApi();

        group.MapGet("/", GetAllBlogPosts)
            .WithName("GetAllBlogPosts")
            .WithSummary("Get all blog posts")
            .Produces<IEnumerable<object>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("/{id}", GetBlogPostById)
            .WithName("GetBlogPostById")
            .WithSummary("Get a blog post by ID")
            .Produces<object>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        return group;
    }

    private static async Task<IResult> GetAllBlogPosts(IBlogService blogService)
    {
        try
        {
            var posts = await blogService.GetAllBlogPostsAsync();

            var mappedPosts = posts.Select(post => new
            {
                post.Sys,
                post.Title,
                post.Slug,
                post.Content,
                FeaturedImage = new
                {
                    Url = post.FeaturedImageUrl
                }
            });
            
            return Results.Ok(mappedPosts);
        }
        catch (Exception ex)
        {
            // Log detailed error
            Console.WriteLine($"Error getting blog posts: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return Results.Problem(
                title: "Internal Server Error",
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> GetBlogPostById(string id, IBlogService blogService)
    {
        try
        {
            var post = await blogService.GetBlogPostByIdAsync(id);
            if (post == null)
            {
                return Results.NotFound($"Blog post with ID {id} not found.");
            }

            var mappedPost = new
            {
                post.Sys,
                post.Title,
                post.Slug,
                post.Content,
                FeaturedImage = new
                {
                    Url = post.FeaturedImageUrl
                }
            };
            
            return Results.Ok(mappedPost);
        }
        catch (Exception ex)
        {
            // Log detailed error
            Console.WriteLine($"Error getting blog post {id}: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return Results.Problem(
                title: "Internal Server Error",
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}