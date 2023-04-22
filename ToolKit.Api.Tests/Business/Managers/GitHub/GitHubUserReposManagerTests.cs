using ToolKit.Api.Business.Exceptions.GitHub;
using ToolKit.Api.Business.Managers.GitHub;
using ToolKit.Api.Interfaces.Providers.GitHub;

namespace ToolKit.Api.UnitTests.Business.Managers.GitHub;

public class GitHubUserReposManagerTests
{
    private readonly Mock<IGitHubUserReposProvider> _gitHubUserReposProviderMock;
    private readonly GitHubUserReposManager _gitHubUserReposManager;

    public GitHubUserReposManagerTests()
    {
        _gitHubUserReposProviderMock = new Mock<IGitHubUserReposProvider>();
        _gitHubUserReposManager = new GitHubUserReposManager(_gitHubUserReposProviderMock.Object);
    }

    [Theory, AutoData]
    public async Task GetReposByUsername_SuccessfulResponse_ReturnsApiResponse(string username, List<GitHubRepo> gitHubRepos)
    {
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(gitHubRepos))
        };

        _gitHubUserReposProviderMock
            .Setup(provider => provider.GetReposByUsername(username))
            .ReturnsAsync(httpResponseMessage);

        var result = await _gitHubUserReposManager.GetReposByUsername(username);
        var expected = new ApiResponse<IEnumerable<GitHubRepo>>()
        {
            Body = gitHubRepos,
            Message = "Successfully retrieved GitHub repositories.",
        };

        string expectedJson = JsonConvert.SerializeObject(expected.Body);
        string resultJson = JsonConvert.SerializeObject(result.Body);

        Assert.Equal(expectedJson, resultJson);
    }


    [Theory, AutoData]
    public void GetReposByUsername_UnsuccessfulResponse_ThrowsRetrieveGitHubUserReposException(string username)
    {
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

        _gitHubUserReposProviderMock
            .Setup(provider => provider.GetReposByUsername(username))
            .ReturnsAsync(httpResponseMessage);

        Assert.ThrowsAsync<RetrieveGitHubUserReposException>(() => _gitHubUserReposManager.GetReposByUsername(username));
    }

    [Theory, AutoData]
    public async Task GetReposByUsername_CallsGetReposByUsernameOnProvider(string username)
    {
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(new List<GitHubRepo>()))
        };

        _gitHubUserReposProviderMock
            .Setup(provider => provider.GetReposByUsername(username))
            .ReturnsAsync(httpResponseMessage);

        await _gitHubUserReposManager.GetReposByUsername(username);

        _gitHubUserReposProviderMock.Verify(provider => provider.GetReposByUsername(username), Times.Once);
    }
}