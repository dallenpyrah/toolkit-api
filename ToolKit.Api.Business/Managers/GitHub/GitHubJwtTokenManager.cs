namespace ToolKit.Api.Business.Managers.GitHub;

public class GitHubJwtTokenManager
{

    private readonly string _privateKeyPath;
    private readonly int _appId;

    public GitHubJwtTokenManager(string privateKeyPath, int appId)
    {
        _privateKeyPath = privateKeyPath;
        _appId = appId;
    }
}