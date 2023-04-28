using Octokit;

namespace ToolKit.Api.Interfaces.Providers.GitHub;

public interface IGitHubIssuesProvider
{
    Task<IReadOnlyList<Issue>> GetAllUserIssues(string userToken);
}