using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications;

public class PublicationNameInMemoryData : InMemoryBaseRepository<PublicationNameModel>, IPublicationNameData
{
	public async Task<int> DeleteModelByIdAsync(long id)
	{
		var deletedObjects = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<int> DeletePublicationNamesByPublicationIdAsync(long publicationId)
	{
		var deletedObjects = _repository.Where(x => x.PublicationId == publicationId);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<IEnumerable<PublicationNameModel>> GetAllAsync()
	{
		return _repository.ToList();
	}

	public async Task<IEnumerable<PublicationNameModel>> GetAllAsync(PaginationFilter filter)
	{
		return _repository.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
	}

	public async Task<PublicationNameModel?> GetModelByIdAsync(long id)
	{
		return _repository.FirstOrDefault(x => x.Id == id);
	}

	public async Task<IEnumerable<PublicationNameModel>> GetPublicationNamesByNameAsync(string name)
	{
		return _repository.Where(x => x.Name == name);
	}

	public async Task<IEnumerable<PublicationNameModel>> GetPublicationNamesByNameTypeAsync(string type)
	{
		return _repository.Where(x => x.NameType == type);
	}

	public async Task<IEnumerable<PublicationNameModel>> GetPublicationNamesByPublicationIdAsync(long publicationId)
	{
		return _repository.Where(x => x.PublicationId == publicationId);
	}

	public async Task<long> InsertModelAsync(PublicationNameModel model)
	{
		return await InsertRecordAsync(model);
	}
	public async Task<int> UpdateModelAsync(PublicationNameModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
