using Hangfire;

namespace EarlyVisionAI.Services
{
    public class JobQueue : IJobQueue
    {
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly ILogger<JobQueue> _logger;

        public JobQueue(IBackgroundJobClient backgroundJobs, ILogger<JobQueue> logger)
        {
            _backgroundJobs = backgroundJobs;
            _logger = logger;
        }

        public Task<string> EnqueueJobAsync(List<string> imageUrls, string email)
        {
            var jobId = _backgroundJobs.Enqueue<IImageProcessor>(x => x.ProcessImagesAsync(imageUrls, email));
            _logger.LogInformation("Job enqueued with ID: {JobId}", jobId);
            return Task.FromResult(jobId);
        }
    }
}
