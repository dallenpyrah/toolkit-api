using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToolKit.Api.Interfaces.Managers;
using ToolKit.Api.Service.Controllers;

namespace ToolKit.Api.UnitTests.Service.Controllers;

public class UsersControllerTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IUsersManager> _usersManagerMock;
    private readonly UsersController _usersController;

    public UsersControllerTests()
    {
        _fixture = new Fixture();
        _usersManagerMock = new Mock<IUsersManager>();
        _usersController = new UsersController(_usersManagerMock.Object);
    }

    [Fact]
    public void CreateUser_WhenEmailIsAlreadyRegistered_ResultIsObjectResult()
    {
        var createUserRequest = _fixture.Create<CreateUserRequest>();

        _usersManagerMock
            .Setup(manager => manager.CreateUser(createUserRequest))
            .Throws(new EmailAlreadyRegisteredException("Email already registered."));

        IActionResult actionResult = _usersController.CreateUser(createUserRequest);

        Assert.IsType<ObjectResult>(actionResult);
    }

    [Fact]
    public void CreateUser_WhenEmailIsAlreadyRegistered_StatusCodeIsConflict()
    {
        var createUserRequest = _fixture.Create<CreateUserRequest>();

        _usersManagerMock
            .Setup(manager => manager.CreateUser(createUserRequest))
            .Throws(new EmailAlreadyRegisteredException("Email already registered."));

        IActionResult actionResult = _usersController.CreateUser(createUserRequest);

        var statusCodeResult = actionResult as ObjectResult;
        Assert.Equal(StatusCodes.Status409Conflict, statusCodeResult?.StatusCode);
    }

    [Fact]
    public void CreateUser_WhenEmailIsNotRegistered_ResultIsOkObjectResult()
    {
        var createUserRequest = _fixture.Create<CreateUserRequest>();

        _usersManagerMock
            .Setup(manager => manager.CreateUser(createUserRequest))
            .Returns(new ApiResponse<User>());

        IActionResult actionResult = _usersController.CreateUser(createUserRequest);

        Assert.IsType<OkObjectResult>(actionResult);
    }

    [Fact]
    public void CreateUser_WhenEmailIsNotRegistered_StatusCodeIsOk()
    {
        var createUserRequest = _fixture.Create<CreateUserRequest>();

        _usersManagerMock
            .Setup(manager => manager.CreateUser(createUserRequest))
            .Returns(new ApiResponse<User>());

        IActionResult actionResult = _usersController.CreateUser(createUserRequest);

        var statusCodeResult = actionResult as OkObjectResult;
        Assert.Equal(StatusCodes.Status200OK, statusCodeResult?.StatusCode);
    }
    
    [Fact]
    public void CreateUser_WhenRequestIsInvalid_StatusCodeIsUnprocessableEntity()
    {
        var createUserRequest = _fixture.Create<CreateUserRequest>();

        _usersManagerMock
            .Setup(manager => manager.CreateUser(createUserRequest))
            .Throws(new UserValidationException("Invalid request"));

        IActionResult actionResult = _usersController.CreateUser(createUserRequest);

        var statusCodeResult = actionResult as ObjectResult;
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, statusCodeResult?.StatusCode);
    }
    
    [Fact]
    public void CreateUser_WhenRequestIsInvalid_ResultIsObjectResult()
    {
        var createUserRequest = _fixture.Create<CreateUserRequest>();

        _usersManagerMock
            .Setup(manager => manager.CreateUser(createUserRequest))
            .Throws(new UserValidationException("Invalid request"));

        IActionResult actionResult = _usersController.CreateUser(createUserRequest);

        Assert.IsType<ObjectResult>(actionResult);
    }
    
    [Fact]
    public void CreateUser_WhenRequestIsValid_StatusCodeIsOk()
    {
        var createUserRequest = _fixture.Create<CreateUserRequest>();

        _usersManagerMock
            .Setup(manager => manager.CreateUser(createUserRequest))
            .Returns(new ApiResponse<User>());

        IActionResult actionResult = _usersController.CreateUser(createUserRequest);

        var statusCodeResult = actionResult as OkObjectResult;
        Assert.Equal(StatusCodes.Status200OK, statusCodeResult?.StatusCode);
    }
    
    [Fact]
    public void CreateUser_WhenRequestIsValid_ResultIsOkObjectResult()
    {
        var createUserRequest = _fixture.Create<CreateUserRequest>();

        _usersManagerMock
            .Setup(manager => manager.CreateUser(createUserRequest))
            .Returns(new ApiResponse<User>());

        IActionResult actionResult = _usersController.CreateUser(createUserRequest);

        Assert.IsType<OkObjectResult>(actionResult);
    }
}