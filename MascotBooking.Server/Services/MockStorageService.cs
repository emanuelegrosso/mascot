namespace MascotBooking.Server.Services;

/// <summary>
/// Mock storage service for development when no cloud storage is configured
/// </summary>
public class MockStorageService : IStorageService
{
    private readonly ILogger<MockStorageService> _logger;

    public MockStorageService(ILogger<MockStorageService> logger)
    {
        _logger = logger;
    }

    public Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder, string contentType)
    {
        _logger.LogWarning("MockStorageService: File upload not implemented. Configure Google Drive or Dropbox in appsettings.json");
        // Return a mock URL
        var mockUrl = $"/uploads/{folder}/{DateTime.UtcNow:yyyyMMdd_HHmmss}_{fileName}";
        return Task.FromResult(mockUrl);
    }

    public Task<bool> DeleteFileAsync(string fileUrl)
    {
        _logger.LogWarning("MockStorageService: File delete not implemented");
        return Task.FromResult(true);
    }

    public Task<string> GetFileUrlAsync(string fileUrl)
    {
        return Task.FromResult(fileUrl);
    }
}
