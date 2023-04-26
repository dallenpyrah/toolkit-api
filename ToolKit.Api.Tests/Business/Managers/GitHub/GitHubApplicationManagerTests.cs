using Octokit;

namespace ToolKit.Api.UnitTests.Business.Managers.GitHub;

public class GitHubApplicationManagerTests
{
    private readonly Mock<IGitHubApplicationProvider> _gitHubApplicationProviderMock;
    private readonly GitHubApplicationManager _gitHubApplicationManager;

    public GitHubApplicationManagerTests()
    {
        _gitHubApplicationProviderMock = new Mock<IGitHubApplicationProvider>();
        _gitHubApplicationManager = new GitHubApplicationManager(_gitHubApplicationProviderMock.Object);
    }

    [Theory]
    [AutoData]
    public async Task GetAuthenticatedApp_ReturnsApiResponseWithGitHubApp(GitHubApp gitHubApp)
    {
        _gitHubApplicationProviderMock
            .Setup(provider => provider.GetAuthenticatedApp())
            .ReturnsAsync(gitHubApp);

        var response = await _gitHubApplicationManager.GetAuthenticatedApp();

        Assert.Equal(gitHubApp, response.Body);
    }

    [Theory]
    [AutoData]
    public async Task GetAuthenticatedApp_ReturnsApiResponseWithMessage(GitHubApp gitHubApp)
    {
        _gitHubApplicationProviderMock
            .Setup(provider => provider.GetAuthenticatedApp())
            .ReturnsAsync(gitHubApp);

        var response = await _gitHubApplicationManager.GetAuthenticatedApp();

        Assert.Equal("GitHub App retrieved successfully.", response.Message);
    }

    [Theory]
    [AutoData]
    public async Task GetAuthenticatedApp_CallsGetAuthenticatedAppOnProvider(GitHubApp gitHubApp)
    {
        _gitHubApplicationProviderMock
            .Setup(provider => provider.GetAuthenticatedApp())
            .ReturnsAsync(gitHubApp);

        await _gitHubApplicationManager.GetAuthenticatedApp();

        _gitHubApplicationProviderMock.Verify(provider => provider.GetAuthenticatedApp(), Times.Once);
    }
}