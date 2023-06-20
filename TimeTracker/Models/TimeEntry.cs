using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker.Models;

public class TimeEntry
{
	public int Id { get; set; }

	public int UserId { get; set; }

	public ApplicationUser User { get; set; }

	public int HoursSpent { get; set; }

	public int MinutesSpent { get; set; }

	public string? Comment { get; set; }

	[Column(TypeName = "Date")] public DateTime Date { get; set; }

	[Column(TypeName = "Date")] public DateTime Week { get; set; }
}