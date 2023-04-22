namespace ToolKit.Api.Interfaces.Providers.GitHub;

public interface IGitHubAuthProvider
{
    Task<HttpResponseMessage> RetrieveAccessToken(HttpRequestMessage httpRequestMessage);
}