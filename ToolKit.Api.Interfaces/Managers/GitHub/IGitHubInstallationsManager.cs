using Octokit;
using ToolKit.Api.Contracts;

namespace ToolKit.Api.Interfaces.Managers.GitHub;

public interface IGitHubInstallationsManager
{
    Task<ApiResponse<IEnumerable<Installation>>> GetInstallations();
    Task<ApiResponse<Installation>> GetInstallationsByUsername(string username);
}