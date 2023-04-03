using HttpClientApiCaller.Client;
using HttpClientApiCaller.Exceptions;
using HttpClientApiCaller.Security;
using HttpClientConsole.Configuration;
using HttpClientConsole.Routes;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientConsole.Clients
{
    internal class AzureApiClient : ApiClientBase
    {
        public AzureApiClient(ILogger logger, AzureApiClientConfig config, IAuthHandler authHandler) : base(logger, config, authHandler)
        {
            if (config == null)
            {
                logger.Log(LogLevel.Error, $"Sample api config is null!");
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
                Logger.Log(LogLevel.Error, "Get Employee Failed!", ex.Message);
                throw new GeneralApplicationException("Get Sample Failed!", ex);
            }
        }
    }
}
