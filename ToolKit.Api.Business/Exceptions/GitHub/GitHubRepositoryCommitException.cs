using System.Net;

namespace ToolKit.Api.Business.Exceptions.GitHub;

public class GitHubRepositoryCommitException : Exception
{
    public string? ResponseMessageReasonPhrase { get; set; }

    public HttpStatusCode ResponseMessageStatusCode { get; set; }

    public GitHubRepositoryCommitException(HttpStatusCode responseMessageStatusCode,
        string? responseMessageReasonPhrase)
    {
        ResponseMessageStatusCode = responseMessageStatusCode;
        ResponseMessageReasonPhrase = responseMessageReasonPhrase;
    }
}