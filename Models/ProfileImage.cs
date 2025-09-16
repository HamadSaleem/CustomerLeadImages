
namespace CustomerLeadImages.Models;
public class ProfileImage
{
    public long Id { get; set; }
    public string OwnerType { get; set; } = "";
    public int OwnerId { get; set; }
    public string Base64Data { get; set; } = "";
    public string MimeType { get; set; } = "image/jpeg";
    public string? Caption { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}
