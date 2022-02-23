using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publication;

public class PublicationInMemoryData : InMemoryBaseRepository<PublicationModel>,  IPublicationData
{
	
	public async Task<int> DeletePublicationByIdAsync(long id)
	{
		var deletedObjects = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<IEnumerable<PublicationModel>> GetAllPublicationsAsync()
	{
		return _repository.Select(x => x);
	}

	public async Task<PublicationModel?> GetPublicationByIdAsync(long id)
	{
		return _repository.FirstOrDefault(x => x.Id == id);
	}

	public async Task<long> InsertPublicationAsync(PublicationModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdatePublicationAsync(PublicationModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
