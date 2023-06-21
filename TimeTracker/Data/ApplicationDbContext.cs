using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Models;

namespace TimeTracker.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>, IDataContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	public DbSet<TimeEntry> TimeEntries => Set<TimeEntry>();
	public DbSet<ClosedWeek> ClosedWeeks => Set<ClosedWeek>();
}