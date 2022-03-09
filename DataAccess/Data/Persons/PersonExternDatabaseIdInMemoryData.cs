using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons;

public class PersonExternDatabaseIdInMemoryData : InMemoryBaseRepository<PersonExternDatabaseIdModel>, IPersonExternDatabaseIdData
{
	public async Task<int> DeleteModelByIdAsync(long id)
	{
		var deletedObjects = _repository.Where(x => x.Id == id).ToList();
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<int> DeletePersonExternDatabaseIdsByPersonIdAsync(long personId)
	{
		var deletedObjects = _repository.Where(x => x.PersonId == personId).ToList();
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<IEnumerable<PersonExternDatabaseIdModel>> GetAllPersonExternDbIdsByIdentifierValueSetAsync(IEnumerable<string> identifierValues)
	{
		return _repository.Where(x => identifierValues.Contains(x.ExternIdentifierValue));
	}

	public async Task<PersonExternDatabaseIdModel?> GetModelByIdAsync(long id)
	{
		return _repository.FirstOrDefault(x => x.Id == id);
	}

	public async Task<IEnumerable<PersonExternDatabaseIdModel>> GetPersonExternDatabaseIdsByExternDbIdAsync(string externDbId)
	{
		return _repository.Where(x => x.ExternIdentifierValue == externDbId).ToList();
	}

	public async Task<IEnumerable<PersonExternDatabaseIdModel>> GetPersonExternDatabaseIdsByPersonIdAsync(long personId)
	{
		return _repository.Where(x => x.PersonId == personId).ToList();
	}

	public async Task<long> InsertModelAsync(PersonExternDatabaseIdModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdateModelAsync(PersonExternDatabaseIdModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
