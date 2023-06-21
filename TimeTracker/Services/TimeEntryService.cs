using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Models;
using TimeTracker.Services.Base;
using TimeTracker.Services.Contracts;

namespace TimeTracker.Services;

public class TimeEntryService : BaseService<TimeEntry>, ITimeEntryService
{
	public TimeEntryService(IDataContext dataContext) : base(dataContext)
	{
	}

	public List<TimeEntry> GetAllByUserAndWeek(int userId, DateTime week)
	{
		return dataContext.TimeEntries
			.Where(t => t.UserId == userId && t.Week == week)
			.ToList();
	}

	public IQueryable<TimeEntry> GetManyByYearAndMonth(int year, int month)
	{
		return GetAll(true).Include(t => t.User)
			.Where(t => t.Date.Year == year)
			.Where(t => t.Date.Month == month);
	}
}
