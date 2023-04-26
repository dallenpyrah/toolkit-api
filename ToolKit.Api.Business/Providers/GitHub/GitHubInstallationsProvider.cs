using Octokit;
using ToolKit.Api.Interfaces.Factories;
using ToolKit.Api.Interfaces.Providers.GitHub;

namespace ToolKit.Api.Business.Providers.GitHub;

public class GitHubInstallationsProvider : IGitHubInstallationsProvider
{
    private readonly IGitHubClientFactory _gitHubClientFactory;

    public GitHubInstallationsProvider(IGitHubClientFactory gitHubClientFactory)
    {
        _gitHubClientFactory = gitHubClientFactory;
    }

    public async Task<IReadOnlyList<Installation>> GetInstallations()
    {
        var client = _gitHubClientFactory.GetGitHubClientWithJwtToken();
        return await client.GitHubApps.GetAllInstallationsForCurrent();
    }

    public Task<Installation> GetInstallationsByUsername(string user)
    {
        var client = _gitHubClientFactory.GetGitHubClientWithJwtToken();
        return client.GitHubApps.GetUserInstallationForCurrent(user);
    }

    public async Task<AccessToken> GetAccessTokenForInstallation(long installationId)
    {
        var client = _gitHubClientFactory.GetGitHubClientWithJwtToken();
        return await client.GitHubApps.CreateInstallationToken(installationId);
    }
}