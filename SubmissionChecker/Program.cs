global using RestSharp;
global using System.Text.Json;
global using System.IO;
global using LibGit2Sharp;
global using Microsoft.VisualBasic.FileIO;

Course c = new("PRR01 TE20A");

c.LoadAssignmentFromFile(@"C:\Users\krank\Documents\_Rättning\Vinterprojektet PRR01\TE20B\gitlist.csv", "Vinterprojektet");
Console.WriteLine("Loaded!");

c.PrintAssignmentStatus(c.Assignments[0]);

c.SyncSubmissionsToFolder(c.Assignments[0], @"C:\Users\krank\Documents\_Rättning\Vinterprojektet PRR01\TE20B\Submissions");

Console.WriteLine("Finished!");

Console.ReadLine();



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