namespace SubmissionChecker;

public class Submission
{
  /// <summary>
  /// Red = No valid repo
  /// Yellow = Valid, only old commits
  /// Green = Valid, new commits
  /// </summary>
  public enum Flags {red, yellow, green}

  public Assignment Assignment { get; private set; }

  public List<GithubRepository> Repos { get; private set; }

  public Submission(Assignment assignment)
  {
    Assignment = assignment;
    Repos = new List<GithubRepository>();
  }

  public void AddRepo(GithubRepository repo)
  {
    Repos.Add(repo);
  }

  public Flags GetStatus(int greenHours = 24)
  {
    Flags statusFlag = Flags.red;
    foreach (GithubRepository repo in Repos)
    {
      TimeSpan age = DateTime.Now - repo.LatestCommit;
      if (age.TotalHours < greenHours) return Flags.green;
      if (repo.Status == GithubRepository.RepoStatus.ok) statusFlag = Flags.yellow;
    }
    
    return statusFlag;
  }

}