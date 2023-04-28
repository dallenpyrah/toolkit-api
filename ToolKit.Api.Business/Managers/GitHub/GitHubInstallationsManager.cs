using Octokit;
using ToolKit.Api.Interfaces.Managers.GitHub;
using ToolKit.Api.Interfaces.Providers.GitHub;

namespace ToolKit.Api.Business.Managers.GitHub;

public class GitHubInstallationsManager : IGitHubInstallationsManager
{
    private readonly IGitHubInstallationsProvider _gitHubInstallationsProvider;

    public GitHubInstallationsManager(IGitHubInstallationsProvider gitHubInstallationsProvider)
    {
        _gitHubInstallationsProvider = gitHubInstallationsProvider;
    }

    public async Task<IReadOnlyList<Installation>> GetInstallations()
    {
        return await _gitHubInstallationsProvider.GetInstallations();
    }

    public async Task<Installation> GetInstallationsByUsername(string user)
    {
        return await _gitHubInstallationsProvider.GetInstallationsByUsername(user);
    }
}