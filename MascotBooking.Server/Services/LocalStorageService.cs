namespace MascotBooking.Server.Services;

public class LocalStorageService : IStorageService
{
    private readonly string _basePath;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<LocalStorageService> _logger;
    private readonly string _baseUrl;

    public LocalStorageService(IConfiguration configuration, IWebHostEnvironment environment, ILogger<LocalStorageService> logger)
    {
        _environment = environment;
        _logger = logger;

        // Get base storage path from configuration or use default
        var configuredPath = configuration["Storage:Local:BasePath"];
        if (string.IsNullOrEmpty(configuredPath))
        {
            // Default to Storage folder in project root
            _basePath = Path.Combine(_environment.ContentRootPath, "..", "..", "Storage");
        }
        else
        {
            _basePath = Path.GetFullPath(configuredPath);
        }

        // Ensure base directory exists
        Directory.CreateDirectory(_basePath);

        // Get base URL for file access
        _baseUrl = configuration["Storage:Local:BaseUrl"] ?? "/storage";
        
        _logger.LogInformation("LocalStorageService initialized. BasePath: {BasePath}, BaseUrl: {BaseUrl}", _basePath, _baseUrl);
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder, string contentType)
    {
        try
        {
            // Sanitize folder name
            var safeFolder = SanitizeFileName(folder ?? "documents");
            var folderPath = Path.Combine(_basePath, safeFolder);
            
            // Ensure folder exists
            Directory.CreateDirectory(folderPath);

            // Sanitize file name and add timestamp
            var safeFileName = SanitizeFileName($"{DateTime.UtcNow:yyyyMMdd_HHmmss}_{fileName}");
            var filePath = Path.Combine(folderPath, safeFileName);

            // Save file
            using (var fileStreamOut = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(fileStreamOut);
            }

            // Return URL relative to web root
            var fileUrl = $"{_baseUrl}/{safeFolder}/{safeFileName}";
            
            _logger.LogInformation("File uploaded successfully: {FilePath}, URL: {FileUrl}", filePath, fileUrl);
            
            return fileUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file to local storage");
            throw;
        }
    }

    public async Task<bool> DeleteFileAsync(string fileUrl)
    {
        try
        {
            // Extract file path from URL
            var relativePath = fileUrl.Replace(_baseUrl + "/", "").Replace('/', Path.DirectorySeparatorChar);
            var filePath = Path.Combine(_basePath, relativePath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                _logger.LogInformation("File deleted: {FilePath}", filePath);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file from local storage");
            return false;
        }
    }

    public Task<string> GetFileUrlAsync(string fileUrl)
    {
        // Local URLs are already accessible
        return Task.FromResult(fileUrl);
    }

    private string SanitizeFileName(string fileName)
    {
        // Remove invalid characters for file names
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
        
        // Remove any remaining dangerous characters
        sanitized = sanitized.Replace("..", "").Replace(" ", "_");
        
        return sanitized;
    }
}
