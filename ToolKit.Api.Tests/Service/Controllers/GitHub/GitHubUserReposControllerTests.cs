using ToolKit.Api.Business.Exceptions.GitHub;
using ToolKit.Api.Interfaces.Managers.GitHub;

namespace ToolKit.Api.UnitTests.Service.Controllers.GitHub;

public class GitHubUserReposControllerTests
{
    private readonly Mock<IGitHubUserReposManager> _gitHubUserReposManagerMock;
    private readonly Mock<ILogger<GitHubUserReposController>> _loggerMock;
    private readonly GitHubUserReposController _gitHubUserReposController;

    public GitHubUserReposControllerTests()
    {
        _gitHubUserReposManagerMock = new Mock<IGitHubUserReposManager>();
        _loggerMock = new Mock<ILogger<GitHubUserReposController>>();
        _gitHubUserReposController =
            new GitHubUserReposController(_gitHubUserReposManagerMock.Object, _loggerMock.Object);
    }

    [Theory, AutoData]
    public async Task GetReposByUsername_ReturnsOkWithRepos(string username, List<GitHubRepo> repos)
    {
        _gitHubUserReposManagerMock
            .Setup(manager => manager.GetReposByUsername(username))
            .ReturnsAsync(new ApiResponse<IEnumerable<GitHubRepo>>()
            {
                Body = repos
            });

        IActionResult result = await _gitHubUserReposController.GetReposByUsername(username);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
    }

    [Theory, AutoData]
    public async Task GetReposByUsername_BodyContainsRepos(string username, List<GitHubRepo> repos)
    {
        _gitHubUserReposManagerMock
            .Setup(manager => manager.GetReposByUsername(username))
            .ReturnsAsync(new ApiResponse<IEnumerable<GitHubRepo>>()
            {
                Body = repos
            });

        IActionResult result = await _gitHubUserReposController.GetReposByUsername(username);
        var okObjectResult = result as OkObjectResult;

        var apiResponse = Assert.IsType<ApiResponse<IEnumerable<GitHubRepo>>>(okObjectResult.Value);
        Assert.Equal(repos, apiResponse.Body);
    }

    [Theory, AutoData]
    public async Task GetReposByUsername_ThrowsRetrieveGitHubUserReposException_ReturnsInternalServerError(
        string username, HttpStatusCode statusCode, string errorMessage)
    {
        _gitHubUserReposManagerMock
            .Setup(manager => manager.GetReposByUsername(username))
            .ThrowsAsync(new GitHubRepositoryException(statusCode, errorMessage));

        IActionResult result = await _gitHubUserReposController.GetReposByUsername(username);

        Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
    }

    [Theory, AutoData]
    public async Task GetReposByUsername_ThrowsException_ReturnsInternalServerError(string username)
    {
        _gitHubUserReposManagerMock
            .Setup(manager => manager.GetReposByUsername(username))
            .ThrowsAsync(new Exception());

        IActionResult result = await _gitHubUserReposController.GetReposByUsername(username);
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
    }

    [Theory]
    [AutoData]
    public async Task GetRepo_SuccessfulResponse_ReturnsOkResult(string owner, string repo)
    {
        var gitHubRepo = new GitHubRepo()
        {
            Name = "Test Repo"
        };

        var response = new ApiResponse<GitHubRepo>()
        {
            Body = gitHubRepo,
            Message = "Successfully retrieved GitHub repository."
        };

        _gitHubUserReposManagerMock
            .Setup(manager => manager.GetUserRepo(owner, repo))
            .ReturnsAsync(response);

        var result = await _gitHubUserReposController.GetRepo(owner, repo);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okObjectResult.Value);
    }

    [Theory]
    [AutoData]
    public async Task GetRepo_RetrieveGitHubUserReposException_ReturnsInternalServerError(string owner, string repo)
    {
        _gitHubUserReposManagerMock
            .Setup(manager => manager.GetUserRepo(owner, repo))
            .ThrowsAsync(new GitHubRepositoryException(HttpStatusCode.InternalServerError,
                "Error retrieving repository."));

        var result = await _gitHubUserReposController.GetRepo(owner, repo);

        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        Assert.Equal("Error retrieving repository.", statusCodeResult.Value);
    }

    [Theory]
    [AutoData]
    public async Task GetRepo_GeneralException_ReturnsInternalServerError(string owner, string repo)
    {
        _gitHubUserReposManagerMock
            .Setup(manager => manager.GetUserRepo(owner, repo))
            .ThrowsAsync(new Exception("General error."));

        var result = await _gitHubUserReposController.GetRepo(owner, repo);

        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        Assert.Equal("General error.", statusCodeResult.Value);
    }
}