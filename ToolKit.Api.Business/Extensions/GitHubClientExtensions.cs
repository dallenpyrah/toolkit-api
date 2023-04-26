using Octokit;

namespace ToolKit.Api.Business.Extensions;

public static class GitHubClientExtensions
{
    public static GitHubClient CreateGitHubClient(string accessToken)
    {
        var client = new GitHubClient(new ProductHeaderValue("ToolKit Desktop"))
        {
            Credentials = new Credentials(accessToken)
        };
        return client;
    }
}