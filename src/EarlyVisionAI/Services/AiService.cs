using GenerativeAI.Classes;
using GenerativeAI.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EarlyVisionAI.Services
{
    // Services/IAiService.cs
    public interface IAiService
    {
        Task<AiResult> AnalyzeImageAsync(Stream imageStream);
    }

    // Services/OpenAiService.cs
    public class GeminiAiService : IAiService
    {
        private readonly ILogger<GeminiAiService> _logger;
        private readonly GeminiProVision _visionModel;

        public GeminiAiService(IConfiguration configuration, ILogger<GeminiAiService> logger)
        {
            _logger = logger;
            var apiKey = configuration["Gemini:ApiKey"]!;
            _visionModel = new GeminiProVision(apiKey);
            _logger.LogInformation("OpenAiService initialized");
        }

        public async Task<AiResult> AnalyzeImageAsync(Stream imageStream)
        {
            _logger.LogInformation("Starting image analysis");

            try
            {
                using var memoryStream = new MemoryStream();
                await imageStream.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();

                var prompt = @"Analyze this medical image for signs of cancer. Provide:
                1. A probability of cancer from 0 to 1.
                2. Points of interest that may indicate cancer, as coordinates (x, y) where x and y are percentages of the image width and height.
                Format the response as JSON with 'probability' and 'pointsOfInterest' fields. The 'pointsOfInterest' should be an array of objects, each with 'x' and 'y' properties.";

                var result = await _visionModel.GenerateContentAsync(prompt, new FileObject(imageBytes, "image.jpg"));
                _logger.LogInformation("Image analysis completed successfully");

                var jsonResponse = result.Text()!;
                _logger.LogDebug("Received JSON response: {JsonResponse}", jsonResponse);

                var analysisResult = JsonSerializer.Deserialize<AnalysisResult>(jsonResponse);

                if (analysisResult == null)
                {
                    _logger.LogError("Failed to deserialize the analysis result");
                    throw new InvalidOperationException("Invalid response format from the AI model");
                }

                return new AiResult
                {
                    CancerProbability = analysisResult.Probability,
                    PointsOfInterest = analysisResult.PointsOfInterest
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during image analysis");
                throw;
            }
        }
    }

    public class AnalysisResult
    {
        public double Probability { get; set; }
        public List<Coordinate> PointsOfInterest { get; set; } = new List<Coordinate>();
    }

    public class Coordinate
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class AiResult
    {
        public double CancerProbability { get; set; }
        public List<Coordinate> PointsOfInterest { get; set; } = new List<Coordinate>();
    }
}
