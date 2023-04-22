using ToolKit.Api.Interfaces.Providers.GitHub;

namespace ToolKit.Api.Business.Providers.GitHub;

public class GitHubUserReposProvider : IGitHubUserReposProvider
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GitHubUserReposProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HttpResponseMessage> GetReposByUsername(string username)
    {
        HttpClient client = _httpClientFactory.CreateClient("GitHub API");
        return await client.GetAsync($"/users/{username}/repos");
    }
}