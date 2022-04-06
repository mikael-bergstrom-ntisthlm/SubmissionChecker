public class Course
{
  public string Name { get; private set; }

  public List<Assignment> Assignments { get; private set; }

  public List<Student> Students { get; private set; }

  public Course(string name)
  {
    Name = name;
    Assignments = new List<Assignment>();
    Students = new List<Student>();
  }

  public void AddAssignment(Assignment assignment)
  {
    Assignments.Add(assignment);
  }

  public void AddStudent(Student student)
  {
    Students.Add(student);
  }

  public void PrintAssignmentStatus(Assignment assignment)
  {
    Console.WriteLine("Assignment: " + assignment.Name);
    foreach (Student student in Students)
    {
      student.PrintStudent(assignment);
    }
  }

  public void SyncSubmissionsToFolder(Assignment assignment, string folder)
  {
    if (!Directory.Exists(folder)) throw new FileNotFoundException("Folder does not exist");

    string AssignmentFolder = folder + Path.DirectorySeparatorChar + assignment.Name;

    if (!Directory.Exists(AssignmentFolder))
    {
      Directory.CreateDirectory(AssignmentFolder);
    }

    foreach (Student student in Students)
    {
      if (student.Submissions.ContainsKey(assignment))
      {
        string StudentFolder = AssignmentFolder + Path.DirectorySeparatorChar + student.Name;
        if (!Directory.Exists(StudentFolder))
        {
          Directory.CreateDirectory(StudentFolder);
        }

        foreach (Repository repo in student.Submissions[assignment].Repos)
        {
          if (repo.Status == Repository.RepoStatus.ok)
          {
            string repoFolder = StudentFolder + Path.DirectorySeparatorChar + repo.Name;
            if (!Directory.Exists(repoFolder))
            {
              Directory.CreateDirectory(repoFolder);
            }
            Console.WriteLine("Syncing " + repo.Name + " to " + repoFolder);
            repo.SyncToFolder(repoFolder);
          }
        }
      }
    }

  }

  public void LoadAssignmentFromFile(string file, string assignmentName)
  {
    if (!File.Exists(file)) throw new FileNotFoundException();

    // Load csv from file
    using TextFieldParser parser = new(file);

    // Parser setup
    parser.TextFieldType = FieldType.Delimited;
    parser.SetDelimiters(",");

    // Create assignment
    Assignment assignment = new(assignmentName);

    // Add assignment to course
    AddAssignment(assignment);

    // Skip header
    parser.ReadFields();

    // Read each line
    while (!parser.EndOfData)
    {
      // Read line
      string[] fields = parser.ReadFields();

      // Separate fields
      string studentName = fields[0];
      string[] repos = fields[1..];

      // Create student
      Student student = new(studentName, "", "");

      // Add student to course
      AddStudent(student);

      // Create submission
      Submission submission = new(assignment);

      // Add repos to submission
      foreach (string repo in repos)
      {
        string repoClean = repo.Replace(@"https://github.com/", "");
        submission.AddRepo(new Repository(repoClean));
      }

      // Add submission to student
      student.AddSubmission(assignment, submission);
    }
  }
}