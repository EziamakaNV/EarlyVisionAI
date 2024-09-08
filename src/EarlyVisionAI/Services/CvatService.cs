namespace EarlyVisionAI.Services
{
    // Services/ICvatService.cs
    public interface ICvatService
    {
        Task<byte[]> AnnotateImageAsync(Stream image, List<Coordinate> points);
    }

    // Services/CvatService.cs
    public class CvatService : ICvatService
    {
        public async Task<byte[]> AnnotateImageAsync(IFormFile image, List<Coordinate> points)
        {
            // Implement CVAT API call here
            // Return annotated image as byte array
        }
    }
}
