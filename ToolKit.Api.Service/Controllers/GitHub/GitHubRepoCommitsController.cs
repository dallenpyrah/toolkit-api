using Microsoft.AspNetCore.Mvc;

namespace ToolKit.Api.Service.Controllers.GitHub;

[ApiController]
[Route("api/github/repos/{repoId}/commits")]
public class GitHubRepoCommitsController : ControllerBase
{
    public GitHubRepoCommitsController()
    {
        
    }
    
    [HttpGet]
    public IActionResult GetCommits(int repoId)
    {
        return Ok();
    }
    
    [HttpGet("{id}")]
    public IActionResult GetCommitById(int repoId, int id)
    {
        return Ok();
    }
    
    [HttpPost]
    public IActionResult CreateCommit(int repoId)
    {
        return Ok();
    }
    
    [HttpPut("{id}")]
    public IActionResult UpdateCommit(int repoId, int id)
    {
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteCommit(int repoId, int id)
    {
        return Ok();
    }
}