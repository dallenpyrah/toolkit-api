using Octokit;
using ToolKit.Api.Interfaces.Factories;
using ToolKit.Api.Interfaces.Managers.GitHub;

namespace ToolKit.Api.Business.Factories;

public class GitHubClientFactory : IGitHubClientFactory
{
    private readonly string _jwtToken;

    public GitHubClientFactory(IGitHubJwtTokenManager gitHubJwtTokenManager)
    {
        _jwtToken = gitHubJwtTokenManager.GenerateJwtToken();
    }

    public GitHubClient GetGitHubClientWithJwtToken()
    {
        var client = new GitHubClient(new ProductHeaderValue("toolkit-desktop"))
        {
            Credentials = new Credentials(_jwtToken, AuthenticationType.Bearer)
        };
        return client;
    }

    public GitHubClient GetGitHubClientWithUserToken(string userToken)
    {
        var client = new GitHubClient(new ProductHeaderValue("toolkit-desktop"))
        {
            Credentials = new Credentials(userToken, AuthenticationType.Bearer)
        };
        return client;
    }
}