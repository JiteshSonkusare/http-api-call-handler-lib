using NLog;
using System.Net;
using HttpClientConsole.Routes;
using HttpClientApiCaller.Client;
using HttpClientApiCaller.Security;
using HttpClientApiCaller.Exceptions;
using HttpClientConsole.Configuration;

namespace HttpClientConsole.Clients
{
    internal class SampleApiClient : ApiClientBase
    {
        public SampleApiClient(ILogger logger, SampleApiClientConfig config, IAuthHandler authHandler) : base(logger, config, authHandler)
        {
            if (config == null)
            {
                logger.Log(LogLevel.Error, $"Sample api client is null!");
                throw new ArgumentNullException(nameof(config));
            }
        }

        public async Task<Response<IList<Models.Sample>>>? GetData(CancellationToken cancellation)
        {
            string endpointUri = SampleEndpoints.GetAll;
            ResponseData result = await Send(
                    new Uri(endpointUri, UriKind.Relative),
                    HttpMethod.Get,
                    null,
                    cancellation,
                    Array.Empty<HeaderData>()
                ).ConfigureAwait(false);

            if (result.StatusCode != (int)HttpStatusCode.OK)
            {
                throw new GeneralApplicationException(result.Content);
            }

            Response<IList<Models.Sample>>? response = new Response<IList<Models.Sample>>(result, result.StatusCode);
            return response;
        }

        protected override DefaultRequestHeaders GetDefaultRequestHeaders()
        {
            return new DefaultRequestHeaders(new string[] { System.Net.Mime.MediaTypeNames.Application.Json }, Array.Empty<HeaderData>());
        }
    }
}
