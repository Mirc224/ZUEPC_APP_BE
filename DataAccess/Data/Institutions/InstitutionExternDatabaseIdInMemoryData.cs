using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.DataAccess.Data.Institutions;

public class InstitutionExternDatabaseIdInMemoryData : InMemoryBaseRepository<InstitutionExternDatabaseIdModel>, 
	IInstitutionExternDatabaseIdData
{
	public async Task<int> DeleteInstitutionExternDatabaseIdsByInstitutionIdAsync(long institutionId)
	{
		IEnumerable<InstitutionExternDatabaseIdModel> deletedObjects = _repository.Where(x => x.InstitutionId == institutionId);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<int> DeleteModelByIdAsync(long id)
	{
		IEnumerable<InstitutionExternDatabaseIdModel> deletedObjects = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<IEnumerable<InstitutionExternDatabaseIdModel>> GetAllAsync()
	{
		return _repository.ToList();
	}

	public async Task<IEnumerable<InstitutionExternDatabaseIdModel>> GetAllInstitutionExternDbIdsByIdentifierValueSetAsync(
		IEnumerable<string> identifierValues)
	{
		return _repository.Where(x => identifierValues.Contains(x.ExternIdentifierValue));
	}

	public async Task<IEnumerable<InstitutionExternDatabaseIdModel>> GetAllAsync(PaginationFilter filter)
	{
		return _repository.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
	}

	public async Task<IEnumerable<InstitutionExternDatabaseIdModel>> GetInstitutionExternDatabaseIdsByExternIdAsync(string externDbId)
	{
		return _repository.Where(x => x.ExternIdentifierValue == externDbId).ToList();
	}

	public async Task<IEnumerable<InstitutionExternDatabaseIdModel>> GetInstitutionExternDatabaseIdsByInstitutionIdAsync(long institutionId)
	{
		return _repository.Where(x => x.InstitutionId == institutionId).ToList();
	}

	public async Task<InstitutionExternDatabaseIdModel?> GetModelByIdAsync(long id)
	{
		return _repository.FirstOrDefault(x => x.Id == id);
	}

	public async Task<long> InsertModelAsync(InstitutionExternDatabaseIdModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdateModelAsync(InstitutionExternDatabaseIdModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
