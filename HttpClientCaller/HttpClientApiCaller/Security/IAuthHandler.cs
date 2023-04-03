namespace HttpClientApiCaller.Security
{
    public interface IAuthHandler
    {
        Task<IAuthToken> GetAuthToken(CancellationToken cancellation);
    }
}
