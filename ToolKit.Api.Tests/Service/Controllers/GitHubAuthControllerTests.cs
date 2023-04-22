using System.Net;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ToolKit.Api.Interfaces.Managers;
using ToolKit.Api.Service.Controllers;

namespace ToolKit.Api.UnitTests.Service.Controllers;

public class GitHubAuthControllerTests
{
    private readonly Mock<IGitHubAuthManager> _gitHubAuthManagerMock;
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<ILogger<GitHubAuthController>> _loggerMock;
    private readonly GitHubAuthController _gitHubAuthController;

    public GitHubAuthControllerTests()
    {
        _gitHubAuthManagerMock = new Mock<IGitHubAuthManager>();
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _loggerMock = new Mock<ILogger<GitHubAuthController>>();
        _gitHubAuthController = new GitHubAuthController(_gitHubAuthManagerMock.Object, _httpClientFactoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public void Authenticate_RedirectsToGitHubAuthUrl()
    {
        var githubAuthUrl = "https://github.com/login/oauth/authorize?...";
        _gitHubAuthManagerMock
            .Setup(manager => manager.GetGitHubAuthUrl())
            .Returns(githubAuthUrl);

        IActionResult result = _gitHubAuthController.Authenticate();

        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal(githubAuthUrl, redirectResult.Url);
    }

    [Theory, AutoData]
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

        IActionResult result = await _gitHubAuthController.Callback(code, state);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var apiResponse = Assert.IsType<ApiResponse<GitHubAuthCallbackResponse>>(okObjectResult.Value);
        Assert.Equal(accessToken, apiResponse.Body.AccessToken);
    }

    [Theory, AutoData]
    public async Task Callback_ThrowsHttpRequestException_ReturnsInternalServerError(string code, string state)
    {
        _gitHubAuthManagerMock
            .Setup(manager => manager.GetTokenRequest(code, state))
            .Returns(new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token"));
        _gitHubAuthManagerMock
            .Setup(manager =>
                manager.RetrieveAccessToken(It.IsAny<HttpRequestMessage>()))
            .ThrowsAsync(new HttpRequestException());

        IActionResult result = await _gitHubAuthController.Callback(code, state);

        Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
    }

    [Fact]
    public void Authenticate_CallsGetGitHubAuthUrl()
    {
        var githubAuthUrl = "https://github.com/login/oauth/authorize?...";
        _gitHubAuthManagerMock
            .Setup(manager => manager.GetGitHubAuthUrl())
            .Returns(githubAuthUrl);

        _gitHubAuthController.Authenticate();

        _gitHubAuthManagerMock.Verify(manager => manager.GetGitHubAuthUrl(), Times.Once);
    }

    [Theory, AutoData]
    public async Task Callback_CallsGetTokenRequest(string code, string state)
    {
        SetupSuccessfulTokenResponse();

        await _gitHubAuthController.Callback(code, state);

        _gitHubAuthManagerMock.Verify(manager => manager.GetTokenRequest(code, state), Times.Once);
    }

    [Theory, AutoData]
    public async Task Callback_CallsRetrieveAccessToken(string code, string state)
    {
        SetupSuccessfulTokenResponse();

        await _gitHubAuthController.Callback(code, state);

        _gitHubAuthManagerMock.Verify(manager => manager.RetrieveAccessToken(It.IsAny<HttpRequestMessage>()),
            Times.Once);
    }

    [Theory, AutoData]
    public async Task Callback_CallsParseAccessToken(string code, string state)
    {
        SetupSuccessfulTokenResponse();

        await _gitHubAuthController.Callback(code, state);

        _gitHubAuthManagerMock.Verify(manager => manager.ParseAccessToken(It.IsAny<HttpResponseMessage>()), Times.Once);
    }

    [Theory, AutoData]
    public async Task Callback_ThrowsGitHubAccessTokenException_ReturnsInternalServerError(string code, string state)
    {
        SetupSuccessfulTokenResponse();
        _gitHubAuthManagerMock
            .Setup(manager => manager.ParseAccessToken(It.IsAny<HttpResponseMessage>()))
            .ThrowsAsync(new GitHubAccessTokenException("Error parsing access token."));

        IActionResult result = await _gitHubAuthController.Callback(code, state);

        Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
    }

    private void SetupSuccessfulTokenResponse()
    {
        var accessToken = "test-access-token";
        HttpResponseMessage tokenResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(
                $"{{\"access_token\":\"{accessToken}\",\"token_type\":\"bearer\",\"scope\":\"repo,user\"}}")
        };

        _gitHubAuthManagerMock
            .Setup(manager => manager.GetTokenRequest(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token"));
        _gitHubAuthManagerMock
            .Setup(manager => manager.RetrieveAccessToken(It.IsAny<HttpRequestMessage>()))
            .ReturnsAsync(tokenResponse);
        _gitHubAuthManagerMock
            .Setup(manager => manager.ParseAccessToken(tokenResponse))
            .ReturnsAsync(accessToken);
    }
}