using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models;

public class TimeEntryFormViewModel
{

	public TimeEntryFormViewModel()
	{
	}

	public TimeEntryFormViewModel(DateTime date, DateTime week)
	{
		Date = date;
		Week = week;
	}

	public int Id { get; set; }

	public int? UserId { get; set; }

	[Required]
	[Range(0, 23)]
	public int HoursSpent { get; set; }

	[Required]
	[Range(0, 59)]
	public int MinutesSpent { get; set; }

	[Required]
	public string? Comment { get; set; } = default!;

	public DateTime Date { get; set; }

	public DateTime Week { get; set; }
}
