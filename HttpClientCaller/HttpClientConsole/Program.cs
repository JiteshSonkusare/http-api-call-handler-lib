using NLog;
using NLog.Web;
using NLog.Extensions.Logging;
using HttpClientApiCaller.Helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using HttpClientConsole.Service.Azure;
using HttpClientConsole.Configuration;
using HttpClientConsole.Service.Samples;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var logger = Logger();

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.Configure<SampleApiClientConfig>(context.Configuration.GetSection(nameof(SampleApiClientConfig)));
        services.Configure<AzureApiClientConfig>(context.Configuration.GetSection(nameof(AzureApiClientConfig)));
        services.AddSingleton<ISampleService, SampleService>(
            serviceProvider => new SampleService(
                logger: logger,
                option: serviceProvider.GetRequiredService<IOptions<SampleApiClientConfig>>())
            );
        services.AddSingleton<IAzureService, AzureService>(
           serviceProvider => new AzureService(
               logger: logger,
               option: serviceProvider.GetRequiredService<IOptions<AzureApiClientConfig>>())
           );
    })
    .Build();

var sampleService = host.Services.GetRequiredService<ISampleService>();
var azureService = host.Services.GetRequiredService<IAzureService>();
var sampleData = await sampleService.GetSampleData();
var azureData = await azureService.GetAzureData();
Console.WriteLine($"Sample -- Api -- Data: {sampleData.ToJson()}");
Console.WriteLine("---------------------------------------------------");
Console.WriteLine($"Azure -- Api -- Data: {azureData.ToJson()}");
Console.ReadLine();


static Logger Logger()
{
    var config = new ConfigurationBuilder()
                    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();
    LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));
    return LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
}