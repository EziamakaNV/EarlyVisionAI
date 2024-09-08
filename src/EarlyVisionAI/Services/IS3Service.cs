namespace EarlyVisionAI.Services
{
    public interface IS3Service
    {
        Task<string> UploadImageAsync(IFormFile image);
        Task<Stream> DownloadImageAsync(string imageUrl);
    }
}
