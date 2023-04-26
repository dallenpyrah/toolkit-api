using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using ToolKit.Api.Contracts;
using ToolKit.Api.Interfaces.Managers.GitHub;

namespace ToolKit.Api.Service.Controllers.GitHub;

[ApiController]
[Route("api/github/installations")]
public class GitHubInstallationsController : ControllerBase
{
    private readonly IGitHubInstallationsManager _gitHubInstallationsManager;
    private readonly ILogger<GitHubInstallationsController> _logger;

    public GitHubInstallationsController(IGitHubInstallationsManager gitHubInstallationsManager,
        ILogger<GitHubInstallationsController> logger)
    {
        _gitHubInstallationsManager = gitHubInstallationsManager;
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<IEnumerable<Installation>>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<IEnumerable<Installation>>>> GetInstallations()
    {
        try
        {
            var response = await _gitHubInstallationsManager.GetInstallations();
            return Ok(response);
        }
        catch (NotFoundException)
        {
            return NotFound(new ApiResponse<string>() { Message = "No installations found." });
        }
        catch (AuthorizationException)
        {
            return Unauthorized(
                new ApiResponse<string> { Message = "Invalid credentials or insufficient permissions." });
        }
        catch (RateLimitExceededException ex)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests,
                new ApiResponse<string> { Message = $"Rate limit exceeded. Resets at {ex.Reset.UtcDateTime}." });
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponse<string>
                    { Message = $"An error occurred while retrieving installations. {ex.Message}" });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponse<string> { Message = e.Message });
        }
    }

    [HttpGet("{user}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<Installation>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult<ApiResponse<Installation>>> GetInstallationByUsername(string user)
    {
        try
        {
            var response = await _gitHubInstallationsManager.GetInstallationsByUsername(user);
            return Ok(response);
        }
        catch (NotFoundException)
        {
            return NotFound(new ApiResponse<string> { Message = $"No installation found for user '{user}'." });
        }
        catch (AuthorizationException)
        {
            return Unauthorized(
                new ApiResponse<string> { Message = "Invalid credentials or insufficient permissions." });
        }
        catch (RateLimitExceededException ex)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests,
                new ApiResponse<string> { Message = $"Rate limit exceeded. Resets at {ex.Reset.UtcDateTime}." });
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponse<string>
                    { Message = $"An error occurred while fetching the installation for user '{user}'." });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponse<string> { Message = e.Message });
        }
    }
}