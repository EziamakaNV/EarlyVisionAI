namespace EarlyVisionAI.Services
{
    public interface IJobQueue
    {
        Task<string> EnqueueJobAsync(List<string> imageUrls, string email);
    }
}
