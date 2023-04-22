namespace ToolKit.Api.Contracts.GitHub;

public class GitHubCommitVerification
{
    public bool Verified { get; set; }
    public string Reason { get; set; }
    public string Signature { get; set; }
    public string Payload { get; set; }
}