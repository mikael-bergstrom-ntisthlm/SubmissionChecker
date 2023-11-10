using System.Text.Json.Serialization;
using SubmissionChecker.Classroom;

namespace SubmissionChecker;

public class LocalCourseworkData
{
  public string Title { get; set; }
  public string ClassroomCourseId { get; set; }
  public string ClassroomCourseworkId { get; set; }
  public List<Student> Students { get; set; } = new();

  [JsonIgnore]
  public ClassroomConnection Connection { get; set; }

  public void SaveToFile(string file)
  {
    JsonSerializerOptions opt = new()
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      WriteIndented = true
    };

    string jsonData = JsonSerializer.Serialize(this, opt);

    File.WriteAllText(file, jsonData);
  }

  public bool IsValid()
  {
    return Title != "" && ClassroomCourseId != "" && ClassroomCourseworkId != "";
  }

  public void FetchStudents()
  {
    if (Connection == null) return;
    Students = Connection.GetStudents(ClassroomCourseId);
    Students.Sort((x, y) => x.Surname.CompareTo(y.Surname));
  }

  public void FetchRepositories()
  {
    if (Connection == null || Students.Count == 0) return;

    Dictionary<string, List<GithubRepository>> repos = Connection.GetRepositories(ClassroomCourseId, ClassroomCourseworkId, Students.Select(s => s.GoogleId).ToList<string>());

    foreach(string studentId in repos.Keys)
    {
      Student student = Students.Find(x => x.GoogleId == studentId);
      if (student == null) continue;

      // TODO: Append, and only new ones
      student.Repositories = repos[studentId];
    }
  }

  public void UpdateRepoInfo()
  {
    foreach (Student student in Students)
    {
      student.UpdateRepoStatus();
    }
  }

  public static LocalCourseworkData CreateFromChoices(ClassroomConnection connection)
  {
    string courseId = "";
    string courseworkId = "";
    string title = "";
    CourseSelectMenu courseMenu = new(connection);
    CourseworkSelectMenu courseworkMenu;

    while (courseId == "" || courseworkId == "")
    {
      // Get a course
      courseId = courseMenu.GetChoice();

      if (courseId == "") return null;

      // Get a coursework
      courseworkMenu = new(connection, courseId);
      courseworkId = courseworkMenu.GetChoice();

      // If user cancelled, reset courseId
      if (courseworkId == "") courseId = "";

      // Get the title of the coursework
      title = courseworkMenu.GetTextOfValue(courseworkId);

    }

    LocalCourseworkData data = new()
    {
      Title = title,
      ClassroomCourseId = courseId,
      ClassroomCourseworkId = courseworkId
    };

    return data;
  }

  public static LocalCourseworkData LoadFromFile(string file)
  {
    JsonSerializerOptions opt = new()
    {
      PropertyNameCaseInsensitive = true
    };

    string jsonData = File.ReadAllText(file);
    return JsonSerializer.Deserialize<LocalCourseworkData>(jsonData, opt);
  }
}
