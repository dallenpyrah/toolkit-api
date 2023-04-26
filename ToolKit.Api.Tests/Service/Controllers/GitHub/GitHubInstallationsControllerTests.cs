using Octokit;

namespace ToolKit.Api.UnitTests.Service.Controllers.GitHub;

public class GitHubInstallationsControllerTests
{
    private readonly Mock<IGitHubInstallationsManager> _gitHubInstallationsManagerMock;
    private readonly Mock<ILogger<GitHubInstallationsController>> _loggerMock;
    private readonly GitHubInstallationsController _gitHubInstallationsController;

    public GitHubInstallationsControllerTests()
    {
        _gitHubInstallationsManagerMock = new Mock<IGitHubInstallationsManager>();
        _loggerMock = new Mock<ILogger<GitHubInstallationsController>>();

        _gitHubInstallationsController = new GitHubInstallationsController(
            _gitHubInstallationsManagerMock.Object,
            _loggerMock.Object
        );
    }

    [Theory]
    [AutoData]
    public async Task GetInstallations_ReturnsOkObjectResult(IEnumerable<Installation> installations)
    {
        var apiResponse = new ApiResponse<IEnumerable<Installation>>
        {
            Body = installations,
            Message = "Success"
        };

        _gitHubInstallationsManagerMock
            .Setup(manager => manager.GetInstallations())
            .ReturnsAsync(apiResponse);

        var result = await _gitHubInstallationsController.GetInstallations();

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Theory]
    [AutoData]
    public async Task GetInstallationByUsername_ReturnsOkObjectResult(string username, Installation installation)
    {
        var apiResponse = new ApiResponse<Installation>
        {
            Body = installation,
            Message = "Success"
        };

        _gitHubInstallationsManagerMock
            .Setup(manager => manager.GetInstallationsByUsername(username))
            .ReturnsAsync(apiResponse);

        var result = await _gitHubInstallationsController.GetInstallationByUsername(username);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetInstallations_WhenExceptionOccurs_ReturnsInternalServerError()
    {
        _gitHubInstallationsManagerMock
            .Setup(manager => manager.GetInstallations())
            .ThrowsAsync(new Exception("An error occurred while fetching installations."));

        var result = await _gitHubInstallationsController.GetInstallations();

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Theory]
    [AutoData]
    public async Task GetInstallationByUsername_WhenExceptionOccurs_ReturnsInternalServerError(string username)
    {
        _gitHubInstallationsManagerMock
            .Setup(manager => manager.GetInstallationsByUsername(username))
            .ThrowsAsync(new Exception("An error occurred while fetching the installation for the specified user."));

        var result = await _gitHubInstallationsController.GetInstallationByUsername(username);

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }
}