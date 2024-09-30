using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace EarlyVisionAI.Pages
{
    public class RecommendedSpecialistsModel : PageModel
    {
        private readonly ILogger<RecommendedSpecialistsModel> _logger;

        public RecommendedSpecialistsModel(ILogger<RecommendedSpecialistsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("Recommended Specialists page accessed");
        }
    }
}