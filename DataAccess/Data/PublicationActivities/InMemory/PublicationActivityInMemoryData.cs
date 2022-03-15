using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.PublicationActivity;

namespace ZUEPC.DataAccess.Data.PublicationActivities;

public class PublicationActivityInMemoryData : InMemoryBaseRepository<PublicationActivityModel>, IPublicationActivityData
{
	public async Task<int> DeleteModelByIdAsync(long id)
	{
		var deletedObject = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObject);
	}

	public async Task<int> DeletePublicationActivityByPublicationIdAsync(long publicationId)
	{
		var deletedObject = _repository.Where(x => x.PublicationId == publicationId);
		return await DeleteRecordsAsync(deletedObject);
	}

	public async Task<IEnumerable<PublicationActivityModel>> GetAllAsync()
	{
		return _repository.ToList();
	}

	public async Task<IEnumerable<PublicationActivityModel>> GetAllAsync(PaginationFilter filter)
	{
		return _repository.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
	}

	public async Task<PublicationActivityModel?> GetModelByIdAsync(long id)
	{
		return _repository.FirstOrDefault(x => x.Id == id);
	}

	public async Task<IEnumerable<PublicationActivityModel>> GetPublicationActivitiesByPublicationIdAsync(long publicationId)
	{
		return _repository.Where(x => x.PublicationId == publicationId);
	}

	public async Task<long> InsertModelAsync(PublicationActivityModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<long> InsertPublicationActivityAsync(PublicationActivityModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdateModelAsync(PublicationActivityModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
