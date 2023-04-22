using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolKit.Api.Business.Exceptions;
using ToolKit.Api.Contracts;
using ToolKit.Api.Interfaces.Managers;

namespace ToolKit.Api.Service.Controllers.GitHub;

[ApiController]
[Route("api/github")]
public class GitHubAuthController : ControllerBase
{
    private readonly IGitHubAuthManager _gitHubAuthManager;
    private readonly IHttpClientFactory _httpClientFactory;
    private ILogger<GitHubAuthController> _logger;

    public GitHubAuthController(IGitHubAuthManager gitHubAuthManager, IHttpClientFactory httpClientFactory, ILogger<GitHubAuthController> logger)
    {
        _gitHubAuthManager = gitHubAuthManager;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }
    
    [HttpGet("authenticate")]
    [Authorize]
    public IActionResult Authenticate()
    {
        string githubAuthUrl = _gitHubAuthManager.GetGitHubAuthUrl();
        return Redirect(githubAuthUrl);
    }
    
    [HttpGet("callback")]
    public async Task<IActionResult> Callback(string code, string state)
    {
        try
        {
            using HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpRequestMessage tokenRequest = _gitHubAuthManager.GetTokenRequest(code, state);

            HttpResponseMessage tokenResponse = await _gitHubAuthManager.RetrieveAccessToken(tokenRequest);
            tokenResponse.EnsureSuccessStatusCode();

            string accessToken = await _gitHubAuthManager.ParseAccessToken(tokenResponse);

            return Ok(new ApiResponse<GitHubAuthCallbackResponse>()
            {
                Body = new GitHubAuthCallbackResponse()
                {
                    AccessToken = accessToken
                },
                Message = "Successfully retrieved access token from GitHub."
            });
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, "An error occured while trying to retrieve access token from GitHub.");
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        catch (GitHubAccessTokenException e)
        {
            _logger.LogError(e, "An error occured while trying to parse access token from GitHub.");
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected error occurred.");
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}