using Contentful.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace ContentfulApi.Models;

public class Product
{
    public SystemProperties Sys { get; set; } = null!;
    
    public string ProductName { get; set; } = string.Empty;
    
    public int Price { get; set; }
    
    // Thay vì sử dụng converter phức tạp, sử dụng JToken để linh hoạt trong việc xử lý dữ liệu
    [JsonProperty("productImage")]
    public object ProductImage { get; set; } = null!;
    
    // Thêm property để truy xuất URL hình ảnh bất kể productImage là string hay object
    [JsonIgnore]
    public string ProductImageUrl 
    { 
        get
        {
            if (ProductImage == null)
                return string.Empty;
                
            if (ProductImage is string stringUrl)
                return stringUrl;
                
            // Nếu là JToken, cố gắng lấy URL
            if (ProductImage is JToken token)
            {
                try 
                {
                    return token["fields"]?["file"]?["url"]?.ToString() ?? string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
            
            return string.Empty;
        }
    }
}