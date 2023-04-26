using Octokit;

namespace ToolKit.Api.UnitTests.Service.Controllers.GitHub;

public class GitHubApplicationControllerTests
{
    private readonly Mock<IGitHubApplicationManager> _gitHubApplicationManagerMock;
    private readonly Mock<ILogger<GitHubApplicationController>> _loggerMock;
    private readonly GitHubApplicationController _gitHubApplicationController;

    public GitHubApplicationControllerTests()
    {
        _gitHubApplicationManagerMock = new Mock<IGitHubApplicationManager>();
        _loggerMock = new Mock<ILogger<GitHubApplicationController>>();
        _gitHubApplicationController =
            new GitHubApplicationController(_gitHubApplicationManagerMock.Object, _loggerMock.Object);
    }

    [Theory]
    [AutoData]
    public async Task GetAuthenticatedApp_ReturnsObjectResult(ApiResponse<GitHubApp> gitHubApp)
    {
        _gitHubApplicationManagerMock
            .Setup(manager => manager.GetAuthenticatedApp())
            .ReturnsAsync(gitHubApp);

        var result = await _gitHubApplicationController.GetAuthenticatedApp();

        Assert.IsType<OkObjectResult>(result.Result);
    }


    [Theory]
    [AutoData]
    public async Task GetAuthenticatedApp_Returns200StatusCode(ApiResponse<GitHubApp> gitHubApp)
    {
        _gitHubApplicationManagerMock
            .Setup(manager => manager.GetAuthenticatedApp())
            .ReturnsAsync(gitHubApp);

        var result = await _gitHubApplicationController.GetAuthenticatedApp();

        Assert.Equal(200, (result.Result as ObjectResult)?.StatusCode);
    }

    [Theory]
    [AutoData]
    public async Task GetAuthenticatedApp_ReturnsAppInResponse(ApiResponse<GitHubApp> gitHubApp)
    {
        _gitHubApplicationManagerMock
            .Setup(manager => manager.GetAuthenticatedApp())
            .ReturnsAsync(gitHubApp);

        var result = await _gitHubApplicationController.GetAuthenticatedApp();
        var okObjectResult = result.Result as OkObjectResult;
        var apiResponse = okObjectResult.Value as ApiResponse<GitHubApp>;

        Assert.Equal(JsonConvert.SerializeObject(gitHubApp.Body), JsonConvert.SerializeObject(apiResponse.Body));
    }

    [Fact]
    public async Task GetAuthenticatedApp_ThrowsException_ReturnsStatusCodeResult()
    {
        _gitHubApplicationManagerMock
            .Setup(manager => manager.GetAuthenticatedApp())
            .ThrowsAsync(new Exception("Error occurred."));

        var result = await _gitHubApplicationController.GetAuthenticatedApp();

        Assert.IsType<ObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetAuthenticatedApp_ThrowsException_Returns500StatusCode()
    {
        _gitHubApplicationManagerMock
            .Setup(manager => manager.GetAuthenticatedApp())
            .ThrowsAsync(new Exception("Error occurred."));

        var result = await _gitHubApplicationController.GetAuthenticatedApp();

        Assert.Equal(500, (result.Result as ObjectResult).StatusCode);
    }
}