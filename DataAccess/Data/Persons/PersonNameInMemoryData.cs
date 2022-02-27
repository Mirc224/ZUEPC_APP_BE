using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons;

public class PersonNameInMemoryData : InMemoryBaseRepository<PersonNameModel>, IPersonNameData
{

	public async Task<int> DeletePersonNameByIdAsync(long id)
	{
		var deletedObjects = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<int> DeletePersonNameByPersonIdAsync(long personId)
	{
		var deletedObjects = _repository.Where(x => x.PersonId == personId);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<IEnumerable<PersonNameModel>> GetPersonNamesByFirstNameAsync(string firstName)
	{
		return _repository.Where(x => x.FirstName == firstName);
	}

	public async Task<IEnumerable<PersonNameModel>> GetPersonNamesByLastNameAsync(string lastName)
	{
		return _repository.Where(x => x.LastName == lastName);
	}

	public async Task<IEnumerable<PersonNameModel>> GetPersonNamesByPersonIdAsync(long personId)
	{
		return _repository.Where(x => x.PersonId == personId);
	}

	public async Task<PersonNameModel?> GetPersonNameByIdAsync(long id)
	{
		return _repository.FirstOrDefault(x => x.Id == id);
	}

	public async Task<long> InsertPersonNameAsync(PersonNameModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdatePersonNameAsync(PersonNameModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
