using ToolKit.Api.Interfaces.Managers.GitHub;

namespace ToolKit.Api.UnitTests.Service.Controllers.GitHub;

public class GitHubUserRepoCommitsControllerTests
{
    private readonly Mock<IGitHubUserRepoCommitsManager> _gitHubUserRepoCommitsManagerMock;
    private readonly Mock<ILogger<GitHubUserRepoCommitsController>> _loggerMock;
    private readonly GitHubUserRepoCommitsController _controller;

    public GitHubUserRepoCommitsControllerTests()
    {
        _gitHubUserRepoCommitsManagerMock = new Mock<IGitHubUserRepoCommitsManager>();
        _loggerMock = new Mock<ILogger<GitHubUserRepoCommitsController>>();
        _controller = new GitHubUserRepoCommitsController(_gitHubUserRepoCommitsManagerMock.Object, _loggerMock.Object);
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