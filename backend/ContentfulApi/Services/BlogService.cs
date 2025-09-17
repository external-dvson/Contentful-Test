using Contentful.Core;
using ContentfulApi.Models;

namespace ContentfulApi;

public class BlogService : IBlogService
{
    private readonly IContentfulClient _contentfulClient;
    public BlogService(IContentfulClient contentfulClient)
    {
        _contentfulClient = contentfulClient;
    }

    public async Task<List<BlogPost>> GetAllBlogPostsAsync()
    {
        var builder = new Contentful.Core.Search.QueryBuilder<BlogPost>()
                .ContentTypeIs("pageBlogPost");

        var blogPosts = await _contentfulClient.GetEntries(builder);
        return [.. blogPosts.Items];
    }

    public async Task<BlogPost?> GetBlogPostByIdAsync(string id)
    {
        try
        {
            var blogPost = await _contentfulClient.GetEntry<BlogPost>(id);
            return blogPost;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving blog post with ID {id}: {ex.Message}");
            return null;
        }
    }
}
