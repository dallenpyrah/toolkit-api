using ToolKit.Api.Business.Enums.GitHub;
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
        var client = _httpClientFactory.CreateClient(GitHubClient.GitHub.ToString());
        return await client.SendAsync(httpRequestMessage);
    }
}