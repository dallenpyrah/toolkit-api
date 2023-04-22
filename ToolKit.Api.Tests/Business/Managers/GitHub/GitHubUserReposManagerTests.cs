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
    public async Task GetReposByUsername_SuccessfulResponse_ReturnsApiResponse(string username,
        List<GitHubRepo> gitHubRepos)
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

        Assert.ThrowsAsync<GitHubRepositoryException>(() => _gitHubUserReposManager.GetReposByUsername(username));
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

    [Theory]
    [AutoData]
    public async Task GetUserRepo_SuccessfulResponse_ReturnsApiResponse(string owner, string repo)
    {
        var gitHubRepo = new GitHubRepo()
        {
            Name = "Test Repo"
        };

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(gitHubRepo))
        };

        _gitHubUserReposProviderMock
            .Setup(provider => provider.GetUserRepo(owner, repo))
            .ReturnsAsync(httpResponseMessage);

        var result = await _gitHubUserReposManager.GetUserRepo(owner, repo);
        var expected = new ApiResponse<GitHubRepo>()
        {
            Body = gitHubRepo,
            Message = $"Successfully retrieved GitHub repository {repo} for user {owner}."
        };

        Assert.Equal(expected.Message, result.Message);
        Assert.Equal(expected.Body.Name, result.Body.Name);
    }

    [Theory]
    [AutoData]
    public async Task GetUserRepo_UnsuccessfulResponse_ThrowsRetrieveGitHubUserReposException(string owner, string repo)
    {
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent("Error retrieving repository."),
            ReasonPhrase = "Error retrieving repository."
        };

        _gitHubUserReposProviderMock
            .Setup(provider => provider.GetUserRepo(owner, repo))
            .ReturnsAsync(httpResponseMessage);

        await Assert.ThrowsAsync<GitHubRepositoryException>(async () =>
        {
            await _gitHubUserReposManager.GetUserRepo(owner, repo);
        });
    }
}