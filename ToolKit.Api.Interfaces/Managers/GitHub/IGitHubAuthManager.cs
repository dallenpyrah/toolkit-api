namespace ToolKit.Api.Interfaces.Managers.GitHub;

public interface IGitHubAuthManager
{
    string GetGitHubAuthUrl();
    HttpRequestMessage GetTokenRequest(string code, string state);
    Task<HttpResponseMessage> RetrieveAccessToken(HttpRequestMessage tokenRequest);
    Task<string> ParseAccessToken(HttpResponseMessage tokenResponse);
}