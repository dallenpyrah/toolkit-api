using ToolKit.Api.Business.Enums.GitHub;
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
        var client = _httpClientFactory.CreateClient(GitHubClient.GitHubApi.ToString());
        return await client.GetAsync($"/users/{username}/repos");
    }

    public Task<HttpResponseMessage> GetUserRepo(string owner, string repo)
    {
        var client = _httpClientFactory.CreateClient(GitHubClient.GitHubApi.ToString());
        return client.GetAsync($"/repos/{owner}/{repo}");
    }
}