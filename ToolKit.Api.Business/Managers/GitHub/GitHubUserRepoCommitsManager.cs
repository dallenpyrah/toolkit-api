using Newtonsoft.Json;
using ToolKit.Api.Business.Exceptions.GitHub;
using ToolKit.Api.Contracts;
using ToolKit.Api.Contracts.GitHub;
using ToolKit.Api.Interfaces.Managers.GitHub;
using ToolKit.Api.Interfaces.Providers.GitHub;

namespace ToolKit.Api.Business.Managers.GitHub;

public class GitHubUserRepoCommitsManager : IGitHubUserRepoCommitsManager
{
    private readonly IGitHubUserRepoCommitsProvider _gitHubUserRepoCommitsProvider;

    public GitHubUserRepoCommitsManager(IGitHubUserRepoCommitsProvider gitHubUserRepoCommitsProvider)
    {
        _gitHubUserRepoCommitsProvider = gitHubUserRepoCommitsProvider;
    }

    public async Task<ApiResponse<IEnumerable<GitHubCommitResponse>>> GetRepoCommits(string owner, string repo)
    {
        var response = await _gitHubUserRepoCommitsProvider.GetRepoCommits(owner, repo);

        if (!response.IsSuccessStatusCode)
            throw new GitHubRepositoryCommitException(response.StatusCode, response.ReasonPhrase);

        var content = await response.Content.ReadAsStringAsync();
        var commits = JsonConvert.DeserializeObject<IEnumerable<GitHubCommitResponse>>(content);
        return new ApiResponse<IEnumerable<GitHubCommitResponse>>()
        {
            Body = commits,
            Message = $"Successfully retrieved commits from {owner}/{repo}"
        };
    }

    public async Task<ApiResponse<GitHubCommitResponse>> GetRepoCommit(string owner, string repo, string commitId)
    {
        var response = await _gitHubUserRepoCommitsProvider.GetRepoCommit(owner, repo, commitId);

        if (!response.IsSuccessStatusCode)
            throw new GitHubRepositoryCommitException(response.StatusCode, response.ReasonPhrase);

        var content = await response.Content.ReadAsStringAsync();
        var commit = JsonConvert.DeserializeObject<GitHubCommitResponse>(content);
        return new ApiResponse<GitHubCommitResponse>()
        {
            Body = commit,
            Message = $"Successfully retrieved commit {commitId} from {owner}/{repo}"
        };
    }
}