namespace TimeTracker.Models;

public class Project
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTime CreatedDate { get; set; }

    public Project(string name)
    {
        Name = name;
        CreatedDate = DateTime.UtcNow;
    }
}