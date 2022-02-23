using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.PublicationActivity;

namespace ZUEPC.DataAccess.Data.PublicationActivity;

public class PublicationActivityInMemoryData : InMemoryBaseRepository<PublicationActivityModel>, IPublicationActivityData
{
	public async Task<int> DeletePublicationActivityByIdAsync(long id)
	{
		var deletedObject = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObject);
	}

	public async Task<int> DeletePublicationActivityByPublicationIdAsync(long publicationId)
	{
		var deletedObject = _repository.Where(x => x.PublicationId == publicationId);
		return await DeleteRecordsAsync(deletedObject);
	}

	public async Task<IEnumerable<PublicationActivityModel>> GetPublicationActivitiesByPublicationIdAsync(long publicationId)
	{
		return _repository.Where(x => x.PublicationId == publicationId);
	}

	public async Task<PublicationActivityModel?> GetPublicationActivityByIdAsync(long id)
	{
		return _repository.FirstOrDefault(x => x.Id == id);
	}

	public async Task<long> InsertPublicationActivityAsync(PublicationActivityModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdatePublicationActivityAsync(PublicationActivityModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
