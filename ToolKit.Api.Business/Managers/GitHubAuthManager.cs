using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using ToolKit.Api.Business.Exceptions;
using ToolKit.Api.Interfaces.Managers;
using ToolKit.Api.Interfaces.Providers;

namespace ToolKit.Api.Business.Managers;

public class GitHubAuthManager : IGitHubAuthManager
{
    private readonly IConfiguration _configuration;
    private readonly IGitHubAuthProvider _gitHubAuthProvider;

    public GitHubAuthManager(IConfiguration configuration, IGitHubAuthProvider gitHubAuthProvider)
    {
        _configuration = configuration;
        _gitHubAuthProvider = gitHubAuthProvider;
    }

    public string GetGitHubAuthUrl()
    {
        string? clientId = _configuration["GitHub:ClientId"];
        string? redirectUri = _configuration["GitHub:RedirectUri"];
        string scope = "repo user"; 
        string state = Guid.NewGuid().ToString(); 
        
        return
            $"https://github.com/login/oauth/authorize?client_id={clientId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&scope={Uri.EscapeDataString(scope)}&state={state}";
    }

    public HttpRequestMessage GetTokenRequest(string code, string state)
    {
        string? clientId = _configuration["GitHub:ClientId"];
        string? clientSecret = _configuration["GitHub:ClientSecret"];
        string? redirectUri = _configuration["GitHub:RedirectUri"];

        HttpRequestMessage tokenRequest =
            new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string?>
                {
                    { "client_id", clientId },
                    { "client_secret", clientSecret },
                    { "code", code },
                    { "redirect_uri", redirectUri },
                    { "state", state }
                })
            };

        tokenRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return tokenRequest;
    }

    public async Task<HttpResponseMessage> RetrieveAccessToken(HttpRequestMessage tokenRequest)
    {
        return await _gitHubAuthProvider.RetrieveAccessToken(tokenRequest);
    }

    public async Task<string> ParseAccessToken(HttpResponseMessage tokenResponse)
    {
        using JsonDocument jsonDocument = JsonDocument.Parse(await tokenResponse.Content.ReadAsStringAsync());
        string? accessToken = jsonDocument.RootElement.GetProperty("access_token").GetString();

        if (accessToken == null)
        {
            throw new GitHubAccessTokenException("Access token from callback is null");   
        }
        
        return accessToken;
    }
}