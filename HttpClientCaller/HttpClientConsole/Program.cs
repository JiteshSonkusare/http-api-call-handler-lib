using HttpClientApiCaller.Helpers;
using HttpClientConsole.Configuration;
using HttpClientConsole.Service.Azure;
using HttpClientConsole.Service.Samples;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NLog.Web;
using System;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddLogging(loggingBuilder =>
         {
             loggingBuilder.ClearProviders();
             loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
             loggingBuilder.AddNLog(context.Configuration.GetSection("NLog"));
         });
        var logger = NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();

        services.Configure<SampleApiClientConfig>(context.Configuration.GetSection(nameof(SampleApiClientConfig)));
        services.Configure<AzureApiClientConfig>(context.Configuration.GetSection(nameof(AzureApiClientConfig))); 
        services.AddSingleton<ISampleService, SampleService>(
            serviceProvider => new SampleService(
                logger : logger,
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

// Todo: Fix logging using NLog 
// Todo: USe IHttpFactoryClient to call api