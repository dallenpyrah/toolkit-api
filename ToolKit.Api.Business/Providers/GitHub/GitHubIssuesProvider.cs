using Octokit;
using ToolKit.Api.Interfaces.Factories;
using ToolKit.Api.Interfaces.Providers.GitHub;

namespace ToolKit.Api.Business.Providers.GitHub;

public class GitHubIssuesProvider : IGitHubIssuesProvider
{
    private readonly IGitHubClientFactory _gitHubClientFactory;

    public GitHubIssuesProvider(IGitHubClientFactory gitHubClientFactory)
    {
        _gitHubClientFactory = gitHubClientFactory;
    }

    public Task<IReadOnlyList<Issue>> GetAllUserIssues(string userToken)
    {
        var client = _gitHubClientFactory.GetGitHubClientWithUserToken(userToken);
        return client.Issue.GetAllForCurrent();
    }
}