using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.DataAccess.Data.Institution;

public class InstitutionExternDatabaseIdInMemoryData : InMemoryBaseRepository<InstitutionExternDatabaseIdModel>, IInstitutionExternDatabaseIdData
{
	public async Task<int> DeleteInstitutionExternDatabaseIdByIdAsync(long id)
	{
		var deletedObjects = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<int> DeleteInstitutionExternDatabaseIdsByInstitutionIdAsync(long institutionId)
	{
		var deletedObjects = _repository.Where(x => x.InstitutionId == institutionId);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<InstitutionExternDatabaseIdModel?> GetInstitutionExternDatabaseIdByIdAsync(long id)
	{
		return _repository.FirstOrDefault(x => x.Id == id);
	}

	public async Task<IEnumerable<InstitutionExternDatabaseIdModel>> GetInstitutionExternDatabaseIdsByExternIdAsync(string externDbId)
	{
		return _repository.Where(x => x.InstitutionExternDbId == externDbId).ToList();
	}

	public async Task<IEnumerable<InstitutionExternDatabaseIdModel>> GetInstitutionExternDatabaseIdsByInstitutionIdAsync(long institutionId)
	{
		return _repository.Where(x => x.InstitutionId == institutionId).ToList();
	}

	public async Task<long> InsertInstitutionExternDatabaseIdAsync(InstitutionExternDatabaseIdModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdateInstitutionExternDatabaseIdAsync(InstitutionExternDatabaseIdModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
