using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons;

public class PersonInMemoryData : InMemoryBaseRepository<PersonModel>, IPersonData
{
	public async Task<int> DeletePersonByIdAsync(long id)
	{
		var deletedObjects = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<IEnumerable<PersonModel>> GetAllPersonsAsync()
	{
		return _repository.Select(x => x).ToList();
	}

	public async Task<PersonModel?> GetPersonByIdAsync(long id)
	{
		return _repository.FirstOrDefault(x => x.Id == id);
	}

	public async Task<long> InsertPersonAsync(PersonModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdatePersonAsync(PersonModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
