namespace TimeTracker.Models;

public class TimeEntriesListViewModel
{
	public List<TimeEntryViewModel> PreviousWeekEntries { get; set; }

	public List<TimeEntryViewModel> CurrentWeekEntries { get; set; }

	public List<TimeEntryViewModel> NextWeekEntries { get; set; }

	public DayOfWeek CurrentDayOfWeek { get; set; }

	public DateTime CurrentWeek { get; set; }

	public DateTime Week { get; set; }

	public bool IsWeekClosed { get; set; }

	public bool IsPreviousWeekClosed { get; set; }

	public bool IsNextWeekClosed { get; set; }

	public TimeEntriesListViewModel(List<TimeEntryViewModel> previousWeekEntries,
		List<TimeEntryViewModel> currentWeekEntries,
		List<TimeEntryViewModel> nextWeekEntries)
	{
		PreviousWeekEntries = previousWeekEntries;
		CurrentWeekEntries = currentWeekEntries;
		NextWeekEntries = nextWeekEntries;
	}
}
