using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications;

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

	public async Task<IEnumerable<PublicationIdentifierModel>> GetPublicationIdentifiersByIdentifierValueAsync(string identifierValue)
	{
		return _repository.Where(x => x.IdentifierValue == identifierValue);
	}

	public async Task<IEnumerable<PublicationIdentifierModel>> GetAllPublicationIdentifiersByIdentifierValueSetAsync(IEnumerable<string> identifierValues)
	{
		return _repository.Where(x => identifierValues.Contains(x.IdentifierValue));
	}

	public async Task<IEnumerable<PublicationIdentifierModel>> GetPublicationIdentifiersByPublicationIdAsync(long publicationId)
	{
		return _repository.Where(x => x.PublicationId == publicationId);
	}

	public async Task<PublicationIdentifierModel?> GetModelByIdAsync(long id)
	{
		return _repository.FirstOrDefault(x => x.Id == id);
	}

	public async Task<long> InsertModelAsync(PublicationIdentifierModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdateModelAsync(PublicationIdentifierModel model)
	{
		return await UpdateRecordAsync(model);
	}

	public async Task<int> DeleteModelByIdAsync(long id)
	{
		var deletedObjects = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObjects);
	}
}
