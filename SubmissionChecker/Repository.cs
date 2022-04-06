public class Repository
{
  public enum RepoStatus { ok, warning }

  public string Url { get; private set; }
  public string Name { get; private set; }
  public DateTime LatestCommit { get; private set; }

  private static readonly RestClient restClient = new("https://api.github.com");
  private static readonly string token = "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";

  public RepoStatus Status { get; private set; }
  public string StatusMessage { get; private set; }

  static Repository()
  {
    restClient.AddDefaultHeader("Authorization", "token " + token);
  }

  public Repository(string repoUrl)
  {
    Url = repoUrl;
    Name = repoUrl.Split('/')[1];
    UpdateInfo();
  }

  public override string ToString()
  {
    return $"{Name} [{LatestCommit}]";
  }

  public string SyncToFolder(string targetDirectory)
  {
    if (LibGit2Sharp.Repository.IsValid(targetDirectory))
    {
      Console.WriteLine($"Updating {Name}");
      LibGit2Sharp.Repository rp = new(targetDirectory);
      MergeResult mergeResult = Commands.Pull(rp,
        new Signature("SubmissionChecker", "mikael.bergstrom@ga.ntig.se", DateTimeOffset.Now),
        new PullOptions()
      );
      return mergeResult.Status.ToString();
    }
    else
    {
      Console.WriteLine($"Cloning {Name}");
      LibGit2Sharp.Repository.Clone($"https://github.com/{Url}.git", targetDirectory);
      return "";
    }
  }

  public void UpdateInfo()
  {
    // Get latest commit
    RestRequest request = new($"repos/{Url}/commits");

    try
    {
      RestResponse response = restClient.GetAsync(request).GetAwaiter().GetResult();

      if (response.StatusCode == System.Net.HttpStatusCode.OK)
      {
        Status = RepoStatus.ok;
        StatusMessage = "";

        List<Commit> commits = JsonSerializer.Deserialize<List<Commit>>(response.Content);

        foreach (Commit commit in commits)
        {
          if (commit.Date > LatestCommit)
          {
            LatestCommit = commit.Date;
          }
        }
      }
      else
      {
        Status = RepoStatus.warning;
        StatusMessage = "Could not get commits";
      }
    }
    catch (Exception e)
    {
      Console.WriteLine($"Error: {e.Message}");
    }
  }
}