using HttpClientApiCaller.Configuration;

namespace HttpClientConsole.Configuration
{
    internal class AzureApiClientConfig : IApiClientConfig
    {
        public string? BaseUrl { get; set; }
        public TimeSpan? Timeout { get; set; }
    }
}
