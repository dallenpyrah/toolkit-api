using GitHubJwt;
using ToolKit.Api.Interfaces.Managers.GitHub;

namespace ToolKit.Api.Business.Managers.GitHub;

public class GitHubJwtTokenManager : IGitHubJwtTokenManager
{
    private readonly string _privateKey;
    private readonly int _appId;

    public GitHubJwtTokenManager(string privateKey, int appId)
    {
        _privateKey = privateKey;
        _appId = appId;
    }

    public string GenerateJwtToken()
    {
        var generator = new GitHubJwtFactory(
            new StringPrivateKeySource(_privateKey),
            new GitHubJwtFactoryOptions
            {
                AppIntegrationId = _appId,
                ExpirationSeconds = 600 // 10 minutes is the maximum time allowed
            }
        );

        return generator.CreateEncodedJwtToken();
    }
}