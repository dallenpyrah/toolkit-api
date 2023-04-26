using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using ToolKit.Api.Contracts;
using ToolKit.Api.Interfaces.Managers.GitHub;

namespace ToolKit.Api.Service.Controllers.GitHub;

[ApiController]
[Route("api/github/application")]
public class GitHubApplicationController : ControllerBase
{
    private readonly IGitHubApplicationManager _gitHubApplicationManager;
    private readonly ILogger<GitHubApplicationController> _logger;

    public GitHubApplicationController(IGitHubApplicationManager gitHubApplicationManager,
        ILogger<GitHubApplicationController> logger)
    {
        _gitHubApplicationManager = gitHubApplicationManager;
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<GitHubApp>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<GitHubApp>>> GetAuthenticatedApp()
    {
        try
        {
            var response = await _gitHubApplicationManager.GetAuthenticatedApp();
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}