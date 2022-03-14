using System.Linq.Expressions;
using System.Reflection;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Data.Common;

public abstract class InMemoryBaseRepository<T>
	where T : ModelBase
{
	protected readonly List<T> _repository = new();
	protected long _idCounter = 1;

	protected async Task<int> DeleteRecordsAsync(IEnumerable<T> deletedObjects)
	{
		deletedObjects = deletedObjects.ToList();
		foreach (T? deletedObject in deletedObjects)
		{
			_repository.Remove(deletedObject);
		}
		return deletedObjects.Count();
	}

	protected async Task<int> UpdateRecordAsync(T model)
	{
		T? updatedRecord = _repository.FirstOrDefault(x => x.Id == model.Id);
		if (updatedRecord is null)
		{
			return 0;
		}

		_repository.Remove(updatedRecord);
		_repository.Add(model);
		return 1;
	}

	protected async Task<long> InsertRecordAsync(T model)
	{
		model.Id = _idCounter++;
		_repository.Add(model);
		return model.Id;
	}

	public async Task<int> CountAsync()
	{
		return _repository.Count();
	}

	public async Task<IEnumerable<T>> GetAllAsync()
	{
		return _repository.ToList();
	}

	public async Task<IEnumerable<T>> GetAllAsync(PaginationFilter filter)
	{
		return _repository.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
	}

	public async Task<T?> GetModelByIdAsync(long id)
	{
		return _repository.Find(x => x.Id == id);
	}

	public async Task<long> InsertModelAsync(T model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdateModelAsync(T model)
	{
		return await UpdateRecordAsync(model);
	}

	public async Task<int> DeleteModelByIdAsync(long id)
	{
		IEnumerable<T>? deletedObjects = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public Expression? GetFilterExpression<TDomainFilter>(TDomainFilter filter)
	{
		Expression body = null;
		ParameterExpression param = Expression.Parameter(typeof(T));

		Type filterType = filter.GetType();
		IList<PropertyInfo> props = new List<PropertyInfo>(filterType.GetProperties());
		foreach (PropertyInfo prop in props)
		{
			object? propValue = prop.GetValue(filter, null);
			if (propValue is null)
			{
				continue;
			}
			if (body is null)
			{
				body =
				Expression.Equal(
					Expression.Property(param, prop.Name),
					Expression.Constant(prop.GetValue(filter), prop.PropertyType));
				continue;
			}
			Expression andExpression =
				Expression.Equal(
					Expression.Property(param, prop.Name),
					Expression.Constant(prop.GetValue(filter), 
					prop.PropertyType));
			body = Expression.AndAlso(body, andExpression);
		}
		return body;
	}
}
