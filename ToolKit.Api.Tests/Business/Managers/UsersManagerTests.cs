using ToolKit.Api.Business.Managers;
using ToolKit.Api.Interfaces.Repositories;

namespace ToolKit.Api.UnitTests.Business.Managers;

public class UsersManagerTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IUsersRepository> _usersRepositoryMock;
    private readonly UsersManager _usersManager;

    public UsersManagerTests()
    {
        _fixture = new Fixture();
        _fixture.Customize<CreateUserRequest>(c => c.With(x => x.Email, _fixture.Create<string>() + "@example.com"));
        _usersRepositoryMock = new Mock<IUsersRepository>();
        _usersManager = new UsersManager(_usersRepositoryMock.Object);
    }

    [Fact]
    public void CreateUser_WhenEmailIsAlreadyRegistered_ThrowsEmailAlreadyRegisteredException()
    {
        var createUserRequest = _fixture.Create<CreateUserRequest>();
        _usersRepositoryMock
            .Setup(repo => repo.IsEmailAlreadyRegistered(createUserRequest.Email))
            .Returns(true);

        Assert.Throws<EmailAlreadyRegisteredException>(() => _usersManager.CreateUser(createUserRequest));
    }

    [Theory, AutoData]
    public void CreateUser_WhenEmailIsNotRegistered_ApiResponseIsNotNull(CreateUserRequest request)
    {
        request.Email = _fixture.Create<string>() + "@example.com";
        
        _usersRepositoryMock
            .Setup(repo => repo.IsEmailAlreadyRegistered(request.Email))
            .Returns(false);
        _usersRepositoryMock
            .Setup(repo => repo.CreateUser(It.IsAny<User>()))
            .Returns((User user) => user);

        var apiResponse = _usersManager.CreateUser(request);

        Assert.NotNull(apiResponse);
    }

    [Theory, AutoData]
    public void CreateUser_WhenEmailIsNotRegistered_ApiResponseBodyIsNotNull(CreateUserRequest request)
    {
        request.Email = _fixture.Create<string>() + "@example.com";
        
        _usersRepositoryMock
            .Setup(repo => repo.IsEmailAlreadyRegistered(request.Email))
            .Returns(false);
        _usersRepositoryMock
            .Setup(repo => repo.CreateUser(It.IsAny<User>()))
            .Returns((User user) => user);

        var apiResponse = _usersManager.CreateUser(request);

        Assert.NotNull(apiResponse.Body);
    }

    [Theory, AutoData]
    public void CreateUser_WhenEmailIsNotRegistered_ApiResponseBodyEmailIsSameAsRequest(CreateUserRequest request)
    {
        request.Email = _fixture.Create<string>() + "@example.com";
        
        _usersRepositoryMock
            .Setup(repo => repo.IsEmailAlreadyRegistered(request.Email))
            .Returns(false);
        _usersRepositoryMock
            .Setup(repo => repo.CreateUser(It.IsAny<User>()))
            .Returns((User user) => user);

        var apiResponse = _usersManager.CreateUser(request);

        Assert.Equal(request.Email, apiResponse.Body.Email);
    }

    [Theory, AutoData]
    public void CreateUser_WhenEmailIsNotRegistered_ApiResponseMessageIsUserCreatedSuccessfully(
        CreateUserRequest request)
    {
        request.Email = _fixture.Create<string>() + "@example.com";
        
        _usersRepositoryMock
            .Setup(repo => repo.IsEmailAlreadyRegistered(request.Email))
            .Returns(false);
        _usersRepositoryMock
            .Setup(repo => repo.CreateUser(It.IsAny<User>()))
            .Returns((User user) => user);

        var apiResponse = _usersManager.CreateUser(request);

        Assert.Equal("User created successfully.", apiResponse.Message);
    }
    
    
    [Theory, AutoData]
    public void GetUserById_WhenUserNotFound_ThrowsUserNotFoundException(int id)
    {
        _usersRepositoryMock
            .Setup(repo => repo.GetUserById(id))
            .Returns((User?)null);

        Assert.Throws<UserNotFoundException>(() => _usersManager.GetUserById(id));
    }

    [Theory, AutoData]
    public void GetUserById_WhenUserFound_ApiResponseIsNotNull(int id, User user)
    {
        _usersRepositoryMock
            .Setup(repo => repo.GetUserById(id))
            .Returns(user);

        var apiResponse = _usersManager.GetUserById(id);

        Assert.NotNull(apiResponse);
    }

    [Theory, AutoData]
    public void GetUserById_WhenUserFound_ApiResponseBodyIsNotNull(int id, User user)
    {
        _usersRepositoryMock
            .Setup(repo => repo.GetUserById(id))
            .Returns(user);

        var apiResponse = _usersManager.GetUserById(id);

        Assert.NotNull(apiResponse.Body);
    }

    [Theory, AutoData]
    public void GetUserById_WhenUserFound_ApiResponseBodyIsSameAsRetrievedUser(int id, User user)
    {
        _usersRepositoryMock
            .Setup(repo => repo.GetUserById(id))
            .Returns(user);

        var apiResponse = _usersManager.GetUserById(id);

        Assert.Same(user, apiResponse.Body);
    }

    [Theory, AutoData]
    public void GetUserById_WhenUserFound_ApiResponseMessageIsUserFoundSuccessfully(int id, User user)
    {
        _usersRepositoryMock
            .Setup(repo => repo.GetUserById(id))
            .Returns(user);

        var apiResponse = _usersManager.GetUserById(id);

        Assert.Equal("User found successfully.", apiResponse.Message);
    }
}