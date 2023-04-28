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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Installation>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    public async Task<ActionResult<IEnumerable<Installation>>> GetInstallations()
    {
        try
        {
            var response = await _gitHubInstallationsManager.GetInstallations();
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse { Message = ex.Message, Details = "No installations found." });
        }
        catch (AuthorizationException ex)
        {
            return Unauthorized(new ErrorResponse
                { Message = ex.Message, Details = "Invalid credentials or insufficient permissions." });
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
                Details = "An error occurred while fetching the installations."
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
            {
                Message = e.Message,
                Details = "An error occurred."
            });
        }
    }

    [HttpGet("{user}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Installation))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    [Authorize]
    public async Task<ActionResult<Installation>> GetInstallationByUsername(string user)
    {
        try
        {
            var response = await _gitHubInstallationsManager.GetInstallationsByUsername(user);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse
            {
                Message = ex.Message,
                Details =
                    "No installation found for the specified user. Please ensure the user is a member of an organization that has installed the application."
            });
        }
        catch (AuthorizationException ex)
        {
            return Unauthorized(new ErrorResponse
                { Message = ex.Message, Details = "Invalid credentials or insufficient permissions." });
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
                Details = "An error occurred."
            });
        }
    }
}