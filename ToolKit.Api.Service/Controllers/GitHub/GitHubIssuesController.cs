using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using ToolKit.Api.Contracts;
using ToolKit.Api.Interfaces.Managers.GitHub;

namespace ToolKit.Api.Service.Controllers.GitHub;

[ApiController]
[Route("api/github/issues")]
public class GitHubIssuesController : ControllerBase
{
    private readonly IGitHubIssuesManager _gitHubIssuesManager;
    private readonly ILogger<GitHubIssuesController> _logger;

    public GitHubIssuesController(IGitHubIssuesManager gitHubIssuesManager, ILogger<GitHubIssuesController> logger)
    {
        _gitHubIssuesManager = gitHubIssuesManager;
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<Issue>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(ErrorResponse))]
    public async Task<ActionResult<IReadOnlyList<Issue>>> GetUserIssues([FromHeader] string userToken)
    {
        try
        {
            var userIssues = await _gitHubIssuesManager.GetUserIssues(userToken);
            return Ok(userIssues);
        }
        catch (AuthorizationException e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status401Unauthorized, new ErrorResponse
            {
                Message = e.Message,
                Details = "Invalid credentials or insufficient permissions."
            });
        }
        catch (RateLimitExceededException ex)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, new ErrorResponse
            {
                Message = ex.Message,
                Details = $"Rate limit exceeded. Resets at {ex.Reset.UtcDateTime}."
            });
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
            {
                Message = ex.Message,
                Details = "An error occurred while fetching the installation."
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
            {
                Message = e.Message,
                Details = "An error occurred while fetching the issues."
            });
        }
    }
}