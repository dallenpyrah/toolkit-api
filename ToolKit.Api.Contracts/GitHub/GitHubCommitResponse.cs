namespace ToolKit.Api.Contracts.GitHub;

public class GitHubCommitResponse
{
    public string Url { get; set; }
    public string Sha { get; set; }
    public string NodeId { get; set; }
    public string HtmlUrl { get; set; }
    public string CommentsUrl { get; set; }
    public GitHubCommit Commit { get; set; }
    public GitHubUser Author { get; set; }
    public GitHubUser Committer { get; set; }
    public List<GitHubCommitParent> Parents { get; set; }
}