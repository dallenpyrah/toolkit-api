using Octokit;
using ToolKit.Api.Interfaces.Factories;
using ToolKit.Api.Interfaces.Providers.GitHub;

namespace ToolKit.Api.Business.Providers.GitHub;

public class GitHubUserReposProvider : IGitHubUserReposProvider
{
    private readonly IGitHubClientFactory _gitHubClientFactory;

    public GitHubUserReposProvider(IGitHubClientFactory gitHubClientFactory)
    {
        _gitHubClientFactory = gitHubClientFactory;
    }

    public Task<IReadOnlyList<Repository>> GetInstallationRepos(string userToken)
    {
        var client = _gitHubClientFactory.GetGitHubClientWithUserToken(userToken);
        return client.Repository.GetAllForCurrent();
    }

    public Task<Repository> GetUserReposById(string userToken, int repositoryId)
    {
        var client = _gitHubClientFactory.GetGitHubClientWithUserToken(userToken);
        return client.Repository.Get(repositoryId);
    }
}