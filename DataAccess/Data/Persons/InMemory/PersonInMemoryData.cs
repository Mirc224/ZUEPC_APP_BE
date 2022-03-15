using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons;

public class PersonInMemoryData : InMemoryBaseRepository<PersonModel>, IPersonData
{
	public Task<int> CountAsync(PersonFilter queryFilter)
	{
		throw new NotImplementedException();
	}

	public async Task<int> DeleteModelByIdAsync(long id)
	{
		var deletedObjects = _repository.Where(x => x.Id == id).ToList();
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<IEnumerable<PersonModel>> GetAllAsync(PaginationFilter filter)
	{
		return _repository.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
	}

	public async Task<IEnumerable<PersonModel>> GetAllAsync()
	{
		return _repository.ToList();
	}

	public Task<IEnumerable<PersonModel>> GetAllAsync(PersonFilter queryFilter, PaginationFilter paginationFilter)
	{
		throw new NotImplementedException();
	}

	public async Task<IEnumerable<PersonModel>> GetAllPersonsAsync(PaginationFilter validFilter)
	{
		return _repository.Skip((validFilter.PageNumber - 1) * 10).Take(validFilter.PageSize).ToList();
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
