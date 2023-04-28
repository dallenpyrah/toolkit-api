using Octokit;

namespace ToolKit.Api.Interfaces.Providers.GitHub;

public interface IGitHubUserInstallationReposProvider
{
    Task<IReadOnlyList<Repository>> GetInstallationRepos(string userToken);
    Task<Repository> GetUserReposById(string userToken, int repositoryId);
}