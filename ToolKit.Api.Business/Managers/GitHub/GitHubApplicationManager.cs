using Octokit;
using ToolKit.Api.Contracts;
using ToolKit.Api.Interfaces.Managers.GitHub;
using ToolKit.Api.Interfaces.Providers.GitHub;

namespace ToolKit.Api.Business.Managers.GitHub;

public class GitHubApplicationManager : IGitHubApplicationManager
{
    private readonly IGitHubApplicationProvider _gitHubApplicationProvider;

    public GitHubApplicationManager(IGitHubApplicationProvider gitHubApplicationProvider)
    {
        _gitHubApplicationProvider = gitHubApplicationProvider;
    }

    public async Task<ApiResponse<GitHubApp>> GetAuthenticatedApp()
    {
        var gitHubApp = await _gitHubApplicationProvider.GetAuthenticatedApp();
        return new ApiResponse<GitHubApp>()
        {
            Body = gitHubApp,
            Message = "GitHub App retrieved successfully."
        };
    }
}