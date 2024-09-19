using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyVisionAI.Services
{
    public interface IImageAnnotationService
    {
        Task<string> AnnotateImageAsync(string imagePath, List<Coordinate> pointsOfInterest);
    }
    public class ImageAnnotationService : IImageAnnotationService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<ImageAnnotationService> _logger;

        public ImageAnnotationService(IConfiguration configuration, ILogger<ImageAnnotationService> logger)
        {
            _logger = logger;

            var cloudinaryUrl = configuration["Cloudinary:Url"];
            _cloudinary = new Cloudinary(cloudinaryUrl);

            _logger.LogInformation("ImageAnnotationService initialized");
        }

        public async Task<string> AnnotateImageAsync(string imagePath, List<Coordinate> pointsOfInterest)
        {
            try
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(imagePath),
                    PublicId = $"annotated_{Guid.NewGuid()}"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                var publicId = uploadResult.PublicId;

                var transformation = new Transformation();

                foreach (var point in pointsOfInterest)
                {
                    transformation.Chain()
                        .Overlay(new TextLayer().Text("X").FontFamily("Arial").FontSize(20))
                        .Gravity("north_west")
                        .X($"{point.X:F2}p")
                        .Y($"{point.Y:F2}p")
                        .Color("red");
                }

                var annotatedImageUrl = _cloudinary.Api.UrlImgUp.Transform(transformation).BuildUrl(publicId);

                _logger.LogInformation($"Image annotated successfully: {annotatedImageUrl}");

                return annotatedImageUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during image annotation");
                throw;
            }
        }
    }
}