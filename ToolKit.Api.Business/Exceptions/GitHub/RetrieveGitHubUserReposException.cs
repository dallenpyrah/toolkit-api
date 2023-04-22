using System.Net;

namespace ToolKit.Api.Business.Exceptions.GitHub;

public class RetrieveGitHubUserReposException : Exception
{
    public string? ResponseMessageReasonPhrase { get; set; }

    public HttpStatusCode ResponseMessageStatusCode { get; set; }
    
    public RetrieveGitHubUserReposException(HttpStatusCode responseMessageStatusCode, string? responseMessageReasonPhrase)
    {
        ResponseMessageStatusCode = responseMessageStatusCode;
        ResponseMessageReasonPhrase = responseMessageReasonPhrase;
    }
}