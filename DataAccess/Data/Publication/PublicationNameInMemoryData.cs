using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publication;

public class PublicationNameInMemoryData : InMemoryBaseRepository<PublicationNameModel>, IPublicationNameData
{
	
	public async Task<int> DeletePublicationNameByIdAsync(long id)
	{
		var deletedObjects = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<int> DeletePublicationNamesByPublicationIdAsync(long publicationId)
	{
		var deletedObjects = _repository.Where(x => x.PublicationId == publicationId);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<PublicationNameModel?> GetPublicationNameByIdAsync(long nameId)
	{
		return _repository.FirstOrDefault(x => x.Id == nameId);
	}

	public async Task<IEnumerable<PublicationNameModel>> GetPublicationNamesByNameAsync(string name)
	{
		return _repository.Where(x => x.PublicationName == name);
	}

	public async Task<IEnumerable<PublicationNameModel>> GetPublicationNamesByNameTypeAsync(string type)
	{
		return _repository.Where(x => x.NameType == type);
	}

	public async Task<IEnumerable<PublicationNameModel>> GetPublicationNamesByPublicationIdAsync(long publicationId)
	{
		return _repository.Where(x => x.PublicationId == publicationId);
	}

	public async Task<long> InsertPublicationNameAsync(PublicationNameModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdatePublicationNameAsync(PublicationNameModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
