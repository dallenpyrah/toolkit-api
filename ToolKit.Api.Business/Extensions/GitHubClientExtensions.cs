using System.Net.Mail;
using System.Text.RegularExpressions;
using Octokit;
using ToolKit.Api.Business.Exceptions;
using ToolKit.Api.Contracts;
using ToolKit.Api.DataModel.Entities;

namespace ToolKit.Api.Business.Extensions;

public static class GitHubClientExtensions
{
    public static GitHubClient CreateGitHubClient(this GitHubClient gitHubClient, string accessToken)
    {
        GitHubClient client = new GitHubClient(new ProductHeaderValue("ToolKit Desktop"));
        client.Credentials = new Credentials(accessToken);
        return client;
    }

}