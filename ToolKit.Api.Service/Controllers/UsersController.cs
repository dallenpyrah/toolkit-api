using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolKit.Api.Business.Exceptions;
using ToolKit.Api.Contracts;
using ToolKit.Api.DataModel.Entities;
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
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        try
        {
            var user = await _usersManager.GetUserById(id);
            return Ok(user);
        }
        catch (UserNotFoundException e)
        {
            return StatusCode(StatusCodes.Status404NotFound, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var user = await _usersManager.CreateUser(request);
            return Ok(user);
        }
        catch (UserValidationException e)
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, e.Message);
        }
        catch (EmailAlreadyRegisteredException e)
        {
            return StatusCode(StatusCodes.Status409Conflict, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
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