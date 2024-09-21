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
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public IFormFile Image { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        public IndexModel(IAiService aiService, ILogger<IndexModel> logger)
        {
            _aiService = aiService;
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

            if (Image == null)
            {
                ErrorMessage = "Please select at least one image to upload.";
                return Page();
            }

            var result = string.Empty;

            try
            {
                using (var stream = Image.OpenReadStream())
                {
                    // Analyze image with AI service
                    var aiResult = await _aiService.AnalyzeImageAsync(stream);
                    result = aiResult;
                }

                _logger.LogInformation("Image analysis completed successfully");
                SuccessMessage = "Images analyzed successfully. Check the results below.";
                return RedirectToPage("Results", new { LlmResult = result });
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