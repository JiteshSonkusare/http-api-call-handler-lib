using HttpClientApiCaller.Configuration;

namespace HttpClientConsole.Configuration
{
    public class SampleApiClientConfig : IApiClientConfig
    {
        public string? BaseUrl { get; set; }
        public TimeSpan? Timeout { get; set; }
    }
}