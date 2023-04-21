namespace ToolKit.Api.Business.Exceptions;

public class EmailAlreadyRegisteredException : Exception
{
    public EmailAlreadyRegisteredException(string message) : base(message)
    {
    }
}