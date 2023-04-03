namespace HttpClientApiCaller.Security
{
    public interface IAuthHandler
    {
        IAuthToken GetAuthToken(CancellationToken cancellation);
    }
}
