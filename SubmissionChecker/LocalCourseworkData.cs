using SubmissionChecker.Classroom;

namespace SubmissionChecker;

public class LocalCourseworkData
{
  public string Title { get; set; }
  public string ClassroomCourseId { get; set; }
  public string ClassroomCourseworkId { get; set; }

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
