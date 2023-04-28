using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using ToolKit.Api.Contracts;
using ToolKit.Api.Interfaces.Managers.GitHub;

namespace ToolKit.Api.Service.Controllers.GitHub;

[ApiController]
[Route("api/github/user/repos")]
public class GitHubUserReposController : ControllerBase
{
    private readonly IGitHubUserReposManager _gitHubUserReposManager;
    private readonly ILogger<GitHubUserReposController> _logger;

    public GitHubUserReposController(IGitHubUserReposManager gitHubUserRepos, ILogger<GitHubUserReposController> logger)
    {
        _gitHubUserReposManager = gitHubUserRepos;
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<Repository>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> GetAllUserRepos([FromHeader] string userToken)
    {
        try
        {
            var repositories = await _gitHubUserReposManager.GetAllUserRepos(userToken);
            return Ok(repositories);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse
            {
                Message = ex.Message,
                Details = "No repositories found for the specified user."
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
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
            {
                Message = "An error occurred while fetching the repositories.",
                Details = e.Message
            });
        }
    }

    [HttpGet("{repositoryId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Repository))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> GetUserReposById([FromHeader] string userToken, int repositoryId)
    {
        try
        {
            var repository = await _gitHubUserReposManager.GetUserReposById(userToken, repositoryId);
            return Ok(repository);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse
            {
                Message = ex.Message,
                Details = $"No repository found for the specified user with ID {repositoryId}."
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
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
            {
                Message = "An error occurred while fetching the repository.",
                Details = e.Message
            });
        }
    }
}