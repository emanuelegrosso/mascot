namespace MascotBooking.Server.Services;

public interface IStorageService
{
    /// <summary>
    /// Uploads a file to cloud storage and returns the public URL
    /// </summary>
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder, string contentType);

    /// <summary>
    /// Deletes a file from cloud storage
    /// </summary>
    Task<bool> DeleteFileAsync(string fileUrl);

    /// <summary>
    /// Gets a public URL for viewing/downloading a file
    /// </summary>
    Task<string> GetFileUrlAsync(string fileUrl);
}
