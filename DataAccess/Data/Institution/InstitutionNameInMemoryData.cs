using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.DataAccess.Data.Institution;

public class InstitutionNameInMemoryData : InMemoryBaseRepository<InstitutionNameModel>, IInstitutionNameData
{
	public async Task<int> DeleteInstitutionNameByIdAsync(long id)
	{
		var deletedObject = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObject);
	}

	public async Task<int> DeleteInstitutionNamesByInstitutionIdAsync(long institutionId)
	{
		var deletedObject = _repository.Where(x => x.InstitutionId == institutionId);
		return await DeleteRecordsAsync(deletedObject);
	}

	public async Task<InstitutionNameModel?> GetInstitutionNameByIdAsync(long id)
	{
		return _repository.FirstOrDefault(x => x.Id == id);
	}

	public async Task<IEnumerable<InstitutionNameModel>> GetInstitutionNamesByInstitutionIdAsync(long institutionId)
	{
		return _repository.Where(x => x.InstitutionId == institutionId);
	}

	public async Task<IEnumerable<InstitutionNameModel>> GetInstitutionNamesByInstitutionNameAsync(string institutionName)
	{
		return _repository.Where(x => x.Name == institutionName);
	}

	public async Task<long> InsertInstitutionNameAsync(InstitutionNameModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdateInstitutionNameAsync(InstitutionNameModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
