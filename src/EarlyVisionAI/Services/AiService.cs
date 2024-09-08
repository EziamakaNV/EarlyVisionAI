namespace EarlyVisionAI.Services
{
    // Services/IAiService.cs
    public interface IAiService
    {
        Task<AiResult> AnalyzeImageAsync(IFormFile image);
    }

    // Services/OpenAiService.cs
    public class OpenAiService : IAiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAiService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["OpenAI:ApiKey"];
        }

        public async Task<AiResult> AnalyzeImageAsync(IFormFile image)
        {
            // Implement OpenAI API call here
            // Return AiResult with cancer probability and points of interest
        }
    }
}
