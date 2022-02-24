using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications;

public class PublicationExternDatabaseIdInMemoryData : InMemoryBaseRepository<PublicationExternDatabaseIdModel>, IPublicationExternDatabaseIdData
{

	public async Task<int> DeletePublicationExternDbIdByIdAsync(long id)
	{
		var deletedObjects = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<int> DeletePublicationExternDbIdsByPublicationIdAsync(long publicationId)
	{
		var deletedObjects = _repository.Where(x => x.PublicationId == publicationId);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<IEnumerable<PublicationExternDatabaseIdModel>> GetAllPublicationExternDbIdsByIdentifierValueSetAsync(IEnumerable<string> identifierValues)
	{
		return _repository.Where(x => identifierValues.Contains(x.PublicationExternDatabaseId));
	}

	public async Task<IEnumerable<PublicationExternDatabaseIdModel>> GetAllPublicationExternDbIdsByPublicationExternDbIdAsync(string externDbId)
	{
		return _repository.Where(x => x.PublicationExternDatabaseId == externDbId);
	}

	public async Task<IEnumerable<PublicationExternDatabaseIdModel>> GetAllPublicationExternDbIdsByPublicationIdAsync(long publicationId)
	{
		return _repository.Where(x => x.PublicationId == publicationId);
	}

	public async Task<PublicationExternDatabaseIdModel?> GetPublicationExternDbIdByIdAsync(long id)
	{
		return _repository.Where(x => x.Id == id).FirstOrDefault();
	}

	public async Task<long> InsertPublicationExternDbIdAsync(PublicationExternDatabaseIdModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdatePublicationExternDbIdAsync(PublicationExternDatabaseIdModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
