namespace ToolKit.Api.Interfaces.Providers.GitHub;

public interface IGitHubPublicUserRepoCommitsProvider
{
    Task<HttpResponseMessage> GetRepoCommits(string owner, string repo);
    Task<HttpResponseMessage> GetRepoCommit(string owner, string repo, string commitId);
}