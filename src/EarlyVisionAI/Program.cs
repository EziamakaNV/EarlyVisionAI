using Amazon;
using Amazon.CloudWatchLogs;
using EarlyVisionAI.Services;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.AwsCloudWatch;
using Serilog.Sinks.AwsCloudWatch.LogStreamNameProvider;
using System.Globalization;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .WriteTo.AmazonCloudWatch(new CloudWatchSinkOptions
    {
        LogGroupName = $"/early-vision-ai/{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}",
        MinimumLogEventLevel = LogEventLevel.Information,
        CreateLogGroup = true,
        TextFormatter = new MessageTemplateTextFormatter("[{Timestamp:HH:mm:ss} {Level:u3}] {TraceId} {Message:lj}{NewLine}{Exception}",
        CultureInfo.InvariantCulture),
        LogStreamNameProvider = new ConfigurableLogStreamNameProvider(DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"))
    }, new AmazonCloudWatchLogsClient(RegionEndpoint.EUWest2))
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddHttpClient();

    builder.Services.AddSingleton<IAiService, GeminiAiService>();
    // Register Image Annotation Service
    builder.Services.AddScoped<IImageAnnotationService, ImageAnnotationService>();

    // Register Cloudinary settings
    builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));

    // Register Gemini settings
    builder.Services.Configure<GeminiSettings>(builder.Configuration.GetSection("Gemini"));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapRazorPages();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
