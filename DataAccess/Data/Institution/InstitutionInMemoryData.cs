using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.DataAccess.Data.Institution;

public class InstitutionInMemoryData : InMemoryBaseRepository<InstitutionModel>, IInstitutionData
{
	public async Task<int> DeleteInstitutionByIdAsync(long id)
	{
		var deletedObjects = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<IEnumerable<InstitutionModel>> GetAllInstitutionsAsync()
	{
		return _repository.ToList();
	}

	public async Task<InstitutionModel?> GetInstitutionByIdAsync(long id)
	{
		return _repository.FirstOrDefault(x => x.Id == id);
	}

	public async Task<long> InsertInstitutionAsync(InstitutionModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdateInstitutionAsync(InstitutionModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
