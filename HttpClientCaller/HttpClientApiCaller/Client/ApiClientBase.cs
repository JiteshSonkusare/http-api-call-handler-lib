using NLog;
using System.Diagnostics;
using HttpClientApiCaller.Exceptions;
using HttpClientApiCaller.Configuration;
using HttpClientApiCaller.Security;
using System.Net.Mime;
using HttpClientApiCaller.Helpers;

namespace HttpClientApiCaller.Client
{
    public abstract class ApiClientBase : IDisposable
    {
        protected ILogger Logger { get; }
        private readonly HttpClient _client;
        private readonly IAuthHandler _authHandler;

        protected ApiClientBase(ILogger logger, IApiClientConfig config, IAuthHandler authHandler)
        {
            Logger = logger;
            _authHandler = authHandler;
            _client = new HttpClient
            {
                BaseAddress = new Uri(config?.BaseUrl ?? throw new ArgumentNullException(nameof(config))),
                Timeout = config?.Timeout ?? new TimeSpan(0, 0, 15)
            };
            AddDefaultHeaders(new string[] { MediaTypeNames.Application.Json });
        }

        private ResponseData ExecuteRequest(HttpRequestMessage request, CancellationToken cancellation)
        {
            try
            {
                if (_authHandler != null)
                    _client.DefaultRequestHeaders.Authorization = _authHandler.GetAuthToken(cancellation)?.GetAuthorizationHeader();
                HttpResponseMessage? response = null;
                Exception? taskError = null;
                var stopwatch = new Stopwatch();
                Logger.Log(LogLevel.Trace, $"ExecuteRequest: {request.Method} request headers: \n {request.Headers.ToJson()}");
                stopwatch.Start();
                _client.SendAsync(request, cancellation).ContinueWith(task =>
                {
                    stopwatch.Stop();
                    //Logger.Log(LogLevel.Debug, $"{request.Method} HTTP request completed with status: {task.Result.StatusCode} in {stopwatch.ElapsedMilliseconds} ms.");
                    if (task.Exception != null)
                        taskError = task.Exception;
                    else
                        response = task.Result;
                }, cancellation).Wait(cancellation);
                if (taskError != null)
                    throw taskError;
                if (response == null)
                    throw new GeneralApplicationException("Unknown error! Could not retrieve response.");

                string content = string.Empty;
                response.Content.ReadAsStringAsync(cancellation).ContinueWith(task =>
                {
                    Logger.Log(LogLevel.Trace, $"RAW response content from URI={request?.RequestUri?.AbsoluteUri}: {task.Result} (status={response.StatusCode})");
                    if (task.Exception != null)
                        taskError = task.Exception;
                    else
                        content = task.Result;
                }, cancellation).Wait(cancellation);
                if (taskError != null)
                    throw taskError;

                var responseHeaders = new List<HeaderData>();
                responseHeaders.AddRange(response.Headers.Select(H => new HeaderData(true, H.Key, H.Value.ToArray())));
                return new ResponseData(response.StatusCode, content, responseHeaders.ToArray());
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Error, "Error occurred while executing HTTP request.", ex.Message);
                throw new GeneralApplicationException("Error occurred while executing HTTP request.", ex);
            }
        }

        protected void AddDefaultHeaders(string[] acceptedMediaTypes, params HeaderData[] headers)
        {
            foreach (string mediaType in acceptedMediaTypes)
                _client.DefaultRequestHeaders.Accept.ParseAdd(mediaType);
            foreach (HeaderData header in headers)
                _client.DefaultRequestHeaders.Add(header.Name, header.Values);
        }

        protected void Send(Uri requestUri, HttpMethod method, Action<ResponseData> responseParser, CancellationToken cancellation, params HeaderData[] requestHeaders)
        {
            var message = new HttpRequestMessage(method, requestUri);
            foreach (var header in requestHeaders)
                message.Headers.Add(header.Name, header.Values);
            var result = ExecuteRequest(message, cancellation);
            responseParser.Invoke(result);
        }

        protected void Send(Uri requestUri, HttpMethod method, HttpContent content, Action<ResponseData> responseParser, CancellationToken cancellation, params HeaderData[] requestHeaders)
        {
            var message = new HttpRequestMessage(method, requestUri);
            if (content != null)
                message.Content = content;
            if (requestHeaders != null)
                foreach (var header in requestHeaders)
                    message.Headers.Add(header.Name, header.Values);
            var result = ExecuteRequest(message, cancellation);
            responseParser.Invoke(result);
        }

        void IDisposable.Dispose() => _client.Dispose();
    }
}
