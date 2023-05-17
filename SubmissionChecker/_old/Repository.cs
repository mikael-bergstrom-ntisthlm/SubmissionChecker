namespace SubmissionChecker;

using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using LibGit2Sharp;
public class GithubRepository
{
  public enum RepoStatus { ok, warning }
  public enum Flags { red, yellow, green }

  public string Username { get; set; }
  public string Reponame { get; set; }

  public string Title { get; private set; }
  public DateTime LatestCommit { get; private set; }

  [JsonIgnore]
  private static readonly RestClient restClient = new("https://api.github.com");

  [JsonIgnore]
  public RepoStatus Status { get; private set; }
  [JsonIgnore]
  public string StatusMessage { get; private set; }

  [JsonIgnore]
  public Flags CommitStatus { get; private set; }

  public static string GithubApiTokenFilename
  {
    private get => "";
    set
    {
      string token = File.ReadAllText(value);
      restClient.AddDefaultHeader("Authorization", "token " + token);
    }
  }

  static readonly Regex regex = new(@"https?://github.com/(?<Username>[A-Za-z0-9-_]*)/(?<Reponame>[A-Za-z0-9-_]*).*");

  public static bool IsValid(string url)
  {
    return regex.IsMatch(url);
  }

  public GithubRepository()
  {}

  public GithubRepository(string repoUrl)
  {
    Init(repoUrl);
    Title = Reponame;
  }

  public GithubRepository(string repoUrl, string title)
  {
    Init(repoUrl);
    Title = title;
  }

  private void Init(string repoUrl)
  {
    Match result = regex.Match(repoUrl);

    Username = result.Groups["Username"]?.Value;
    Reponame = result.Groups["Reponame"]?.Value;
  }

  public override string ToString()
  {
    return $"{Title} [{LatestCommit}]";
  }

  public string SyncToFolder(string targetDirectory)
  {
    if (LibGit2Sharp.Repository.IsValid(targetDirectory))
    {
      Console.WriteLine($"Updating {Title}");
      LibGit2Sharp.Repository rp = new(targetDirectory);
      MergeResult mergeResult = Commands.Pull(rp,
        // TODO: Fix signature
        new Signature("SubmissionChecker", "mikael.bergstrom@ga.ntig.se", DateTimeOffset.Now),
        new PullOptions()
      );
      return mergeResult.Status.ToString();
    }
    else
    {
      // TODO: 404 check: do not trust the status code
      Console.WriteLine($"Cloning {Title}");
      try
      {
        LibGit2Sharp.Repository.Clone($"https://github.com/{Username}/{Reponame}.git", targetDirectory);
      }
      catch (Exception e)
      {
        Console.WriteLine($"Failed to clone {Title}: {e.Message}");
        return "Failed to clone";
      }
      return "";
    }
  }

  public void UpdateCommitInfo()
  {
    // Get latest commit
    RestRequest request = new($"repos/{Username}/{Reponame}/commits");

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