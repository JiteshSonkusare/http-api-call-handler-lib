using System.Net.Http.Headers;

namespace HttpClientApiCaller.Security
{
    internal static class AuthExtensions
    {
        public static AuthenticationHeaderValue GetAuthorizationHeader(this IAuthToken token)
            => new AuthenticationHeaderValue(token.Scheme, token.Value);
    }
}
