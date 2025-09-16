
namespace CustomerLeadImages.Models;
public class UploadImagesRequest
{
    public List<string> Base64Images { get; set; } = new();
    public string? MimeType { get; set; }
}
public class ImageResponse
{
    public long Id { get; set; }
    public string MimeType { get; set; } = "image/jpeg";
    public string Base64Data { get; set; } = "";
    public string? Caption { get; set; }
    public DateTime CreatedOn { get; set; }
}
