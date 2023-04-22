using System.Text;
using ToolKit.Api.Business.Managers.GitHub;
using ToolKit.Api.Interfaces.Providers.GitHub;

namespace ToolKit.Api.UnitTests.Business.Managers.GitHub.Public;

public class GitHubPublicUserRepoCommitsManagerTests
{
    private readonly Mock<IGitHubPublicUserRepoCommitsProvider> _gitHubPublicUserRepoCommitsProviderMock;
    private readonly GitHubPublicUserRepoCommitsManager _gitHubPublicUserRepoCommitsManager;

    public GitHubPublicUserRepoCommitsManagerTests()
    {
        _gitHubPublicUserRepoCommitsProviderMock = new Mock<IGitHubPublicUserRepoCommitsProvider>();
        _gitHubPublicUserRepoCommitsManager =
            new GitHubPublicUserRepoCommitsManager(_gitHubPublicUserRepoCommitsProviderMock.Object);
    }

    [Theory]
    [AutoData]
    public async Task GetRepoCommits_ReturnsApiResponse(string owner, string repo,
        List<GitHubCommitResponse> gitHubCommitResponses)
    {
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(gitHubCommitResponses), Encoding.UTF8,
                "application/json")
        };

        _gitHubPublicUserRepoCommitsProviderMock
            .Setup(provider => provider.GetRepoCommits(owner, repo))
            .ReturnsAsync(httpResponse);

        var result = await _gitHubPublicUserRepoCommitsManager.GetRepoCommits(owner, repo);

        Assert.Equal(JsonConvert.SerializeObject(gitHubCommitResponses), JsonConvert.SerializeObject(result.Body));
    }

    [Theory]
    [AutoData]
    public async Task GetRepoCommits_ReturnsSuccessMessage(string owner, string repo,
        List<GitHubCommitResponse> gitHubCommitResponses)
    {
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(gitHubCommitResponses), Encoding.UTF8,
                "application/json")
        };

        _gitHubPublicUserRepoCommitsProviderMock
            .Setup(provider => provider.GetRepoCommits(owner, repo))
            .ReturnsAsync(httpResponse);

        var result = await _gitHubPublicUserRepoCommitsManager.GetRepoCommits(owner, repo);

        Assert.Equal($"Successfully retrieved commits from {owner}/{repo}", result.Message);
    }

    [Theory]
    [AutoData]
    public async Task GetRepoCommit_ReturnsApiResponse(string owner, string repo, string commitId,
        GitHubCommitResponse gitHubCommitResponse)
    {
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(gitHubCommitResponse), Encoding.UTF8,
                "application/json")
        };

        _gitHubPublicUserRepoCommitsProviderMock
            .Setup(provider => provider.GetRepoCommit(owner, repo, commitId))
            .ReturnsAsync(httpResponse);

        var result = await _gitHubPublicUserRepoCommitsManager.GetRepoCommit(owner, repo, commitId);

        Assert.Equal(JsonConvert.SerializeObject(gitHubCommitResponse), JsonConvert.SerializeObject(result.Body));
    }

    [Theory]
    [AutoData]
    public async Task GetRepoCommit_ReturnsSuccessMessage(string owner, string repo, string commitId,
        GitHubCommitResponse gitHubCommitResponse)
    {
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(gitHubCommitResponse), Encoding.UTF8,
                "application/json")
        };

        _gitHubPublicUserRepoCommitsProviderMock
            .Setup(provider => provider.GetRepoCommit(owner, repo, commitId))
            .ReturnsAsync(httpResponse);

        var result = await _gitHubPublicUserRepoCommitsManager.GetRepoCommit(owner, repo, commitId);

        Assert.Equal($"Successfully retrieved commit {commitId} from {owner}/{repo}", result.Message);
    }
}