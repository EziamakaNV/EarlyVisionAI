using EarlyVisionAI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EarlyVisionAI.Pages
{
    public class UploadModel : PageModel
    {
        private readonly IJobQueue _jobQueue;
        private readonly IS3Service _s3Service;
        private readonly ILogger<UploadModel> _logger;

        public UploadModel(IJobQueue jobQueue, IS3Service s3Service, ILogger<UploadModel> logger)
        {
            _jobQueue = jobQueue;
            _s3Service = s3Service;
            _logger = logger;
        }

        [BindProperty]
        public List<IFormFile> Images { get; set; } = new();

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state in upload form");
                return Page();
            }

            try
            {
                var imageUrls = new List<string>();
                foreach (var image in Images)
                {
                    var imageUrl = await _s3Service.UploadImageAsync(image);
                    imageUrls.Add(imageUrl);
                    _logger.LogInformation("Image uploaded to S3: {ImageUrl}", imageUrl);
                }

                var jobId = await _jobQueue.EnqueueJobAsync(imageUrls, Email);
                _logger.LogInformation("Job enqueued with ID: {JobId}", jobId);

                return RedirectToPage("Confirmation", new { jobId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing upload");
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return Page();
            }
        }
    }
}
