using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using System.Text;
using GDriveFile = Google.Apis.Drive.v3.Data.File;

namespace MascotBooking.Server.Services;

public class GoogleDriveStorageService : IStorageService
{
    private readonly DriveService _driveService;
    private readonly string _folderId;
    private readonly ILogger<GoogleDriveStorageService> _logger;

    public GoogleDriveStorageService(IConfiguration configuration, ILogger<GoogleDriveStorageService> logger)
    {
        _logger = logger;
        _folderId = configuration["Storage:GoogleDrive:FolderId"] ?? "root";

        // Initialize Google Drive Service
        var credentialsJson = configuration["Storage:GoogleDrive:CredentialsJson"];
        if (string.IsNullOrEmpty(credentialsJson))
        {
            throw new InvalidOperationException("Google Drive credentials not configured. Set Storage:GoogleDrive:CredentialsJson in appsettings.json");
        }

        var credential = GoogleCredential.FromJson(credentialsJson)
            .CreateScoped(DriveService.Scope.DriveFile);

        _driveService = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "MascotBooking"
        });
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder, string contentType)
    {
        try
        {
            // Create folder structure if needed
            var targetFolderId = await GetOrCreateFolderAsync(folder);

            // Create file metadata
            var fileMetadata = new GDriveFile()
            {
                Name = $"{DateTime.UtcNow:yyyyMMdd_HHmmss}_{fileName}",
                Parents = new List<string> { targetFolderId }
            };

            // Upload file
            var request = _driveService.Files.Create(fileMetadata, fileStream, contentType);
            request.Fields = "id, webViewLink, webContentLink";
            var file = await request.UploadAsync();

            if (file.Status != Google.Apis.Upload.UploadStatus.Completed)
            {
                throw new Exception($"Upload failed: {file.Exception?.Message}");
            }

            // Make file publicly viewable
            var uploadedFile = request.ResponseBody;
            if (uploadedFile != null)
            {
                var permission = new Permission
                {
                    Type = "anyone",
                    Role = "reader"
                };
                await _driveService.Permissions.Create(permission, uploadedFile.Id).ExecuteAsync();

                // Return public URL
                return uploadedFile.WebViewLink ?? uploadedFile.WebContentLink ?? $"https://drive.google.com/file/d/{uploadedFile.Id}/view";
            }

            throw new Exception("Failed to get file URL after upload");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file to Google Drive");
            throw;
        }
    }

    public async Task<bool> DeleteFileAsync(string fileUrl)
    {
        try
        {
            var fileId = ExtractFileIdFromUrl(fileUrl);
            if (string.IsNullOrEmpty(fileId))
                return false;

            await _driveService.Files.Delete(fileId).ExecuteAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file from Google Drive");
            return false;
        }
    }

    public Task<string> GetFileUrlAsync(string fileUrl)
    {
        // Google Drive URLs are already public after permission is set
        return Task.FromResult(fileUrl);
    }

    private async Task<string> GetOrCreateFolderAsync(string folderName)
    {
        if (string.IsNullOrEmpty(folderName) || folderName == "root")
            return _folderId;

        // Search for existing folder
        var listRequest = _driveService.Files.List();
        listRequest.Q = $"name='{folderName}' and mimeType='application/vnd.google-apps.folder' and '{_folderId}' in parents and trashed=false";
        listRequest.Fields = "files(id, name)";
        var folders = await listRequest.ExecuteAsync();

        if (folders.Files != null && folders.Files.Any())
        {
            return folders.Files.First().Id;
        }

        // Create new folder
        var folderMetadata = new GDriveFile()
        {
            Name = folderName,
            MimeType = "application/vnd.google-apps.folder",
            Parents = new List<string> { _folderId }
        };

        var folder = await _driveService.Files.Create(folderMetadata).ExecuteAsync();
        return folder.Id;
    }

    private string? ExtractFileIdFromUrl(string url)
    {
        // Extract file ID from Google Drive URL
        // Format: https://drive.google.com/file/d/FILE_ID/view
        var match = System.Text.RegularExpressions.Regex.Match(url, @"/file/d/([a-zA-Z0-9_-]+)");
        return match.Success ? match.Groups[1].Value : null;
    }
}
