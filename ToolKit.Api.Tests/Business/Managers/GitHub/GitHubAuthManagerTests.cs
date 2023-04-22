using Microsoft.Extensions.Configuration;
using ToolKit.Api.Business.Managers.GitHub;
using ToolKit.Api.Interfaces.Providers.GitHub;

namespace ToolKit.Api.UnitTests.Business.Managers.GitHub;

public class GitHubAuthManagerTests
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IGitHubAuthProvider> _gitHubAuthProviderMock;
    private readonly GitHubAuthManager _gitHubAuthManager;

    public GitHubAuthManagerTests()
    {
        _configurationMock = new Mock<IConfiguration>();
        _gitHubAuthProviderMock = new Mock<IGitHubAuthProvider>();
        _gitHubAuthManager = new GitHubAuthManager(_configurationMock.Object, _gitHubAuthProviderMock.Object);
    }

    [Fact]
    public void GetGitHubAuthUrl_StartsWithExpectedUrl()
    {
        SetupGitHubConfiguration();

        string result = _gitHubAuthManager.GetGitHubAuthUrl();

        Assert.StartsWith("https://github.com/login/oauth/authorize?client_id=testClientId", result);
    }

    [Theory, AutoData]
    public void GetTokenRequest_HasExpectedMethod(string code, string state)
    {
        SetupGitHubConfiguration();

        HttpRequestMessage result = _gitHubAuthManager.GetTokenRequest(code, state);

        Assert.Equal(HttpMethod.Post, result.Method);
    }

    [Theory, AutoData]
    public void GetTokenRequest_HasExpectedRequestUri(string code, string state)
    {
        SetupGitHubConfiguration();

        HttpRequestMessage result = _gitHubAuthManager.GetTokenRequest(code, state);

        Assert.Equal("https://github.com/login/oauth/access_token", result.RequestUri.ToString());
    }

    [Theory, AutoData]
    public void GetTokenRequest_HasExpectedHeader(string code, string state)
    {
        SetupGitHubConfiguration();

        HttpRequestMessage result = _gitHubAuthManager.GetTokenRequest(code, state);

        Assert.Contains("application/json", result.Headers.Accept.ToString());
    }

    [Fact]
    public async Task RetrieveAccessToken_CallsAuthProvider()
    {
        HttpRequestMessage tokenRequest = new HttpRequestMessage();

        await _gitHubAuthManager.RetrieveAccessToken(tokenRequest);

        _gitHubAuthProviderMock.Verify(provider => provider.RetrieveAccessToken(tokenRequest), Times.Once);
    }

    [Fact]
    public async Task ParseAccessToken_ReturnsAccessToken()
    {
        string accessToken = "testAccessToken";
        HttpResponseMessage tokenResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(
                $"{{\"access_token\":\"{accessToken}\",\"token_type\":\"bearer\",\"scope\":\"repo,user\"}}")
        };

        string result = await _gitHubAuthManager.ParseAccessToken(tokenResponse);

        Assert.Equal(accessToken, result);
    }

    private void SetupGitHubConfiguration()
    {
        _configurationMock.SetupGet(config => config["GitHub:ClientId"]).Returns("testClientId");
        _configurationMock.SetupGet(config => config["GitHub:ClientSecret"]).Returns("testClientSecret");
        _configurationMock.SetupGet(config => config["GitHub:RedirectUri"]).Returns("https://testRedirectUri.com");
    }
}