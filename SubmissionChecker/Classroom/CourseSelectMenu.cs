using System;
using ConsoleMenu;

namespace SubmissionChecker.Classroom;

public class CourseSelectMenu : Menu<string>
{
  public CourseSelectMenu(ClassroomConnection connection) : base("Choose course")
  {
    Console.WriteLine("Loading courses...");
    var cInfos = connection.GetCoursesInfo();

    foreach (var cInfo in cInfos)
    {
      AddMenuItem(cInfo.Name, cInfo.Id);
    }
    AddMenuItem("-- Cancel --", "");
  }
}
