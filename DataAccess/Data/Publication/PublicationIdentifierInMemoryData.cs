using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publication;

public class PublicationIdentifierInMemoryData : InMemoryBaseRepository<PublicationIdentifierModel>, IPublicationIdentifierData
{
	public async Task<int> DeletePublicationIdentifierByIdAsync(long id)
	{
		var deletedObjects = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<int> DeletePublicationIdentifiersByPublicationIdAsync(long publicationId)
	{
		var deletedObjects = _repository.Where(x => x.PublicationId == publicationId);
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<IEnumerable<PublicationIdentifierModel>> GetAllPublicationIdentifiersByIdentifierValueAsync(string identifierValue)
	{
		return _repository.Where(x => x.PublicationIdentifierValue == identifierValue);
	}

	public async Task<IEnumerable<PublicationIdentifierModel>> GetAllPublicationIdentifiersByIdentifierValueSetAsync(IEnumerable<string> identifierValues)
	{
		return _repository.Where(x => identifierValues.Contains(x.PublicationIdentifierValue));
	}

	public async Task<IEnumerable<PublicationIdentifierModel>> GetAllPublicationIdentifiersByPublicationIdAsync(long publicationId)
	{
		return _repository.Where(x => x.PublicationId == publicationId);
	}

	public async Task<PublicationIdentifierModel?> GetPublicationIdentifierByIdAsync(long id)
	{
		return _repository.FirstOrDefault(x => x.Id == id);
	}

	public async Task<long> InsertPublicationIdentifierAsync(PublicationIdentifierModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdatePublicationIdentifierAsync(PublicationIdentifierModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
