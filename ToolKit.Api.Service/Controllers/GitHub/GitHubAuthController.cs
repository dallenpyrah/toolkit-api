using Microsoft.AspNetCore.Mvc;
using ToolKit.Api.Business.Exceptions.GitHub;
using ToolKit.Api.Contracts;
using ToolKit.Api.Contracts.GitHub;
using ToolKit.Api.Interfaces.Managers.GitHub;

namespace ToolKit.Api.Service.Controllers.GitHub;

[ApiController]
[Route("api/github")]
public class GitHubAuthController : ControllerBase
{
    private readonly IGitHubAuthManager _gitHubAuthManager;
    private readonly IHttpClientFactory _httpClientFactory;
    private ILogger<GitHubAuthController> _logger;
    private readonly IConfiguration _configuration;

    public GitHubAuthController(IGitHubAuthManager gitHubAuthManager, IHttpClientFactory httpClientFactory,
        ILogger<GitHubAuthController> logger, IConfiguration configuration)
    {
        _gitHubAuthManager = gitHubAuthManager;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _configuration = configuration;
    }


    [HttpGet("authenticate")]
    public IActionResult Authenticate()
    {
        var githubAuthUrl = _gitHubAuthManager.GetGitHubAuthUrl();
        return Redirect(githubAuthUrl);
    }

    [HttpGet("install")]
    public IActionResult Install()
    {
        string? clientId = _configuration["GitHub:ClientId"];
        string installationUrl = $"https://github.com/apps/toolkit-desktop/installations/new?client_id={clientId}";
        return Redirect(installationUrl);
    }

    [HttpGet("callback")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GitHubAuthCallbackResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public async Task<ActionResult<GitHubAuthCallbackResponse>> Callback(string code, string state)
    {
        try
        {
            using HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpRequestMessage tokenRequest = _gitHubAuthManager.GetTokenRequest(code, state);

            HttpResponseMessage tokenResponse = await _gitHubAuthManager.RetrieveAccessToken(tokenRequest);
            tokenResponse.EnsureSuccessStatusCode();

            string accessToken = await _gitHubAuthManager.ParseAccessToken(tokenResponse);

            return Ok(new GitHubAuthCallbackResponse
            {
                AccessToken = accessToken
            });
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, "An error occured while trying to retrieve access token from GitHub.");
            return BadRequest(new ErrorResponse
            {
                Message = "An error occured while trying to retrieve access token from GitHub.",
                Details = e.Message
            });
        }
        catch (GitHubAccessTokenException e)
        {
            _logger.LogError(e, "An error occured while trying to parse access token from GitHub.");
            return Unauthorized(new ErrorResponse
            {
                Message = "An error occured while trying to parse access token from GitHub.",
                Details = e.Message
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected error occurred.");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
            {
                Message = "An unexpected error occurred.",
                Details = e.Message
            });
        }
    }
}