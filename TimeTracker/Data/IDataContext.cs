using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Models;

namespace TimeTracker.Data;

public interface IDataContext
{
	DatabaseFacade Database { get; }
	DbSet<TEntity> Set<TEntity>() where TEntity : class;
	EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
	DbSet<ApplicationUser> Users { get; }
	DbSet<ApplicationRole> Roles { get; }
	DbSet<ClosedWeek> ClosedWeeks { get; }
	DbSet<TimeEntry> TimeEntries { get; }
	int SaveChanges();
	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
