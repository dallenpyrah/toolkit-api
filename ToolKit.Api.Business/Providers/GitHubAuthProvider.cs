using Newtonsoft.Json;
using ToolKit.Api.Contracts;
using ToolKit.Api.Interfaces.Providers;

namespace ToolKit.Api.Business.Providers;

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