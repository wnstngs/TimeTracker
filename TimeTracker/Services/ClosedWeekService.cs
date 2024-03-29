﻿using TimeTracker.Data;
using TimeTracker.Models;
using TimeTracker.Services.Base;
using TimeTracker.Services.Contracts;

namespace TimeTracker.Services;

public class ClosedWeekService : BaseService<ClosedWeek>, IClosedWeekService
{
	public ClosedWeekService(IDataContext dataContext) : base(dataContext)
	{
	}

	public bool IsWeekClosed(int userId, DateTime weekDate)
	{
		return dataContext.ClosedWeeks
			.Where(t => t.Week == weekDate)
			.Any(c => c.UserId == userId);
	}
}
