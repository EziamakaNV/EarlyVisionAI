using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace EarlyVisionAI.Pages
{
    public class ResultsModel : PageModel
    {
        private readonly ILogger<ResultsModel> _logger;

        [BindProperty(SupportsGet = true)]
        public string LlmResult { get; set; }

        public ResultsModel(ILogger<ResultsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("Displaying LLM result on Results page");

            if (string.IsNullOrEmpty(LlmResult))
            {
                _logger.LogWarning("LLM result is empty or null");
                LlmResult = "No result available. Please try again.";
            }
        }
    }
}