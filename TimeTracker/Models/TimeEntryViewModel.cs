using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models;

public class TimeEntryViewModel
{
	public int Id { get; set; }

	public int UserId { get; set; }

	public int HoursSpent { get; set; }

	public int MinutesSpent { get; set; }

	public string? Comment { get; set; } = default!;

	public DateTime Date { get; set; }

	public DateTime Week { get; set; }

	public TimeEntryViewModel()
	{
	}
}