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

	[Required(ErrorMessage = "HoursSpentRequired")]
	[Range(0, 23, ErrorMessage = "HoursSpentRange")]
	public int HoursSpent { get; set; }

	[Required(ErrorMessage = "MinutesSpentRequired")]
	[Range(0, 59, ErrorMessage = "MinutesSpentRange")]
	public int MinutesSpent { get; set; }

	[Required(ErrorMessage = "CommentRequired")]
	public string? Comment { get; set; } = default!;

	public DateTime Date { get; set; }

	public DateTime Week { get; set; }
}
