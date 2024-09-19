using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EarlyVisionAI.Services;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace EarlyVisionAI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAiService _aiService;
        private readonly IImageAnnotationService _annotationService;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public List<IFormFile> Images { get; set; }

        [BindProperty, Required, EmailAddress]
        public string Email { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        public IndexModel(IAiService aiService, IImageAnnotationService annotationService, ILogger<IndexModel> logger)
        {
            _aiService = aiService;
            _annotationService = annotationService;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please correct the errors and try again.";
                return Page();
            }

            if (Images == null || Images.Count == 0)
            {
                ErrorMessage = "Please select at least one image to upload.";
                return Page();
            }

            var results = new List<(string OriginalImage, string AnnotatedImage, double CancerProbability)>();

            try
            {
                foreach (var image in Images)
                {
                    if (image.Length > 0)
                    {
                        using (var stream = image.OpenReadStream())
                        {
                            // Analyze image with AI service
                            var aiResult = await _aiService.AnalyzeImageAsync(stream);

                            // Save the original image temporarily
                            var tempPath = Path.GetTempFileName();
                            using (var fileStream = new FileStream(tempPath, FileMode.Create))
                            {
                                await image.CopyToAsync(fileStream);
                            }

                            // Annotate the image
                            var annotatedImageUrl = await _annotationService.AnnotateImageAsync(tempPath, aiResult.PointsOfInterest);

                            results.Add((image.FileName, annotatedImageUrl, aiResult.CancerProbability));

                            // Clean up the temporary file
                            System.IO.File.Delete(tempPath);
                        }
                    }
                }

                // Here you would typically send an email with the results
                // For demonstration, we'll just redirect to the results page
                SuccessMessage = "Images analyzed successfully. Check the results below.";
                return RedirectToPage("Results", new { results = results });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during image analysis");
                ErrorMessage = "An error occurred while processing your images. Please try again later.";
                return Page();
            }
        }
    }
}