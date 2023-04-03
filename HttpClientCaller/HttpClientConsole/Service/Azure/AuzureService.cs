using HttpClientConsole.Clients;
using HttpClientConsole.Configuration;
using HttpClientConsole.Service.Samples;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;

namespace HttpClientConsole.Service.Azure
{
    internal class AzureService : IAzureService
    {
        private AzureApiClientConfig _option;
        private NLog.ILogger _logger;

        public AzureService(NLog.ILogger logger, IOptions<AzureApiClientConfig> option)
        {
            _logger = logger;
            _option = option.Value;
        }

        public IList<Models.Sample> GetAzureData()
        {
            var client = new AzureApiClient(_logger, _option, null);
            var result = client.GetData(CancellationToken.None);
            return result.Data;
        }
    }
}
