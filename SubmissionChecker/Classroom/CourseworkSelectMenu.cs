using System;
using ConsoleMenu;

namespace SubmissionChecker.Classroom;

public class CourseworkSelectMenu : Menu<string>
{
  public CourseworkSelectMenu(ClassroomConnection connection, string courseId) : base("Choose coursework")
  {
    Console.WriteLine("Loading coursework...");
    var assInfos = connection.GetCourseworkList(courseId);

    foreach (var assInfo in assInfos)
    {
      AddMenuItem(assInfo.Name, assInfo.Id);
    }
    AddMenuItem("-- Cancel --", "");

  }
}
