using TimeTracker.Models;
using TimeTracker.Services.Base;

namespace TimeTracker.Services.Contracts;

public interface IUserService : IBaseService<ApplicationUser>
{
	ApplicationUser GetByIdForEdit(int userId);

	bool UserExists(string email);
}
