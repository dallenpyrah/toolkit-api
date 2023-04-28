using Octokit;

namespace ToolKit.Api.Interfaces.Managers.GitHub;

public interface IGitHubInstallationsManager
{
    Task<IReadOnlyList<Installation>> GetInstallations();
    Task<Installation> GetInstallationsByUsername(string username);
}