using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace EarlyVisionAI.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly ILogger<S3Service> _logger;

        public S3Service(IConfiguration configuration, ILogger<S3Service> logger,
            IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
            _bucketName = configuration["AWS:S3:BucketName"];
            _logger = logger;
        }

        public async Task<string> UploadImageAsync(IFormFile image)
        {
            try
            {
                var fileExtension = Path.GetExtension(image.FileName);
                var fileName = $"{Guid.NewGuid()}{fileExtension}";

                using var memoryStream = new MemoryStream();
                await image.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                var putRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    InputStream = memoryStream,
                    ContentType = image.ContentType
                };

                await _s3Client.PutObjectAsync(putRequest);

                var imageUrl = $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
                _logger.LogInformation("Image uploaded to S3: {ImageUrl}", imageUrl);
                return imageUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image to S3");
                throw;
            }
        }

        public async Task<Stream> DownloadImageAsync(string imageUrl)
        {
            try
            {
                var uri = new Uri(imageUrl);
                var key = uri.AbsolutePath.TrimStart('/');

                var getRequest = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key
                };

                var response = await _s3Client.GetObjectAsync(getRequest);
                _logger.LogInformation("Image downloaded from S3: {ImageUrl}", imageUrl);
                return response.ResponseStream;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading image from S3: {ImageUrl}", imageUrl);
                throw;
            }
        }
    }
}
