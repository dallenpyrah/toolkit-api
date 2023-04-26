using Octokit;

namespace ToolKit.Api.Interfaces.Providers.GitHub;

public interface IGitHubInstallationsProvider
{
    Task<IReadOnlyList<Installation>> GetInstallations();
    Task<Installation> GetInstallationsByUsername(string user);
    Task<AccessToken> GetAccessTokenForInstallation(long installationId);
}