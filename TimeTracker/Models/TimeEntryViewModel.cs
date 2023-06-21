using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models;

public class TimeEntryViewModel
{
	public TimeEntryViewModel()
	{
	}

	public TimeEntryViewModel(int id)
	{
		Id = id;
	}

	public TimeEntryViewModel(int id, string? comment, int hoursSpent, int minutesSpent)
	{
		Id = id;
		Comment = comment;
		HoursSpent = hoursSpent;
		MinutesSpent = minutesSpent;
	}

	public int Id { get; set; }

	public int UserId { get; set; }

	public int HoursSpent { get; set; }

	public int MinutesSpent { get; set; }

	public string? Comment { get; set; } = default!;

	public DateTime Date { get; set; }

	public DateTime Week { get; set; }
}