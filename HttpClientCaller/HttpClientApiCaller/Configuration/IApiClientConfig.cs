using System.Net.Http;

namespace HttpClientApiCaller.Configuration
{
    public interface IApiClientConfig
    {
        public string? BaseUrl { get; set; }
        TimeSpan? Timeout { get; set; }
    }
}
