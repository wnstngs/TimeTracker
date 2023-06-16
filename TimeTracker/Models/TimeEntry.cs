using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker.Models;

public class TimeEntry
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public User User { get; set; }

    public int HoursSpent { get; set; }

    public int MinutesSpent { get; set; }

    public string? Comment { get; set; }

    [Column(TypeName = "Date")]
    public DateTime Date { get; set; }

    [Column(TypeName = "Date")] 
    public DateTime Week { get; set; }

    public TimeEntry(
        int userId,
        int hours,
        int minutes,
        DateTime date,
        DateTime week)
    {
        UserId = userId;
        User = default!;
        HoursSpent = hours;
        MinutesSpent = minutes;
        Comment = string.Empty;
        Date = date;
        Week = week;
    }

    public TimeEntry(
        int userId, 
        int hours,
        int minutes,
        string comment,
        DateTime date,
        DateTime week)
    {
        UserId = userId;
        User = default!;
        HoursSpent = hours;
        MinutesSpent = minutes;
        Comment = comment;
        Date = date;
        Week = week;
    }
}