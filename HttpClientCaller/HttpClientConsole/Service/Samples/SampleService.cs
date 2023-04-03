using HttpClientApiCaller.Helpers;
using HttpClientConsole.Clients;
using HttpClientConsole.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;

namespace HttpClientConsole.Service.Samples
{
    internal class SampleService : ISampleService
    {
        private SampleApiClientConfig _option;
        private NLog.ILogger _logger;

        public SampleService(NLog.ILogger logger, IOptions<SampleApiClientConfig> option)
        {
            _option = option.Value;
            _logger = logger;
        }

        public IList<Models.Sample> GetSampleData()
        {
            var client = new SampleApiClient(_logger, _option, null);
            var result = client.GetData(CancellationToken.None);
            _logger.Info(result.ToJson());
            return result.Data;
        }
    }
}
