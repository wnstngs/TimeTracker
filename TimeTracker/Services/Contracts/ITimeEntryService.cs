using TimeTracker.Models;

namespace TimeTracker.Services.Contracts;

public interface ITimeEntryService
{
	List<TimeEntry> GetAllByUserAndWeek(string userId, DateTime week);

	IQueryable<TimeEntry> GetManyByYearAndMonth(int year, int month);
}
