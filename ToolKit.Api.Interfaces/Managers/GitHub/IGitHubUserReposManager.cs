using Octokit;

namespace ToolKit.Api.Interfaces.Managers.GitHub;

public interface IGitHubUserReposManager
{
    Task<IReadOnlyList<Repository>> GetAllUserRepos(string userToken);
    Task<Repository> GetUserReposById(string userToken, int repositoryId);
}