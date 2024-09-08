using Amazon;
using Amazon.CloudWatchLogs;
using Amazon.S3;
using Amazon.SimpleEmail;
using EarlyVisionAI.Services;
using Hangfire;
using Hangfire.PostgreSql;
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
    builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());

    builder.Services.AddAWSService<IAmazonSimpleEmailService>();
    builder.Services.AddAWSService<IAmazonS3>();
    builder.Services.AddHttpClient();

    builder.Services.AddSingleton<IJobQueue, JobQueue>();
    builder.Services.AddSingleton<IImageProcessor, ImageProcessor>();
    builder.Services.AddSingleton<IAiService, OpenAiService>();
    builder.Services.AddSingleton<ICvatService, CvatService>();
    builder.Services.AddSingleton<IPdfGenerator, PdfGenerator>();
    builder.Services.AddSingleton<IEmailService, EmailService>();
    builder.Services.AddSingleton<IS3Service, S3Service>();

    builder.Services.AddHangfire(x => x.UsePostgreSqlStorage(x => {
        x.UseNpgsqlConnection(builder.Configuration.GetConnectionString("HangfireConnection"));
    }));
    builder.Services.AddHangfireServer();

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
