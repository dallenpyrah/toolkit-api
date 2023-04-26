using Octokit;

namespace ToolKit.Api.Interfaces.Providers.GitHub;

public interface IGitHubApplicationProvider
{
    Task<GitHubApp> GetAuthenticatedApp();
}