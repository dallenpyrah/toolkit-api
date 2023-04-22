namespace ToolKit.Api.Business.Exceptions.GitHub;

public class GitHubAccessTokenException : Exception
{
    public GitHubAccessTokenException(string message) : base(message)
    {
    }
}