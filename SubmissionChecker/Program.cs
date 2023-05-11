global using RestSharp;
global using System.Text.Json;
global using System.IO;
global using LibGit2Sharp;

global using Google.Apis.Auth.OAuth2;
global using Google.Apis.Classroom.v1;
global using Google.Apis.Classroom.v1.Data;
global using Google.Apis.Services;
global using Google.Apis.Util.Store;

using SubmissionChecker;
using SubmissionChecker.Classroom;

using ConsoleMenu;

// --- PARAMETERS ---

string ApplicationName = "SubmissionChecker";
string credentialsPath = @".\Credentials\credentials.json";
string courseworkJsonFile = "coursework.json";

// --- PREPARE DATA ---
ClassroomConnection connection = new(credentialsPath, ApplicationName);

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

// --- MAIN DISPLAY ---

// Display each student's submission status
//  Green: Submitted commit within the last day
//  Yellow: Submitted commit within the last week
//  Red: No commit within the last week
//  Grey: No github link
//  White: No submission
// Shortcut keys:
//  Show student's github link & latest commit
//  Clone / pull student repository

/*
SVEN ERIKSSON
  https://www.github.com/sven/eriksson.git
PETRA PIETROVSKI
  https://www.github.com/petra/slutprojekt.git
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