namespace EarlyVisionAI.Services
{
    // Services/IPdfGenerator.cs
    public interface IPdfGenerator
    {
        Task<byte[]> GenerateReportAsync(byte[] annotatedImage, AiResult aiResult);
    }

    // Services/PdfGenerator.cs
    public class PdfGenerator : IPdfGenerator
    {
        public async Task<byte[]> GenerateReportAsync(byte[] annotatedImage, AiResult aiResult)
        {
            // Implement PDF generation logic here
            // Return PDF as byte array
        }
    }
}
