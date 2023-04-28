using Octokit;
using ToolKit.Api.Interfaces.Managers.GitHub;
using ToolKit.Api.Interfaces.Providers.GitHub;

namespace ToolKit.Api.Business.Managers.GitHub;

public class GitHubIssuesManager : IGitHubIssuesManager
{
    private readonly IGitHubIssuesProvider _gitHubIssuesProvider;

    public GitHubIssuesManager(IGitHubIssuesProvider gitHubIssuesProvider)
    {
        _gitHubIssuesProvider = gitHubIssuesProvider;
    }

    public Task<IReadOnlyList<Issue>> GetUserIssues(string userToken)
    {
        return _gitHubIssuesProvider.GetAllUserIssues(userToken);
    }
}