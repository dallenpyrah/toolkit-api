namespace ToolKit.Api.Contracts.GitHub;

public class GitHubCommit
{
    public string Url { get; set; }
    public GitHubCommitAuthor Author { get; set; }
    public GitHubCommitAuthor Committer { get; set; }
    public string Message { get; set; }
    public GitHubTree Tree { get; set; }
    public int CommentCount { get; set; }
    public GitHubCommitVerification Verification { get; set; }
}