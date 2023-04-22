using Microsoft.AspNetCore.Mvc;

namespace ToolKit.Api.Service.Controllers.GitHub;

[ApiController]
[Route("api/github/repos")]
public class GitHubReposController : ControllerBase
{
    public GitHubReposController()
    {
        
    }
    
    [HttpGet]
    public IActionResult GetRepos()
    {
        return Ok();
    }
    
    [HttpGet("{id}")]
    public IActionResult GetRepoById(int id)
    {
        return Ok();
    }
    
    [HttpPost]
    public IActionResult CreateRepo()
    {
        return Ok();
    }
    
    [HttpPut("{id}")]
    public IActionResult UpdateRepo(int id)
    {
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteRepo(int id)
    {
        return Ok();
    }
}