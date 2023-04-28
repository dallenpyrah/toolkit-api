using Octokit;
using ToolKit.Api.Interfaces.Managers.GitHub;
using ToolKit.Api.Interfaces.Providers.GitHub;

namespace ToolKit.Api.Business.Managers.GitHub;

public class GitHubUserReposManager : IGitHubUserReposManager
{
    private readonly IGitHubUserReposProvider _gitHubUserReposProvider;

    public GitHubUserReposManager(IGitHubUserReposProvider gitHubUserReposProvider)
    {
        _gitHubUserReposProvider = gitHubUserReposProvider;
    }

    public async Task<IReadOnlyList<Repository>> GetAllUserRepos(string userToken)
    {
        return await _gitHubUserReposProvider.GetInstallationRepos(userToken);
    }

    public async Task<Repository> GetUserReposById(string userToken, int repositoryId)
    {
        return await _gitHubUserReposProvider.GetUserReposById(userToken, repositoryId);
    }
}