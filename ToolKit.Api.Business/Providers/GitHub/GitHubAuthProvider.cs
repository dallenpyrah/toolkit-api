using ToolKit.Api.Interfaces.Providers.GitHub;

namespace ToolKit.Api.Business.Providers.GitHub;

public class GitHubAuthProvider : IGitHubAuthProvider
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GitHubAuthProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<HttpResponseMessage> RetrieveAccessToken(HttpRequestMessage httpRequestMessage)
    {
        HttpClient client = _httpClientFactory.CreateClient("GitHub");
        return await client.SendAsync(httpRequestMessage);
    }
}