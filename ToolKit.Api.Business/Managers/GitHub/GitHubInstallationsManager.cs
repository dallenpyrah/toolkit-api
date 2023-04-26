using Octokit;
using ToolKit.Api.Contracts;
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

    public async Task<ApiResponse<IEnumerable<Installation>>> GetInstallations()
    {
        var installations = await _gitHubInstallationsProvider.GetInstallations();
        return new ApiResponse<IEnumerable<Installation>>()
        {
            Body = installations,
            Message = "GitHub Installations retrieved successfully."
        };
    }

    public async Task<ApiResponse<Installation>> GetInstallationsByUsername(string user)
    {
        var installation = await _gitHubInstallationsProvider.GetInstallationsByUsername(user);
        return new ApiResponse<Installation>()
        {
            Body = installation,
            Message = $"GitHub installation retrieved for {user} successfully."
        };
    }

    public async Task<ApiResponse<AccessToken>> GetAccessTokenForInstallation(long installationId)
    {
        var accessToken = await _gitHubInstallationsProvider.GetAccessTokenForInstallation(installationId);
        return new ApiResponse<AccessToken>()
        {
            Body = accessToken,
            Message = $"GitHub access token retrieved for installation {installationId} successfully."
        };
    }
}