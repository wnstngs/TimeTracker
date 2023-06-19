using TimeTracker.Data;
using TimeTracker.Services.Base;
using TimeTracker.Services.Contracts;

namespace TimeTracker.Services;

public class ClosedWeekService : BaseService<ClosedWeekService>, IClosedWeekService
{
	public ClosedWeekService(ApplicationDbContext dataContext) : base(dataContext)
	{
	}

	public bool IsWeekClosed(string userId, DateTime weekDate)
	{
		return dataContext.ClosedWeeks
			.Where(t => t.Week == weekDate)
			.Any(c => c.UserId == userId);
	}
}
