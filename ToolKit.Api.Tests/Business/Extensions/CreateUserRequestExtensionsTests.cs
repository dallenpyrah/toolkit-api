namespace ToolKit.Api.UnitTests.Business.Extensions;

public class CreateUserRequestExtensionsTests
{
    [Theory]
    [InlineData("test@example.com")]
    [InlineData("john.doe@example.co.uk")]
    [InlineData("jane_doe@example.io")]
    public void Validate_WithValidEmail_DoesNotThrowException(string email)
    {
        var request = new CreateUserRequest
        {
            Email = email,
            FirstName = "John",
            LastName = "Doe",
            Password = "P@ssw0rd!"
        };

        request.Validate();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("test@")]
    [InlineData("test@example@")]
    [InlineData("@example.com")]
    public void Validate_WithInvalidEmail_ThrowsUserValidationException(string email)
    {
        var request = new CreateUserRequest
        {
            Email = email,
            FirstName = "John",
            LastName = "Doe",
            Password = "P@ssw0rd!"
        };

        Assert.Throws<UserValidationException>(() => request.Validate());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithEmptyOrWhitespaceNames_ThrowsUserValidationException(string value)
    {
        var request = new CreateUserRequest
        {
            Email = "test@example.com",
            FirstName = value,
            LastName = "Doe",
            Password = "P@ssw0rd!"
        };

        Assert.Throws<UserValidationException>(() => request.Validate());

        request.FirstName = "John";
        request.LastName = value;

        Assert.Throws<UserValidationException>(() => request.Validate());
    }

    [Theory]
    [InlineData("P@ssw0rd!")]
    [InlineData("Password123!")]
    [InlineData("P@ssw0rd!123")]
    public void Validate_WithValidPassword_DoesNotThrowException(string password)
    {
        var request = new CreateUserRequest
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Password = password
        };

        request.Validate();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("password")]
    [InlineData("PASSWORD")]
    [InlineData("Password")]
    [InlineData("password123")]
    [InlineData("P@ssword")]
    public void Validate_WithInvalidPassword_ThrowsUserValidationException(string password)
    {
        var request = new CreateUserRequest
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Password = password
        };

        Assert.Throws<UserValidationException>(() => request.Validate());
    }
}