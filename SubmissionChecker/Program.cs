/*
CLEANUP & TESTING
 Fixing github token checking
 Local application data special folder
Commandline vs menu
Make sure we can sync w/ classroom
Make sure we can sync local repos w/ remote
Generate report (html? Open w/ browser?)

Commandline usage:
 checker [coursework.json] --updateclassroom
 checker [coursework.json] --updateclassroom --classroom [classroomcode] --coursework [courseworkcode]
   (Warn if changing)
 checker [coursework.json] --sync-git
 checker [coursework.json] --report
*/


global using RestSharp;
global using System.Text.Json;
global using System.IO;

global using Google.Apis.Auth.OAuth2;
global using Google.Apis.Classroom.v1;
global using Google.Apis.Classroom.v1.Data;
global using Google.Apis.Services;
global using Google.Apis.Util.Store;

using SubmissionChecker;
using SubmissionChecker.Classroom;
using Student = SubmissionChecker.Student;

// Github token MUST have "full control over private repos"

// --- PARAMETERS ---

string ApplicationName = "SubmissionChecker";
string googleCredentialsPath = @".\Credentials\GoogleCredentials.json";
string courseworkJsonFile = "coursework.json";
GithubRepository.GithubApiTokenFilename = @".\Credentials\GithubApiToken";

// --- PREPARE DATA ---
ClassroomConnection connection = new(googleCredentialsPath, ApplicationName);

LocalCourseworkData data = null;

if (File.Exists(courseworkJsonFile))
{
  Console.WriteLine($"Loading {courseworkJsonFile}...");
  try
  {
    data = LocalCourseworkData.LoadFromFile(courseworkJsonFile);
  }
  catch (Exception e)
  {
    Console.WriteLine($"Failed to load {courseworkJsonFile}: {e.Message}");
  }
}

if (data == null || !data.IsValid())
{
  Console.WriteLine("No valid coursework data found. Creating new...");
  data = LocalCourseworkData.CreateFromChoices(connection);

  if (data == null)
  {
    Console.WriteLine("No coursework selected. Exiting...");
    return;
  }

  Console.WriteLine($"Saving to {courseworkJsonFile}...");
  data.SaveToFile(courseworkJsonFile);
}

data.Connection = connection;


// --- GET DATA

// Get list of all students
data.FetchStudents();

data.FetchRepositories();

data.UpdateRepoInfo();

// --- MAIN DISPLAY ---

foreach(Student student in data.Students)
{
  student.Print(true);
}

data.SaveToFile(courseworkJsonFile);


// Display each student's submission status
//  Green: Submitted commit within the last day
//  Yellow: Submitted commit within the last week
//  Red: No commit within the last week
//  Grey: No github link
//  White: No submission
// Shortcut keys:
//  Show student's github link & latest commit
//  Clone / pull student repository


// --- struktur
/*


*/

Console.ReadLine();



// LocalAssignmentData assignmentData = LocalAssignmentData.LoadFromFile("assignment.json");

// LocalCourseworkData courseworkData = LocalCourseworkData.CreateFromChoices(connection);

// if (courseworkData == null) return;

// courseworkData.SaveToFile("coursework.json");

// Console.WriteLine("Press enter to exit");
// Console.ReadLine();




/* -- NOMENCLATURE

 - Course
 - Coursework: Assignment
 - StudentSubmission

*/


// STEP 1: Solid foundation
/*
 - If no course.json found, create new
 - List all current local courses (in course.json)
 - 
*/

/* course.json example

{
  "courseid": "NTM4MjMxNzk0NzAy",
  "coursework": 
  [
    {
      "title": "Slutprojektet",
      "id": "NTkzOTAxNDUzNDk5"
    }
  ]
}

*/



// SubmissionChecker new -- create new course file



// using ConsoleMenu;

// Menu<string> m = new("Main");

// m.AddMenuItem("List courses", "list");
// m.AddMenuItem("Exit", "exit");

// string choice = m.GetChoice();

// connection.GetCoursework("378671304989", "475785723913");

// Topic K4: 475785723914
// Course PRR02: 378671304989
// Assignment Slutprojekt: 475785723913

// var l = connection.GetAssignmentsInfo("378671304989", "475785723914");



// Course c = new (connection, "378671304989");

// c.PrintStudents();

// TODO: Get coursework for assignment, transform to GitHub etc



// var l = connection.GetAssignmentsInfo("378671304989", "475785723914");
// var l = connection.GetTopicsInfo("378671304989");
// Course c = new("PRR01 TE20A");

// c.LoadAssignmentFromFile(@"C:\Users\krank\Documents\_Rättning\Vinterprojektet PRR01\TE20B\gitlist.csv", "Vinterprojektet");
// Console.WriteLine("Loaded!");

// c.PrintAssignmentStatus(c.Assignments[0]);

// c.SyncSubmissionsToFolder(c.Assignments[0], @"C:\Users\krank\Documents\_Rättning\Vinterprojektet PRR01\TE20B\Submissions");

// Console.WriteLine("Finished!");

// Console.ReadLine();



// Get list of courses from the Classroom API
// Get list of assignments for course
// Get list of students and their answers
// For each student/submission, try to find github link
// ---
// For each github link, get date & name of latest commit
// List all students, github accounts, github links, latest commit date, latest commit name
//   Green mark for students who've submitted a commit within the last day
//   Yellow mark for students who've submitted a commit within the last week
//   Red mark for students who haven't submitted a commit within the last week
//   Grey mark for students who haven't submitted a commit at all
// Option to git-clone or git-pull the latest commit

// STUDENT
//  Name
//  Github
//  Email
//  Submissions (Dictionary: Assignment -> Submission)

// SUBMISSION
//  Assigment
//  Repo

// ASSIGNMENT
//  Name