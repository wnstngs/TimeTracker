using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TimeTracker.Data;

namespace TimeTracker.Services.Base;

public abstract class BaseService<T> where T : class
{
	protected ApplicationDbContext dataContext;
	protected readonly DbSet<T> dbSet;

	protected BaseService(ApplicationDbContext dataContext)
	{
		this.dataContext = dataContext;
		dbSet = dataContext.Set<T>();
	}

	public virtual void Add(T entity)
	{
		dbSet.Add(entity);
		dataContext.SaveChanges();
	}

	public virtual void AddRange(IEnumerable<T> entities)
	{
		dbSet.AddRange(entities);
		dataContext.SaveChanges();
	}

	public virtual void Update(T entity)
	{
		dbSet.Attach(entity);
		dataContext.Entry(entity).State = EntityState.Modified;

		dataContext.SaveChanges();
	}

	public virtual void UpdateRange(IEnumerable<T> entities)
	{
		dbSet.UpdateRange(entities);
		dataContext.SaveChanges();
	}

	public virtual void Delete(T entity)
	{
		dbSet.Remove(entity);

		dataContext.SaveChanges();
	}

	public virtual void Delete(Expression<Func<T, bool>> where)
	{
		var entities = dbSet.Where(where).AsEnumerable();

		foreach (T entity in entities)
		{
			dbSet.Remove(entity);
		}

		dataContext.SaveChanges();
	}

	public virtual T FindById(int id, bool asNoTracking = false)
	{
		var entity = dbSet.Find(id);

		if (entity == null)
		{
			throw new ArgumentException(
				$"The entity with id {id} of type {typeof(T)} is not found"
			);
		}

		if (asNoTracking)
			dataContext.Entry(entity).State = EntityState.Detached;

		return entity;
	}

	public virtual T? FindByIdOrDefault(int id, bool asNoTracking = false)
	{
		var entity = dbSet.Find(id);

		if (entity == null)
			return null;

		if (asNoTracking)
			dataContext.Entry(entity).State = EntityState.Detached;

		return entity;
	}

	public virtual T? FindByExpressionOrDefault(
		Func<T, bool> where, 
		bool asNoTracking = false, 
		bool ignoreQueryFilters = false)
	{
		IQueryable<T> query = dbSet;

		if (asNoTracking)
			query = query.AsNoTracking();

		if (ignoreQueryFilters)
			query = query.IgnoreQueryFilters();

		return query.FirstOrDefault(where);
	}

	public virtual IQueryable<T> Get(
		Expression<Func<T, bool>> where, 
		bool asNoTracking = false, 
		bool ignoreQueryFilters = false)
	{
		var query = dbSet.Where(where);
		if (asNoTracking)
			query = query.AsNoTracking();

		if (ignoreQueryFilters)
			query = query.IgnoreQueryFilters();

		return query;
	}

	public virtual IQueryable<T> GetAll(
		bool asNoTracking = false, 
		bool ignoreQueryFilters = false)
	{
		var query = dbSet.AsQueryable();
		if (asNoTracking)
			query = query.AsNoTracking();

		if (ignoreQueryFilters)
			query = query.IgnoreQueryFilters();

		return query;
	}
}