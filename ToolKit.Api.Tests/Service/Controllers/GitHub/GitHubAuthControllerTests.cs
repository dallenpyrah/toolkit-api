using Microsoft.Extensions.Configuration;
using ToolKit.Api.Business.Exceptions.GitHub;

namespace ToolKit.Api.UnitTests.Service.Controllers.GitHub;

public class GitHubAuthControllerTests
{
    private readonly Mock<IGitHubAuthManager> _gitHubAuthManagerMock;
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<ILogger<GitHubAuthController>> _loggerMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly GitHubAuthController _gitHubAuthController;

    public GitHubAuthControllerTests()
    {
        _gitHubAuthManagerMock = new Mock<IGitHubAuthManager>();
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _loggerMock = new Mock<ILogger<GitHubAuthController>>();
        _configurationMock = new Mock<IConfiguration>();
        _gitHubAuthController = new GitHubAuthController(_gitHubAuthManagerMock.Object, _httpClientFactoryMock.Object,
            _loggerMock.Object, _configurationMock.Object);
    }

    [Fact]
    public void Install_ReturnsRedirectResult()
    {
        var clientId = "test-client-id";
        _configurationMock.SetupGet(c => c["GitHub:ClientId"]).Returns(clientId);
        var expectedRedirectUrl = $"https://github.com/apps/toolkit-desktop/installations/new?client_id={clientId}";

        var result = _gitHubAuthController.Install();

        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal(expectedRedirectUrl, redirectResult.Url);
    }

    [Theory]
    [AutoData]
    public async Task Callback_SuccessfullyRetrievesAccessToken_ReturnsOkWithAccessToken(string code, string state,
        string accessToken)
    {
        HttpResponseMessage tokenResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(
                $"{{\"access_token\":\"{accessToken}\",\"token_type\":\"bearer\",\"scope\":\"repo,user\"}}")
        };

        _gitHubAuthManagerMock
            .Setup(manager => manager.GetTokenRequest(code, state))
            .Returns(new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token"));
        _gitHubAuthManagerMock
            .Setup(manager =>
                manager.RetrieveAccessToken(It.IsAny<HttpRequestMessage>()))
            .ReturnsAsync(tokenResponse);
        _gitHubAuthManagerMock
            .Setup(manager => manager.ParseAccessToken(tokenResponse))
            .ReturnsAsync(accessToken);

        var result = await _gitHubAuthController.Callback(code, state);

        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponse<GitHubAuthCallbackResponse>>(okObjectResult.Value);
        Assert.Equal(accessToken, apiResponse.Body.AccessToken);
    }

    [Theory]
    [AutoData]
    public async Task Callback_ThrowsHttpRequestException_ReturnsInternalServerError(string code, string state)
    {
        _gitHubAuthManagerMock
            .Setup(manager => manager.GetTokenRequest(code, state))
            .Returns(new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token"));
        _gitHubAuthManagerMock
            .Setup(manager =>
                manager.RetrieveAccessToken(It.IsAny<HttpRequestMessage>()))
            .ThrowsAsync(new HttpRequestException());

        var result = await _gitHubAuthController.Callback(code, state);

        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
    }

    [Theory]
    [AutoData]
    public async Task Callback_ThrowsGitHubAccessTokenException_ReturnsInternalServerError(string code, string state)
    {
        HttpResponseMessage tokenResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(
                $"{{\"access_token\":\"test-access-token\",\"token_type\":\"bearer\",\"scope\":\"repo,user\"}}")
        };

        _gitHubAuthManagerMock
            .Setup(manager => manager.GetTokenRequest(code, state))
            .Returns(new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token"));
        _gitHubAuthManagerMock
            .Setup(manager =>
                manager.RetrieveAccessToken(It.IsAny<HttpRequestMessage>()))
            .ReturnsAsync(tokenResponse);
        _gitHubAuthManagerMock
            .Setup(manager => manager.ParseAccessToken(tokenResponse))
            .ThrowsAsync(new GitHubAccessTokenException("Error parsing access token."));

        var result = await _gitHubAuthController.Callback(code, state);

        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
    }


    [Theory]
    [AutoData]
    public async Task Callback_ThrowsGenericException_ReturnsInternalServerError(string code, string state)
    {
        _gitHubAuthManagerMock
            .Setup(manager => manager.GetTokenRequest(code, state))
            .Returns(new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token"));
        _gitHubAuthManagerMock
            .Setup(manager =>
                manager.RetrieveAccessToken(It.IsAny<HttpRequestMessage>()))
            .ThrowsAsync(new Exception("Unexpected exception."));

        var result = await _gitHubAuthController.Callback(code, state);

        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
    }
}