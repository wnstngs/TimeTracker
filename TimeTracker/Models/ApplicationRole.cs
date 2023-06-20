using Microsoft.AspNetCore.Identity;

namespace TimeTracker.Models
{
	public class ApplicationRole : IdentityRole<int>
	{
		public ApplicationRole(string name) : base()
		{
			Name = name;
		}
	}
}
