namespace ToolKit.Api.Interfaces.Providers.GitHub;

public interface IGitHubUserReposProvider
{
    Task<HttpResponseMessage> GetReposByUsername(string username);
}