using NLog;
using HttpClientConsole.Clients;
using HttpClientApiCaller.Helpers;
using Microsoft.Extensions.Options;
using HttpClientConsole.Configuration;

namespace HttpClientConsole.Service.Samples
{
    internal class SampleService : ISampleService
    {
        private readonly SampleApiClientConfig _option;
        private readonly ILogger _logger;

        public SampleService(ILogger logger, IOptions<SampleApiClientConfig> option)
        {
            _option = option.Value;
            _logger = logger;
        }

        public async Task<IList<Models.Sample>> GetSampleData()
        {
            var client = new SampleApiClient(_logger, _option, null);
            var samples = await client.GetData(CancellationToken.None);
            var result = samples.Data ?? new List<Models.Sample>();
            _logger.Info($"Sample data : {result.ToJson()}");
            return result;
        }
    }
}
