using GenerativeAI.Classes;
using GenerativeAI.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EarlyVisionAI.Services
{
    // Services/IAiService.cs
    public interface IAiService
    {
        Task<string> AnalyzeImageAsync(Stream imageStream);
    }

    // Services/OpenAiService.cs
    public class GeminiAiService : IAiService
    {
        private readonly ILogger<GeminiAiService> _logger;
        private readonly Gemini15Flash _visionModel;

        public GeminiAiService(IConfiguration configuration, ILogger<GeminiAiService> logger)
        {
            _logger = logger;
            var apiKey = configuration["Gemini:ApiKey"]!;
            _visionModel = new Gemini15Flash(apiKey);
            _logger.LogInformation("GeminiService initialized");
        }

        public async Task<string> AnalyzeImageAsync(Stream imageStream)
        {
            _logger.LogInformation("Starting image analysis");

            try
            {
                using var memoryStream = new MemoryStream();
                await imageStream.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();

                var prompt = @"Analyze the image provided for cancerous features. If there's a likelihood that it might be cancer,
                inform the user to see a specialist, if not ask the user not to bother. This is not medical advice, you are
                acting as a supplementary tool or knowledge base on if a user should see a specialist.
                Do not add any message about you not being a medical professional.";

                var result = await _visionModel.GenerateContentAsync(prompt, new FileObject(imageBytes, "image.jpg"));
                _logger.LogInformation("Image analysis completed successfully");

                var jsonResponse = result.Text()!;
                _logger.LogDebug("Received JSON response: {JsonResponse}", jsonResponse);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during image analysis");
                throw;
            }
        }
    }
}
