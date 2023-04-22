using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolKit.Api.Business.Exceptions.GitHub;
using ToolKit.Api.Contracts;
using ToolKit.Api.Contracts.GitHub;
using ToolKit.Api.Interfaces.Managers.GitHub;

namespace ToolKit.Api.Service.Controllers.GitHub;

[ApiController]
[Route("api/github/repos")]
public class GitHubUserReposController : ControllerBase
{
    private readonly IGitHubUserReposManager _gitHubUserReposManager;
    private ILogger<GitHubUserReposController> _logger;

    public GitHubUserReposController(IGitHubUserReposManager gitHubUserReposManager,
        ILogger<GitHubUserReposController> logger)
    {
        _gitHubUserReposManager = gitHubUserReposManager;
        _logger = logger;
    }

    [HttpGet("{username}")]
    [Authorize]
    public async Task<IActionResult> GetReposByUsername(string username)
    {
        try
        {
            ApiResponse<IEnumerable<GitHubRepo>> response = await _gitHubUserReposManager.GetReposByUsername(username);
            return Ok(response);
        }
        catch (GitHubRepositoryException e)
        {
            _logger.LogError(e, e.ResponseMessageReasonPhrase);
            return StatusCode(StatusCodes.Status500InternalServerError, e.ResponseMessageReasonPhrase);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet("{owner}/{repo}")]
    public async Task<IActionResult> GetRepo(string owner, string repo)
    {
        try
        {
            var response = await _gitHubUserReposManager.GetUserRepo(owner, repo);
            return Ok(response);
        }
        catch (GitHubRepositoryException e)
        {
            _logger.LogError(e, e.ResponseMessageReasonPhrase);
            return StatusCode(StatusCodes.Status500InternalServerError, e.ResponseMessageReasonPhrase);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}