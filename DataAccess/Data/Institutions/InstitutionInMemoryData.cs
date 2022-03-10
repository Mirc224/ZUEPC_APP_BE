using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.DataAccess.Data.Institutions;

public class InstitutionInMemoryData : InMemoryBaseRepository<InstitutionModel>, IInstitutionData
{
	public async Task<int> DeleteModelByIdAsync(long id)
	{
		var deletedObjects = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<IEnumerable<InstitutionModel>> GetAllAsync()
	{
		return _repository.ToList();
	}

	public async Task<IEnumerable<InstitutionModel>> GetAllAsync(PaginationFilter filter)
	{
		return _repository.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
	}

	public async Task<InstitutionModel?> GetModelByIdAsync(long id)
	{
		return _repository.FirstOrDefault(x => x.Id == id);
	}

	public async Task<long> InsertModelAsync(InstitutionModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdateModelAsync(InstitutionModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
