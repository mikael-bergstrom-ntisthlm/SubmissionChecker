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

  public List<Repository> Repos { get; private set; }

  public Submission(Assignment assignment)
  {
    Assignment = assignment;
    Repos = new List<Repository>();
  }

  public void AddRepo(Repository repo)
  {
    Repos.Add(repo);
  }

  public Flags GetStatus(int greenHours = 24)
  {
    Flags statusFlag = Flags.red;
    foreach (Repository repo in Repos)
    {
      TimeSpan age = DateTime.Now - repo.LatestCommit;
      if (age.TotalHours < greenHours) return Flags.green;
      if (repo.Status == Repository.RepoStatus.ok) statusFlag = Flags.yellow;
    }
    
    return statusFlag;
  }

}