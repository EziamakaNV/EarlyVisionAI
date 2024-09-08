namespace EarlyVisionAI.Services
{
    // Services/IEmailService.cs
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, byte[] attachment);
    }

    // Services/EmailService.cs
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, byte[] attachment)
        {
            // Implement email sending logic here using AWS SES or another email service
        }
    }
}
