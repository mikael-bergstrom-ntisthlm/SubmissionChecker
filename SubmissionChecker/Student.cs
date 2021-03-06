
public class Student
{
  public string Name { get; private set; }
  public string Email { get; private set; }
  public string Github { get; private set; }
  public Dictionary<Assignment, Submission> Submissions { get; private set; }

  public Student(string name, string email, string github)
  {
    Name = name;
    Email = email;
    Github = github;
    Submissions = new Dictionary<Assignment, Submission>();
  }

  public override string ToString()
  {
    return $"{Name} ({Email})";
  }

  public void AddSubmission(Assignment assignment, Submission submission)
  {
    Submissions.Add(assignment, submission);
  }

  public void PrintStudent(Assignment assignment)
  {
    // Status
    PrintStatus(assignment);

    // Name
    Console.WriteLine(" " + this);

    // Repos
    if (Submissions.ContainsKey(assignment))
    {
      foreach (Repository repo in Submissions[assignment].Repos)
      {
        Console.WriteLine("    " + repo);
      }
    }
    else {
      Console.WriteLine("    No repos");
    }
  }

  public void PrintStatus(Assignment assignment)
  {
    ConsoleColor bg = Console.BackgroundColor;
    ConsoleColor fg = Console.ForegroundColor;
    if (!Submissions.ContainsKey(assignment))
    {
      Console.BackgroundColor = ConsoleColor.Gray;
      Console.ForegroundColor = ConsoleColor.Gray;
    }
    else
    {

      // Check age of submission
      Submission submission = Submissions[assignment];

      // Check status

      switch (submission.GetStatus())
      {
        case Submission.Flags.red:
          Console.BackgroundColor = Console.ForegroundColor = ConsoleColor.Red;
          break;
        case Submission.Flags.yellow:
          Console.BackgroundColor = Console.ForegroundColor = ConsoleColor.Yellow;
          break;
        case Submission.Flags.green:
          Console.BackgroundColor = Console.ForegroundColor = ConsoleColor.Green;
          break;
      }
    }
    Console.Write(" ");
    Console.BackgroundColor = bg;
    Console.ForegroundColor = fg;
  }
}