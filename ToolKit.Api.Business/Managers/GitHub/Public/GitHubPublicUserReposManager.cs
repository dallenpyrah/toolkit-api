using Newtonsoft.Json;
using ToolKit.Api.Business.Exceptions.GitHub;
using ToolKit.Api.Contracts;
using ToolKit.Api.Contracts.GitHub;
using ToolKit.Api.Interfaces.Managers.GitHub;
using ToolKit.Api.Interfaces.Providers.GitHub;

namespace ToolKit.Api.Business.Managers.GitHub;

public class GitHubPublicUserReposManager : IGitHubPublicUserReposManager
{
    private readonly IGitHubPublicUserReposProvider _gitHubPublicUserReposProvider;

    public GitHubPublicUserReposManager(IGitHubPublicUserReposProvider gitHubPublicUserReposProvider)
    {
        _gitHubPublicUserReposProvider = gitHubPublicUserReposProvider;
    }

    public async Task<ApiResponse<IEnumerable<GitHubRepo>>> GetReposByUsername(string username)
    {
        var responseMessage = await _gitHubPublicUserReposProvider.GetReposByUsername(username);

        if (!responseMessage.IsSuccessStatusCode)
            throw new GitHubRepositoryException(responseMessage.StatusCode, responseMessage.ReasonPhrase);

        string responseContent = await responseMessage.Content.ReadAsStringAsync();
        var gitHubRepositoryResponses = JsonConvert.DeserializeObject<IEnumerable<GitHubRepo>>(responseContent);
        return new ApiResponse<IEnumerable<GitHubRepo>>()
        {
            Body = gitHubRepositoryResponses,
            Message = $"Successfully retrieved GitHub repositories for user {username}.",
        };
    }

    public async Task<ApiResponse<GitHubRepo>> GetUserRepo(string owner, string repo)
    {
        var responseMessage = await _gitHubPublicUserReposProvider.GetUserRepo(owner, repo);

        if (!responseMessage.IsSuccessStatusCode)
            throw new GitHubRepositoryException(responseMessage.StatusCode, responseMessage.ReasonPhrase);

        var responseContent = await responseMessage.Content.ReadAsStringAsync();
        var gitHubRepositoryResponse = JsonConvert.DeserializeObject<GitHubRepo>(responseContent);
        return new ApiResponse<GitHubRepo>()
        {
            Body = gitHubRepositoryResponse,
            Message = $"Successfully retrieved GitHub repository {repo} for user {owner}."
        };
    }
}