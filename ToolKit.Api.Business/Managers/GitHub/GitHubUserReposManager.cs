using System.Text.Json;
using ToolKit.Api.Business.Exceptions.GitHub;
using ToolKit.Api.Contracts;
using ToolKit.Api.Contracts.GitHub;
using ToolKit.Api.Interfaces.Managers.GitHub;
using ToolKit.Api.Interfaces.Providers.GitHub;

namespace ToolKit.Api.Business.Managers.GitHub;

public class GitHubUserReposManager : IGitHubUserReposManager
{
    private readonly IGitHubUserReposProvider _gitHubUserReposProvider;

    public GitHubUserReposManager(IGitHubUserReposProvider gitHubUserReposProvider)
    {
        _gitHubUserReposProvider = gitHubUserReposProvider;
    }

    public async Task<ApiResponse<IEnumerable<GitHubRepo>>> GetReposByUsername(string username)
    {
        HttpResponseMessage responseMessage = await _gitHubUserReposProvider.GetReposByUsername(username);

        if (!responseMessage.IsSuccessStatusCode)
            throw new RetrieveGitHubUserReposException(responseMessage.StatusCode, responseMessage.ReasonPhrase);

        string responseContent = await responseMessage.Content.ReadAsStringAsync();
        IEnumerable<GitHubRepo> gitHubRepositoryResponses = JsonSerializer.Deserialize<IEnumerable<GitHubRepo>>(responseContent) ?? throw new InvalidOperationException();
        return new ApiResponse<IEnumerable<GitHubRepo>>()
        {
            Body = gitHubRepositoryResponses,
            Message = $"Successfully retrieved GitHub repositories for user {username}.",
        };
    }
}