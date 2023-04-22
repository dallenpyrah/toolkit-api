using ToolKit.Api.Contracts;
using ToolKit.Api.Contracts.GitHub;

namespace ToolKit.Api.Interfaces.Managers.GitHub;

public interface IGitHubPublicUserReposManager
{
    Task<ApiResponse<IEnumerable<GitHubRepo>>> GetReposByUsername(string username);
    Task<ApiResponse<GitHubRepo>> GetUserRepo(string owner, string repo);
}