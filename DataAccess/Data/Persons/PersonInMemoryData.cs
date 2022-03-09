using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons;

public class PersonInMemoryData : InMemoryBaseRepository<PersonModel>, IPersonData
{
	public async Task<int> DeleteModelByIdAsync(long id)
	{
		var deletedObjects = _repository.Where(x => x.Id == id).ToList();
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<IEnumerable<PersonModel>> GetAllPersonsAsync()
	{
		return _repository.Select(x => x).ToList();
	}

	public async Task<PersonModel?> GetModelByIdAsync(long id)
	{
		return _repository.FirstOrDefault(x => x.Id == id);
	}

	public async Task<long> InsertModelAsync(PersonModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdateModelAsync(PersonModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
