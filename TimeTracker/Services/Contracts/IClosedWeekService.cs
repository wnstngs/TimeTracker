using TimeTracker.Models;
using TimeTracker.Services.Base;

namespace TimeTracker.Services.Contracts;

public interface IClosedWeekService : IBaseService<ClosedWeek>
{
	bool IsWeekClosed(string? userId, DateTime week);
}
