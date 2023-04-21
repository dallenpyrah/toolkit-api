namespace ToolKit.Api.UnitTests.Business.Extensions;

public class UserExtensionsTests
{
    private readonly Fixture _fixture;

    public UserExtensionsTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void ToApiResponse_WithValidUser_ReturnsApiResponseWithCorrectBody()
    {
        var user = _fixture.Create<User>();
        var message = "User created successfully.";
        
        ApiResponse<User> apiResponse = user.ToApiResponse(message);
        
        Assert.Equal(user, apiResponse.Body);
    }

    [Fact]
    public void ToApiResponse_WithValidMessage_ReturnsApiResponseWithCorrectMessage()
    {
        var user = _fixture.Create<User>();
        var message = "User created successfully.";
        
        ApiResponse<User> apiResponse = user.ToApiResponse(message);
        
        Assert.Equal(message, apiResponse.Message);
    }
}