namespace ToolKit.Api.Business.Exceptions;

public class GitHubAccessTokenException : Exception
{
    public GitHubAccessTokenException(string message) : base(message)
    {
    }
}