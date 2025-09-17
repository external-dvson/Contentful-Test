using Microsoft.AspNetCore.Mvc;

namespace ContentfulApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlogPostsController : ControllerBase
{
    private readonly IBlogService _blogService;

    public BlogPostsController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBlogPosts()
    {
        try
        {
            var posts = await _blogService.GetAllBlogPostsAsync();

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
            
            return Ok(mappedPosts);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting blog posts: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return StatusCode(500, $"Error getting blog posts: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlogPost(string id)
    {
        try
        {
            var post = await _blogService.GetBlogPostByIdAsync(id);
            if (post == null)
                return NotFound();

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
            
            return Ok(mappedPost);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting blog post: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return StatusCode(500, $"Error getting blog post: {ex.Message}");
        }
    }
}