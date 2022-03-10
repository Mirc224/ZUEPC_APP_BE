using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications;

public class PublicationInMemoryData : InMemoryBaseRepository<PublicationModel>,  IPublicationData
{
	public async Task<int> DeleteModelByIdAsync(long id)
	{
		var deletedObjects = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<IEnumerable<PublicationModel>> GetAllAsync()
	{
		return _repository.ToList();
	}

	public async Task<IEnumerable<PublicationModel>> GetAllAsync(PaginationFilter filter)
	{
		return _repository.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
	}

	public async Task<IEnumerable<PublicationModel>> GetAllPublicationsAsync()
	{
		return _repository.Select(x => x);
	}

	public async Task<PublicationModel?> GetModelByIdAsync(long id)
	{
		return _repository.FirstOrDefault(x => x.Id == id);
	}

	public async Task<long> InsertModelAsync(PublicationModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdateModelAsync(PublicationModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
