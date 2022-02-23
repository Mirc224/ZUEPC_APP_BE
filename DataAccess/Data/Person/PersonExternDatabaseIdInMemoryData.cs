using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Person;

public class PersonExternDatabaseIdInMemoryData : InMemoryBaseRepository<PersonExternDatabaseIdModel>, IPersonExternDatabaseIdData
{
	public async Task<int> DeletePersonExternDatabaseIdByIdAsync(long id)
	{
		var deletedObjects = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<int> DeletePersonExternDatabaseIdsByPersonIdAsync(long personId)
	{
		var deletedObjects = _repository.Where(x => x.PersonId == personId);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<PersonExternDatabaseIdModel?> GetPersonExternDatabaseIdByIdAsync(long id)
	{
		return _repository.FirstOrDefault(x => x.Id == id);
	}

	public async Task<IEnumerable<PersonExternDatabaseIdModel>> GetPersonExternDatabaseIdsByExternDbIdAsync(string externDbId)
	{
		return _repository.Where(x => x.PersonExternDatabaseId == externDbId).ToList();
	}

	public async Task<IEnumerable<PersonExternDatabaseIdModel>> GetPersonExternDatabaseIdsByPersonIdAsync(long personId)
	{
		return _repository.Where(x => x.PersonId == personId).ToList();
	}

	public async Task<long> InsertPersonExternDatabaseIdAsync(PersonExternDatabaseIdModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdatePersonExternDatabaseIdAsync(PersonExternDatabaseIdModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
