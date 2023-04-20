using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolKit.Api.Interfaces.Managers;

namespace ToolKit.Api.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersManager _usersManager;

    public UsersController(IUsersManager usersManager)
    {
        _usersManager = usersManager;
    }
    
    [HttpGet]
    [Authorize]
    public IActionResult GetUsers()
    {
        return Ok();
    }
    
    [HttpGet("{id}")]
    [Authorize]
    public IActionResult GetUserById(int id)
    {
        return Ok();
    }
    
    [HttpPost]
    [Authorize]
    public IActionResult CreateUser()
    {
        return Ok();
    }
    
    [HttpPut("{id}")]
    [Authorize]
    public IActionResult UpdateUser(int id)
    {
        return Ok();
    }
    
    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult DeleteUser(int id)
    {
        return Ok();
    }
}