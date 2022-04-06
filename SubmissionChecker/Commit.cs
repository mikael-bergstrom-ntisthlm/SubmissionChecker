using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Commit
{

  public string Name { get; private set; }
  public DateTime Date { get; private set; }

  [JsonPropertyName("commit")]
  public JsonCommit SubCommit
  {
    private get { 
      return null;
    }
    set
    {
      Name = value.Author.Name;
      Date = value.Author.Date;
    }
  }
  

  public class JsonCommit
  {
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("author")]
    public JsonAuthor Author { get; set; }
  }

  public class JsonAuthor
  {
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
  }
}