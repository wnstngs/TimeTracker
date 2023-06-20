using System.Linq.Expressions;

namespace TimeTracker.Services.Base;

public interface IBaseService<T> where T : class
{
	void Add(T entity);

	void AddRange(IEnumerable<T> entities);

	void Update(T entity);

	void UpdateRange(IEnumerable<T> entities);

	void Delete(T entity);

	void Delete(Expression<Func<T, bool>> where);

	T FindById(int id, bool asNoTracking = false);

	T? FindByIdOrDefault(int id, bool asNoTracking = false);

	T? FindByExpressionOrDefault(Func<T, bool> where, bool asNoTracking = false, bool ignoreQueryFilters = false);

	IQueryable<T> Get(Expression<Func<T, bool>> where, bool asNoTracking = false, bool ignoreQueryFilters = false);

	IQueryable<T> GetAll(bool asNoTracking = false, bool ignoreQueryFilters = false);
}
