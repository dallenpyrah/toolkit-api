using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> GetAuthenticatedApp()
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