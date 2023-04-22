using ToolKit.Api.Interfaces.Managers.GitHub;

namespace ToolKit.Api.UnitTests.Service.Controllers.GitHub;

public class GitHubPublicUserRepoCommitsControllerTests
{
    private readonly Mock<IGitHubPublicUserRepoCommitsManager> _gitHubUserRepoCommitsManagerMock;
    private readonly Mock<ILogger<GitHubPublicUserRepoCommitsController>> _loggerMock;
    private readonly GitHubPublicUserRepoCommitsController _controller;

    public GitHubPublicUserRepoCommitsControllerTests()
    {
        _gitHubUserRepoCommitsManagerMock = new Mock<IGitHubPublicUserRepoCommitsManager>();
        _loggerMock = new Mock<ILogger<GitHubPublicUserRepoCommitsController>>();
        _controller =
            new GitHubPublicUserRepoCommitsController(_gitHubUserRepoCommitsManagerMock.Object, _loggerMock.Object);
    }

    [Theory]
    [AutoData]
    public async Task GetRepoCommits_ReturnsOkObjectResult(string owner, string repo,
        List<GitHubCommitResponse> gitHubCommitResponses)
    {
        var apiResponse = new ApiResponse<IEnumerable<GitHubCommitResponse>>()
        {
            Body = gitHubCommitResponses,
            Message = "Success"
        };
        _gitHubUserRepoCommitsManagerMock
            .Setup(manager => manager.GetRepoCommits(owner, repo))
            .ReturnsAsync(apiResponse);

        var result = await _controller.GetRepoCommits(owner, repo);

        Assert.IsType<OkObjectResult>(result);
    }

    [Theory]
    [AutoData]
    public async Task GetRepoCommits_ReturnsApiResponse(string owner, string repo,
        List<GitHubCommitResponse> gitHubCommitResponses)
    {
        var apiResponse = new ApiResponse<IEnumerable<GitHubCommitResponse>>()
        {
            Body = gitHubCommitResponses,
            Message = "Success"
        };
        _gitHubUserRepoCommitsManagerMock
            .Setup(manager => manager.GetRepoCommits(owner, repo))
            .ReturnsAsync(apiResponse);

        var result = await _controller.GetRepoCommits(owner, repo) as OkObjectResult;

        Assert.Equal(apiResponse, result.Value);
    }

    [Theory]
    [AutoData]
    public async Task GetRepoCommit_ReturnsOkObjectResult(string owner, string repo, string commitId,
        GitHubCommitResponse gitHubCommitResponse)
    {
        var apiResponse = new ApiResponse<GitHubCommitResponse>()
        {
            Body = gitHubCommitResponse,
            Message = "Success"
        };
        _gitHubUserRepoCommitsManagerMock
            .Setup(manager => manager.GetRepoCommit(owner, repo, commitId))
            .ReturnsAsync(apiResponse);

        var result = await _controller.GetRepoCommit(owner, repo, commitId);

        Assert.IsType<OkObjectResult>(result);
    }

    [Theory]
    [AutoData]
    public async Task GetRepoCommit_ReturnsApiResponse(string owner, string repo, string commitId,
        GitHubCommitResponse gitHubCommitResponse)
    {
        var apiResponse = new ApiResponse<GitHubCommitResponse>()
        {
            Body = gitHubCommitResponse,
            Message = "Success"
        };
        _gitHubUserRepoCommitsManagerMock
            .Setup(manager => manager.GetRepoCommit(owner, repo, commitId))
            .ReturnsAsync(apiResponse);

        var result = await _controller.GetRepoCommit(owner, repo, commitId) as OkObjectResult;

        Assert.Equal(apiResponse, result.Value);
    }
}