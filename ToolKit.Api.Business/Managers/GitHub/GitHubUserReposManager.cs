using Octokit;
using ToolKit.Api.Interfaces.Managers.GitHub;
using ToolKit.Api.Interfaces.Providers.GitHub;

namespace ToolKit.Api.Business.Managers.GitHub;

public class GitHubUserReposManager : IGitHubUserReposManager
{
    private readonly IGitHubUserInstallationReposProvider _gitHubUserInstallationReposProvider;

    public GitHubUserReposManager(IGitHubUserInstallationReposProvider gitHubUserInstallationReposProvider)
    {
        _gitHubUserInstallationReposProvider = gitHubUserInstallationReposProvider;
    }

    public async Task<IReadOnlyList<Repository>> GetAllUserRepos(string userToken)
    {
        return await _gitHubUserInstallationReposProvider.GetInstallationRepos(userToken);
    }

    public async Task<Repository> GetUserReposById(string userToken, int repositoryId)
    {
        return await _gitHubUserInstallationReposProvider.GetUserReposById(userToken, repositoryId);
    }
}