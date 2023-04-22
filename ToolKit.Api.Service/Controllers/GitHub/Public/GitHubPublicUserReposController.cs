using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolKit.Api.Business.Exceptions.GitHub;
using ToolKit.Api.Interfaces.Managers.GitHub;

namespace ToolKit.Api.Service.Controllers.GitHub;

[ApiController]
[Route("api/github/repos")]
public class GitHubPublicUserReposController : ControllerBase
{
    private readonly IGitHubPublicUserReposManager _gitHubPublicUserReposManager;
    private ILogger<GitHubPublicUserReposController> _logger;

    public GitHubPublicUserReposController(IGitHubPublicUserReposManager gitHubPublicUserReposManager,
        ILogger<GitHubPublicUserReposController> logger)
    {
        _gitHubPublicUserReposManager = gitHubPublicUserReposManager;
        _logger = logger;
    }

    [HttpGet("{username}")]
    [Authorize]
    public async Task<IActionResult> GetReposByUsername(string username)
    {
        try
        {
            var response = await _gitHubPublicUserReposManager.GetReposByUsername(username);
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
            var response = await _gitHubPublicUserReposManager.GetUserRepo(owner, repo);
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