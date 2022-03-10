using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Data.Common;

public abstract class InMemoryBaseRepository<T>
	where T : EPCBaseModel
{
	protected readonly List<T> _repository = new();
	protected long _idCounter = 1;

	protected async Task<int> DeleteRecordsAsync(IEnumerable<T> deletedObjects)
	{
		deletedObjects = deletedObjects.ToList();
		foreach (var deletedObject in deletedObjects)
		{
			_repository.Remove(deletedObject);
		}
		return deletedObjects.Count();
	}

	protected async Task<int> UpdateRecordAsync(T model)
	{
		var updatedRecord = _repository.FirstOrDefault(x=> x.Id == model.Id);
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
}
