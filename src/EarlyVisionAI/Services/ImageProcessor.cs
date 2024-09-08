namespace EarlyVisionAI.Services
{
    // Services/IImageProcessor.cs
    public interface IImageProcessor
    {
        Task ProcessImagesAsync(List<string> imageUrls, string email);
    }

    // Services/ImageProcessor.cs
    public class ImageProcessor : IImageProcessor
    {
        private readonly IAiService _aiService;
        private readonly ICvatService _cvatService;
        private readonly IPdfGenerator _pdfGenerator;
        private readonly IEmailService _emailService;
        private readonly IS3Service _s3Service;
        private readonly ILogger<ImageProcessor> _logger;

        public ImageProcessor(IAiService aiService, ICvatService cvatService,
                              IPdfGenerator pdfGenerator, IEmailService emailService,
                              IS3Service s3Service, ILogger<ImageProcessor> logger)
        {
            _aiService = aiService;
            _cvatService = cvatService;
            _pdfGenerator = pdfGenerator;
            _emailService = emailService;
            _s3Service = s3Service;
            _logger = logger;
        }

        public async Task ProcessImagesAsync(List<string> imageUrls, string email)
        {
            foreach (var imageUrl in imageUrls)
            {
                try
                {
                    using var imageStream = await _s3Service.DownloadImageAsync(imageUrl);
                    var aiResult = await _aiService.AnalyzeImageAsync(imageStream);

                    if (aiResult.CancerProbability > 0.5)
                    {
                        _logger.LogWarning("Potential cancer detected in image: {ImageUrl}", imageUrl);
                        var annotatedImage = await _cvatService.AnnotateImageAsync(imageStream, aiResult.PointsOfInterest);
                        var report = await _pdfGenerator.GenerateReportAsync(annotatedImage, aiResult);
                        await _emailService.SendEmailAsync(email, "Cancer Detection Report", report);
                        _logger.LogInformation("Report sent for image: {ImageUrl}", imageUrl);
                    }
                    else
                    {
                        _logger.LogInformation("No potential cancer detected in image: {ImageUrl}", imageUrl);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing image: {ImageUrl}", imageUrl);
                }
            }
        }
    }
}
