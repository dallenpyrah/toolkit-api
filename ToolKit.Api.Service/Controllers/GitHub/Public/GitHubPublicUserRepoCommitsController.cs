using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolKit.Api.Business.Exceptions.GitHub;
using ToolKit.Api.Interfaces.Managers.GitHub;

namespace ToolKit.Api.Service.Controllers.GitHub;

[ApiController]
[Route("api/github/repos/{owner}/{repo}/commits")]
public class GitHubPublicUserRepoCommitsController : ControllerBase
{
    private readonly IGitHubPublicUserRepoCommitsManager _gitHubPublicUserRepoCommitsManager;
    private readonly ILogger<GitHubPublicUserRepoCommitsController> _logger;

    public GitHubPublicUserRepoCommitsController(IGitHubPublicUserRepoCommitsManager gitHubPublicUserRepoCommitsManager,
        ILogger<GitHubPublicUserRepoCommitsController> logger)
    {
        _gitHubPublicUserRepoCommitsManager = gitHubPublicUserRepoCommitsManager;
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetRepoCommits(string owner, string repo)
    {
        try
        {
            var response =
                await _gitHubPublicUserRepoCommitsManager.GetRepoCommits(owner, repo);
            return Ok(response);
        }
        catch (GitHubRepositoryCommitException e)
        {
            _logger.LogError(e, e.ResponseMessageReasonPhrase);
            _logger.LogError(e, e.ResponseMessageStatusCode.ToString());
            return StatusCode((int)e.ResponseMessageStatusCode, e.ResponseMessageReasonPhrase);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet("{commitId}")]
    [Authorize]
    public async Task<IActionResult> GetRepoCommit(string owner, string repo, string commitId)
    {
        try
        {
            var response =
                await _gitHubPublicUserRepoCommitsManager.GetRepoCommit(owner, repo, commitId);
            return Ok(response);
        }
        catch (GitHubRepositoryCommitException e)
        {
            _logger.LogError(e, e.ResponseMessageReasonPhrase);
            _logger.LogError(e, e.ResponseMessageStatusCode.ToString());
            return StatusCode((int)e.ResponseMessageStatusCode, e.ResponseMessageReasonPhrase);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}