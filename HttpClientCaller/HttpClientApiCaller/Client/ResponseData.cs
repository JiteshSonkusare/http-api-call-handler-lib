using System.Net;
using HttpClientApiCaller.Client;
using Newtonsoft.Json;

namespace HttpClientApiCaller.Client
{
    public sealed class ResponseData
    {
        public int StatusCode { get; private set; }
        public string Content { get; private set; }
        public IEnumerable<HeaderData> ResponseHeaders { get; private set; }

        public ResponseData(HttpStatusCode statusCode, string content, IEnumerable<HeaderData>? responseHeaders)
        {
            StatusCode = (int)statusCode;
            Content = content;
            ResponseHeaders = responseHeaders ?? Enumerable.Empty<HeaderData>();
        }

        public ResponseData(HttpStatusCode statusCode, string content, params HeaderData[]? responseHeaders)
            : this(statusCode, content, responseHeaders as IEnumerable<HeaderData>)
        {
        }

        public T? ConvertContent<T>() => JsonConvert.DeserializeObject<T?>(Content);

        public T? ConvertContent<T>(JsonSerializerSettings? settings) => JsonConvert.DeserializeObject<T?>(Content, settings);

        public object? ConvertContent(Type objectType) => JsonConvert.DeserializeObject(Content, objectType);

        public object? ConvertContent(Type objectType, JsonSerializerSettings? settings) => JsonConvert.DeserializeObject(Content, objectType, settings);
    }
}
