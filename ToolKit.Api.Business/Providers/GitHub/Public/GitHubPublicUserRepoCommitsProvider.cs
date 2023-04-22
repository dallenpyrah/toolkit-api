using ToolKit.Api.Business.Enums.GitHub;
using ToolKit.Api.Interfaces.Providers.GitHub;

namespace ToolKit.Api.Business.Providers.GitHub;

public class GitHubPublicUserRepoCommitsProvider : IGitHubPublicUserRepoCommitsProvider
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GitHubPublicUserRepoCommitsProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Task<HttpResponseMessage> GetRepoCommits(string owner, string repo)
    {
        var httpClient = _httpClientFactory.CreateClient(GitHubClient.GitHubApi.ToString());
        return httpClient.GetAsync($"/repos/{owner}/{repo}/commits");
    }

    public Task<HttpResponseMessage> GetRepoCommit(string owner, string repo, string commitId)
    {
        var httpClient = _httpClientFactory.CreateClient(GitHubClient.GitHubApi.ToString());
        return httpClient.GetAsync($"/repos/{owner}/{repo}/commits/{commitId}");
    }
}