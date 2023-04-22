namespace ToolKit.Api.Interfaces.Providers.GitHub;

public interface IGitHubPublicUserReposProvider
{
    Task<HttpResponseMessage> GetReposByUsername(string username);
    Task<HttpResponseMessage> GetUserRepo(string owner, string repo);
}