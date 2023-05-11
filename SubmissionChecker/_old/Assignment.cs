using System;

namespace SubmissionChecker;
public class Assignment
{
  public string Name { get; private set; }
  public string GoogleAssignmentId { get; private set; }

  public Assignment(string name, string googleAssignmentId)
  {
    Name = name;
    GoogleAssignmentId = googleAssignmentId;
  }
}