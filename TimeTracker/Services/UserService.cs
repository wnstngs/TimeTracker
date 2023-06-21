using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Models;
using TimeTracker.Services.Base;
using TimeTracker.Services.Contracts;

namespace TimeTracker.Services;

public class UserService : BaseService<ApplicationUser>, IUserService
{
	public UserService(ApplicationDbContext dataContext) : base(dataContext)
	{
	}

	public ApplicationUser GetByIdForEdit(int userId)
	{
		return dataContext.Users.IgnoreQueryFilters().First(u => u.Id == userId);
	}

	public bool UserExists(string email)
	{
		var existingUser = dbSet.AsNoTracking().IgnoreQueryFilters().FirstOrDefault(u => u.UserName == email);
		if (existingUser == null)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
}
