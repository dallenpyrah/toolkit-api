using Octokit;

namespace ToolKit.Api.Interfaces.Managers.GitHub;

public interface IGitHubIssuesManager
{
    Task<IReadOnlyList<Issue>> GetUserIssues(string userToken);
}