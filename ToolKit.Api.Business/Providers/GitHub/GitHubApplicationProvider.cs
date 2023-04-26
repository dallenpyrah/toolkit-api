using Octokit;
using ToolKit.Api.Interfaces.Factories;
using ToolKit.Api.Interfaces.Providers.GitHub;

namespace ToolKit.Api.Business.Providers.GitHub;

public class GitHubApplicationProvider : IGitHubApplicationProvider
{
    private readonly IGitHubClientFactory _gitHubClientFactory;

    public GitHubApplicationProvider(IGitHubClientFactory gitHubClientFactory)
    {
        _gitHubClientFactory = gitHubClientFactory;
    }

    public async Task<GitHubApp> GetAuthenticatedApp()
    {
        var client = _gitHubClientFactory.GetGitHubClientWithJwtToken();
        return await client.GitHubApps.GetCurrent();
    }
}