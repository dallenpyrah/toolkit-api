using Octokit;

namespace ToolKit.Api.Interfaces.Factories;

public interface IGitHubClientFactory
{
    GitHubClient GetGitHubClientWithJwtToken();
    GitHubClient GetGitHubClientWithUserToken(string userToken);
}