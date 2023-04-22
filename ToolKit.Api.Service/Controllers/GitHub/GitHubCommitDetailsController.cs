using Microsoft.AspNetCore.Mvc;

namespace ToolKit.Api.Service.Controllers.GitHub;

[ApiController]
[Route("api/github/repos/{repoId}/commits/{commitId}/details")]
public class GitHubCommitDetailsController : ControllerBase
{
    public GitHubCommitDetailsController()
    {
        
    }
    
    [HttpGet]
    public IActionResult GetCommitDetails(string repoId, string commitId)
    {
        return Ok();
    }
}