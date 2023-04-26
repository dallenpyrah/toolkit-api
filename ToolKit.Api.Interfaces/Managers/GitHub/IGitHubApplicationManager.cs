using Octokit;
using ToolKit.Api.Contracts;

namespace ToolKit.Api.Interfaces.Managers.GitHub;

public interface IGitHubApplicationManager
{
    Task<ApiResponse<GitHubApp>> GetAuthenticatedApp();
}