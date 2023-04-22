using ToolKit.Api.Contracts;
using ToolKit.Api.Contracts.GitHub;

namespace ToolKit.Api.Interfaces.Managers.GitHub;

public interface IGitHubUserRepoCommitsManager
{
    Task<ApiResponse<IEnumerable<GitHubCommitResponse>>> GetRepoCommits(string owner, string repo);
    Task<ApiResponse<GitHubCommitResponse>> GetRepoCommit(string owner, string repo, string commitId);
}