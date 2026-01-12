using Microsoft.AspNetCore.Mvc;
using MascotBooking.Server.Services;

namespace MascotBooking.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController : ControllerBase
{
    private readonly IStorageService _storageService;
    private readonly ILogger<FileUploadController> _logger;

    public FileUploadController(IStorageService storageService, ILogger<FileUploadController> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] string folder = "documents")
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded");
        }

        try
        {
            using var stream = file.OpenReadStream();
            var fileUrl = await _storageService.UploadFileAsync(
                stream,
                file.FileName,
                folder,
                file.ContentType
            );

            return Ok(new { url = fileUrl, fileName = file.FileName });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("upload-base64")]
    public async Task<IActionResult> UploadBase64([FromBody] Base64UploadRequest request)
    {
        if (string.IsNullOrEmpty(request.Data) || string.IsNullOrEmpty(request.FileName))
        {
            return BadRequest("Invalid request");
        }

        try
        {
            // Parse base64 data URL (data:image/jpeg;base64,...)
            var base64Data = request.Data;
            if (base64Data.Contains(","))
            {
                base64Data = base64Data.Split(',')[1];
            }

            var bytes = Convert.FromBase64String(base64Data);
            using var stream = new MemoryStream(bytes);

            var contentType = request.ContentType ?? "image/jpeg";
            var fileUrl = await _storageService.UploadFileAsync(
                stream,
                request.FileName,
                request.Folder ?? "documents",
                contentType
            );

            return Ok(new { url = fileUrl, fileName = request.FileName });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading base64 file");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpDelete("{*fileUrl}")]
    public async Task<IActionResult> DeleteFile(string fileUrl)
    {
        try
        {
            var decodedUrl = Uri.UnescapeDataString(fileUrl);
            var result = await _storageService.DeleteFileAsync(decodedUrl);
            return result ? Ok() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}

public class Base64UploadRequest
{
    public string Data { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string? ContentType { get; set; }
    public string? Folder { get; set; }
}
