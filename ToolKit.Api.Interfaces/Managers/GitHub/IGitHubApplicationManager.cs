using Octokit;

namespace ToolKit.Api.Interfaces.Managers.GitHub;

public interface IGitHubApplicationManager
{
    Task<GitHubApp> GetAuthenticatedApp();
}