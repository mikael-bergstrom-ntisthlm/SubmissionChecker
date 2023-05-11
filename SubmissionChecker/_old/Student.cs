namespace SubmissionChecker;

public class Student
{
  public string Name { get; private set; }
  public string Surname { get; private set; }
  public string Email { get; private set; }
  public string Github { get; private set; }
  public string GoogleId { get; private set; }

  public string FullName => $"{Name} {Surname}";

  public List<Repository> repositories = new();

  public Student(string name, string surname, string email, string googleId = "")
  {
    Name = name;
    Surname = surname;
    Email = email;
    GoogleId = googleId;
  }

  public override string ToString()
  {
    return $"{Surname} {Name} ({Email})";
  }

  public void Print()
  {
    Console.Write($"{this} ");
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