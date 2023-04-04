using NLog;
using HttpClientConsole.Clients;
using HttpClientApiCaller.Helpers;
using Microsoft.Extensions.Options;
using HttpClientConsole.Configuration;

namespace HttpClientConsole.Service.Azure
{
    internal class AzureService : IAzureService
    {
        private readonly AzureApiClientConfig _option;
        private readonly ILogger _logger;

        public AzureService(ILogger logger, IOptions<AzureApiClientConfig> option)
        {
            _logger = logger;
            _option = option.Value;
        }

        public async Task<IList<Models.Sample>> GetAzureData()
        {
            var client = new AzureApiClient(_logger, _option, null);
            var samples = await client.GetData(CancellationToken.None);
            var result = samples.Data ?? new List<Models.Sample>();
            _logger.Info($"Azure data : {result.ToJson()}");
            return result;
        }
    }
}
