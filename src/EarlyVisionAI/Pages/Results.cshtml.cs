using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace EarlyVisionAI.Pages
{
    public class ResultsModel : PageModel
    {
        public List<(string OriginalImage, string AnnotatedImage, double CancerProbability)> Results { get; set; }

        public void OnGet(List<(string, string, double)> results)
        {
            Results = results;
        }
    }
}