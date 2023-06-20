using TimeTracker.Models;
using TimeTracker.Services.Base;

namespace TimeTracker.Services.Contracts;

public interface ITimeEntryService : IBaseService<TimeEntry>
{
	List<TimeEntry> GetAllByUserAndWeek(string userId, DateTime week);

	IQueryable<TimeEntry> GetManyByYearAndMonth(int year, int month);
}
