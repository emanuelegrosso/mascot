using Dropbox.Api;
using Dropbox.Api.Files;
using System.Text;

namespace MascotBooking.Server.Services;

public class DropboxStorageService : IStorageService
{
    private readonly DropboxClient _dropboxClient;
    private readonly string _basePath;
    private readonly ILogger<DropboxStorageService> _logger;

    public DropboxStorageService(IConfiguration configuration, ILogger<DropboxStorageService> logger)
    {
        _logger = logger;
        _basePath = configuration["Storage:Dropbox:BasePath"] ?? "/MascotBooking";

        var accessToken = configuration["Storage:Dropbox:AccessToken"];
        if (string.IsNullOrEmpty(accessToken))
        {
            throw new InvalidOperationException("Dropbox access token not configured. Set Storage:Dropbox:AccessToken in appsettings.json");
        }

        _dropboxClient = new DropboxClient(accessToken);
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder, string contentType)
    {
        try
        {
            var folderPath = string.IsNullOrEmpty(folder) ? _basePath : $"{_basePath}/{folder}";
            var filePath = $"{folderPath}/{DateTime.UtcNow:yyyyMMdd_HHmmss}_{fileName}";

            // Ensure folder exists
            await EnsureFolderExistsAsync(folderPath);

            // Upload file
            var uploadArg = new UploadArg(filePath, WriteMode.Overwrite.Instance);
            var uploadedFile = await _dropboxClient.Files.UploadAsync(uploadArg, fileStream);

            // Get shared link (public URL)
            var sharedLink = await _dropboxClient.Sharing.CreateSharedLinkWithSettingsAsync(filePath);
            
            // Convert to direct download link
            var directLink = sharedLink.Url.Replace("?dl=0", "?raw=1");
            
            return directLink;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file to Dropbox");
            throw;
        }
    }

    public async Task<bool> DeleteFileAsync(string fileUrl)
    {
        try
        {
            var filePath = ExtractFilePathFromUrl(fileUrl);
            if (string.IsNullOrEmpty(filePath))
                return false;

            await _dropboxClient.Files.DeleteV2Async(filePath);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file from Dropbox");
            return false;
        }
    }

    public Task<string> GetFileUrlAsync(string fileUrl)
    {
        // Dropbox URLs are already public
        return Task.FromResult(fileUrl);
    }

    private async Task EnsureFolderExistsAsync(string folderPath)
    {
        try
        {
            await _dropboxClient.Files.GetMetadataAsync(folderPath);
        }
        catch (ApiException<GetMetadataError> ex)
        {
            // Check if it's a path not found error
            if (ex.ErrorResponse.IsPath && ex.ErrorResponse.AsPath.Value.IsNotFound)
            {
                // Folder doesn't exist, create it
                await _dropboxClient.Files.CreateFolderV2Async(folderPath);
            }
            else
            {
                throw;
            }
        }
        catch (Exception ex)
        {
            // Check if it's a path not found error by message
            if (ex.Message.Contains("not_found") || ex.Message.Contains("path_not_found"))
            {
                await _dropboxClient.Files.CreateFolderV2Async(folderPath);
            }
            else
            {
                throw;
            }
        }
    }

    private string? ExtractFilePathFromUrl(string url)
    {
        // Extract file path from Dropbox shared link
        // This is a simplified version - you may need to adjust based on actual URL format
        try
        {
            var uri = new Uri(url);
            var pathMatch = System.Text.RegularExpressions.Regex.Match(uri.PathAndQuery, @"/s/([^?]+)");
            if (pathMatch.Success)
            {
                return "/" + Uri.UnescapeDataString(pathMatch.Groups[1].Value);
            }
        }
        catch { }

        return null;
    }
}
