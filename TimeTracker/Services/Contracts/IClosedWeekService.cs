namespace TimeTracker.Services.Contracts;

public interface IClosedWeekService
{
	bool IsWeekClosed(string? userId, DateTime week);
}
