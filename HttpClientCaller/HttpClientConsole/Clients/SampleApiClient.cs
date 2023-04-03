using HttpClientApiCaller.Client;
using HttpClientApiCaller.Exceptions;
using HttpClientApiCaller.Security;
using HttpClientConsole.Configuration;
using HttpClientConsole.Routes;
using Microsoft.Extensions.Options;
using NLog;
using System.Net;

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

        public Response<IList<Models.Sample>>? GetData(CancellationToken cancellation)
        {
            string endpointUri = SampleEndpoints.GetAll;
            Response<IList<Models.Sample>>? response = null;
            try
            {
                Send(new Uri(endpointUri, UriKind.Relative),
                                     HttpMethod.Get,
                                     result =>
                                     {
                                         if (result.StatusCode == (int)HttpStatusCode.OK)
                                             response = new Response<IList<Models.Sample>>(result, result.StatusCode);
                                         else
                                             throw new GeneralApplicationException(result.Content);
                                     },
                                     cancellation);

                return response;
            }
            catch (Exception ex)
            {
                //Logger.Log(LogLevel.Error, "Get Employee Failed!", ex.Message);
                throw new GeneralApplicationException("Get Sample Failed!", ex);
            }
        }
    }
}
