using System;

namespace SubmissionChecker.Classroom;

public class ClassroomConnection
{
  readonly string[] Scopes = { ClassroomService.Scope.ClassroomCoursesReadonly,
                    ClassroomService.Scope.ClassroomCourseworkStudents,
                    ClassroomService.Scope.ClassroomRostersReadonly,
                    ClassroomService.Scope.ClassroomProfileEmails,
                    ClassroomService.Scope.ClassroomTopicsReadonly
                    };
  readonly UserCredential credential;
  readonly ClassroomService service;

  public ClassroomConnection(string credentialsFile, string applicationName)
  {
    using var stream = new FileStream(credentialsFile, FileMode.Open, FileAccess.Read);

    string credPath = "token.json";

    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
      GoogleClientSecrets.FromStream(stream).Secrets,
      Scopes,
      "user",
      CancellationToken.None,
      new FileDataStore(credPath, true)
    ).Result;

    service = new ClassroomService(new BaseClientService.Initializer()
    {
      HttpClientInitializer = credential,
      ApplicationName = applicationName,
    });

  }

  public List<CourseInfo> GetCoursesInfo(bool onlyActive = true)
  {
    CoursesResource.ListRequest request = service.Courses.List();
    request.PageSize = 50;

    ListCoursesResponse response = request.Execute();

    if (response.Courses != null && response.Courses.Count > 0)
    {
      List<CourseInfo> courses = new();

      foreach (var course in response.Courses)
      {
        if (onlyActive && course.CourseState != "ACTIVE") continue;

        courses.Add(new CourseInfo()
        {
          Name = FormatCourseName(course),
          Id = course.Id
        });
      }

      return courses;
    }
    else
    {
      return new List<CourseInfo>();
    }
  }

  public List<CourseworkInfo> GetCourseworkList(string courseId, string topicId = "")
  {
    CoursesResource.CourseWorkResource.ListRequest request = service.Courses.CourseWork.List(courseId);
    request.PageSize = 100;

    ListCourseWorkResponse response = request.Execute();

    if (response.CourseWork != null && response.CourseWork.Count > 0)
    {
      List<CourseworkInfo> courseworkInfos = new();

      foreach (var coursework in response.CourseWork)
      {
        if (topicId != "" && coursework.TopicId != topicId) continue;

        courseworkInfos.Add(new CourseworkInfo()
        {
          Name = coursework.Title,
          Id = coursework.Id,
          Description = coursework.Description,
          State = coursework.State
        });
      }

      return courseworkInfos;
    }
    else
    {
      return new List<CourseworkInfo>();
    }
  }

  public List<TopicInfo> GetTopicsInfo(string courseId)
  {
    CoursesResource.TopicsResource.ListRequest request = service.Courses.Topics.List(courseId);
    request.PageSize = 50;

    ListTopicResponse response = request.Execute();

    if (response.Topic != null && response.Topic.Count > 0)
    {
      List<TopicInfo> topics = new();

      foreach (var topic in response.Topic)
      {
        topics.Add(new TopicInfo()
        {
          Name = topic.Name,
          Id = topic.TopicId
        });
      }

      return topics;
    }
    else
    {
      return new List<TopicInfo>();
    }
  }

  public string GetCourseName(string courseId)
  {
    CoursesResource.GetRequest request = service.Courses.Get(courseId);

    Google.Apis.Classroom.v1.Data.Course course = request.Execute();

    return FormatCourseName(course);
  }

  private static string FormatCourseName(Google.Apis.Classroom.v1.Data.Course course)
  {
    return $"{course.Name} ({course.Section})";
  }

  public List<Student> GetStudents(string courseId)
  {
    CoursesResource.StudentsResource.ListRequest request = service.Courses.Students.List(courseId);
    request.PageSize = 50;

    try
    {
      ListStudentsResponse response = request.Execute();

      if (response.Students != null && response.Students.Count > 0)
      {
        List<Student> students = new();

        foreach (var student in response.Students)
        {
          students.Add(
            new Student(student.Profile.Name.GivenName,
                        student.Profile.Name.FamilyName,
                        student.Profile.EmailAddress,
                        student.Profile.Id,
                        ""));
        }
        return students;
      }
    }
    catch (Exception e)
    {
      Console.WriteLine("Error: " + e.Message);
    }
    return new List<Student>();
  }

  public void GetAssignments(Course course, ICollection<string> assignmentIds)
  {
    // TODO: make list of Assigment objects
  }

  public Dictionary<string, Submission> GetSubmissions(string courseId, Assignment assignment)
  {
    Dictionary<string, Submission> submissions = new(); // studentId, submission

    CoursesResource.CourseWorkResource.StudentSubmissionsResource.ListRequest request = service.Courses.CourseWork.StudentSubmissions.List(courseId, assignment.GoogleAssignmentId);
    request.PageSize = 50;

    var response = request.Execute();

    if (response.StudentSubmissions != null && response.StudentSubmissions.Count > 0)
    {
      foreach (StudentSubmission submission in response.StudentSubmissions)
      {
        // Submission sub = new(assignment);
        // sub.Repos.AddRange(InterpretSubmissionAttachments(submission));
        // submissions.Add(submission.UserId, sub);
      }
    }

    return submissions;
  }

  public class CourseworkInfo
  {
    public string Name { get; set; }
    public string Id { get; set; }
    public string Description { get; set; }
    public string State { get; set; }
  }

  public class TopicInfo
  {
    public string Name { get; set; }
    public string Id { get; set; }
  }

  public class CourseInfo
  {
    public string Name { get; set; }
    public string Id { get; set; }
  }
}