using ContentfulApi.Models;

namespace ContentfulApi;

public interface IBlogService
{
    Task<List<BlogPost>> GetAllBlogPostsAsync();
    Task<BlogPost?> GetBlogPostByIdAsync(string id);
}
