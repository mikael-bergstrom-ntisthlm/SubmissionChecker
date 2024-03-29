using System.Text.Json.Serialization;

namespace SubmissionChecker;

public class Student
{
  public string Name { get; set; }
  public string Surname { get; set; }
  public string Email { get; set; }
  public string GoogleId { get; set; }

  [JsonIgnore]
  public string FullName => $"{Name} {Surname}";

  public List<GithubRepository> Repositories { get; set; } = new();

  public override string ToString()
  {
    return $"{Surname} {Name} ({Email})";
  }

  public void Print(bool includeRepos = false)
  {

    if (includeRepos)
    {
      Console.WriteLine($"{this} ");
      foreach (GithubRepository repo in Repositories)
      {
        Console.Write("  ");
        repo.WriteStatus();
        Console.Write($" {repo}");
        Console.WriteLine();
      }
    }
    else
    {
      GithubRepository mostRecent = Repositories.OrderByDescending(x => x.LatestCommit).FirstOrDefault();


      if (mostRecent != null)
      {
        // Console.Write("  ");
        mostRecent.WriteStatus();
        // Console.Write($" {mostRecent}");
        // Console.WriteLine();
      }
      else
      {
        Console.Write(" ");
      }
      Console.WriteLine($" {this}");
    }


    // Console.WriteLine();
  }

  public void UpdateRepoStatus()
  {
    foreach (GithubRepository repo in Repositories)
    {
      repo.UpdateCommitInfo();
    }
  }



  // public void SyncReposToFolder(Assignment assignment, string targetFolder)
  // {
  //   foreach (Repository repo in Submissions[assignment].Repos)
  //   {
  //     if (repo.Status == Repository.RepoStatus.ok)
  //     {
  //       string repoFolder = targetFolder;
  //       if (Submissions[assignment].Repos.Count > 1)
  //       {
  //         repoFolder += Path.DirectorySeparatorChar + repo.Name;
  //       }

  //       if (!Directory.Exists(repoFolder))
  //       {
  //         Directory.CreateDirectory(repoFolder);
  //       }
  //       Console.WriteLine("Syncing " + repo.Name + " to " + repoFolder);
  //       repo.SyncToFolder(repoFolder);
  //     }
  //   }
  // }
}