namespace ToolKit.Api.Interfaces.Providers;

public interface IGitHubAuthProvider
{
    Task<HttpResponseMessage> RetrieveAccessToken(HttpRequestMessage httpRequestMessage);
}