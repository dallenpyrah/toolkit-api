namespace ToolKit.Api.Business.Exceptions;

public class UserValidationException : Exception
{
    public UserValidationException(string message) : base(message) { }
}