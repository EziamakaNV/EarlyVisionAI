namespace EarlyVisionAI.Services
{
    // Services/ICvatService.cs
    public interface ICvatService
    {
        Task<byte[]> AnnotateImageAsync(IFormFile image, List<PointOfInterest> points);
    }

    // Services/CvatService.cs
    public class CvatService : ICvatService
    {
        public async Task<byte[]> AnnotateImageAsync(IFormFile image, List<PointOfInterest> points)
        {
            // Implement CVAT API call here
            // Return annotated image as byte array
        }
    }
}
