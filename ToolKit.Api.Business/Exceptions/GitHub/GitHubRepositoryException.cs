using System.Net;

namespace ToolKit.Api.Business.Exceptions.GitHub;

public class GitHubRepositoryException : Exception
{
    public string? ResponseMessageReasonPhrase { get; set; }

    public HttpStatusCode ResponseMessageStatusCode { get; set; }

    public GitHubRepositoryException(HttpStatusCode responseMessageStatusCode, string? responseMessageReasonPhrase)
    {
        ResponseMessageStatusCode = responseMessageStatusCode;
        ResponseMessageReasonPhrase = responseMessageReasonPhrase;
    }
}