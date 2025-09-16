
using CustomerLeadImages.Data;
using CustomerLeadImages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerLeadImages.Controllers;
[ApiController]
[Route("api/profile-images")]
public class ProfileImagesController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProfileImagesController(AppDbContext db) { _db = db; }

    [HttpGet("{ownerType}/{ownerId:int}")]
    public async Task<ActionResult<List<ImageResponse>>> List(string ownerType, int ownerId)
    {
        ownerType = ownerType.ToLowerInvariant();
        var images = await _db.ProfileImages
            .Where(i => i.OwnerType == ownerType && i.OwnerId == ownerId)
            .OrderBy(i => i.CreatedOn)
            .Select(i => new ImageResponse { Id = i.Id, MimeType = i.MimeType, Base64Data = i.Base64Data, Caption = i.Caption, CreatedOn = i.CreatedOn })
            .ToListAsync();
        return Ok(images);
    }

    [HttpPost("{ownerType}/{ownerId:int}")]
    public async Task<IActionResult> Upload(string ownerType, int ownerId, [FromBody] UploadImagesRequest req)
    {
        if (req == null || req.Base64Images == null || req.Base64Images.Count == 0) return BadRequest("No images provided.");
        ownerType = ownerType.ToLowerInvariant();
        var mime = string.IsNullOrWhiteSpace(req.MimeType) ? "image/jpeg" : req.MimeType;
        const int MaxImages = 10;
        const int MaxBytes = 2 * 1024 * 1024;
        using var tx = await _db.Database.BeginTransactionAsync();
        var current = await _db.ProfileImages.CountAsync(i => i.OwnerType == ownerType && i.OwnerId == ownerId);
        var remain = MaxImages - current;
        if (remain <= 0) return Conflict("Image limit reached (10/10).");
        var toAdd = req.Base64Images.Take(remain).ToList();
        var skipped = req.Base64Images.Count - toAdd.Count;
        foreach (var b64 in toAdd)
        {
            byte[] bytes;
            try { bytes = Convert.FromBase64String(b64); } catch { return BadRequest("Invalid Base64 image."); }
            if (bytes.Length > MaxBytes) return BadRequest("An image exceeds the 2MB limit.");
            _db.ProfileImages.Add(new ProfileImage { OwnerType = ownerType, OwnerId = ownerId, Base64Data = b64, MimeType = mime });
        }
        var finalCount = current + toAdd.Count;
        if (finalCount > MaxImages) return Conflict("Concurrent upload exceeded the limit.");
        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        if (skipped > 0) return StatusCode(StatusCodes.Status207MultiStatus, new { added = toAdd.Count, skipped });
        return Created($"/api/profile-images/{ownerType}/{ownerId}", new { added = toAdd.Count });
    }

    [HttpDelete("{imageId:long}")]
    public async Task<IActionResult> Delete(long imageId)
    {
        var entity = await _db.ProfileImages.FindAsync(imageId);
        if (entity == null) return NotFound();
        _db.ProfileImages.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
