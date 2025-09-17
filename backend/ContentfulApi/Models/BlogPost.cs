using Contentful.Core.Models;
using Newtonsoft.Json;

namespace ContentfulApi.Models;

public class BlogPost
{
    public required SystemProperties Sys { get; set; }
    public required string Title { get; set; }
    public required string Slug { get; set; }
    
    [JsonProperty("content")]
    public required Document Content { get; set; }
    
    public Asset? FeaturedImage { get; set; }
    
    // Helper property to extract the image URL correctly
    [JsonProperty(nameof(FeaturedImageUrl))]
    public string? FeaturedImageUrl => FeaturedImage?.File?.Url != null 
        ? $"https:{FeaturedImage.File.Url}" 
        : null;
}

public class Document
{
    public string NodeType { get; set; } = "document";
    public required List<ContentNode> Content { get; set; }
}

public class ContentNode
{
    public string NodeType { get; set; } = "";
    public List<ContentNode>? Content { get; set; }
    public ContentData? Data { get; set; }
    public string? Value { get; set; }
}

public class ContentData
{
    public string? Uri { get; set; }
    public List<ContentMark>? Marks { get; set; }
}

public class ContentMark
{
    public string Type { get; set; } = "";
}