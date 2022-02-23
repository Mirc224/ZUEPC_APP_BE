using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.RelatedPublication;

namespace ZUEPC.DataAccess.Data.RelatedPublication;

public class RelatedPublicationInMemoryData : InMemoryBaseRepository<RelatedPublicationModel>, IRelatedPublicationData
{
	public async Task<int> DeleteRelatedPublicationByIdAsync(long id)
	{
		var deletedObject = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObject);
	}

	public async Task<int> DeleteRelatedPublicationsByPublicationIdAsync(long publicationId)
	{
		var deletedObject = _repository.Where(x => x.PublicationId == publicationId);
		return await DeleteRecordsAsync(deletedObject);
	}

	public async Task<int> DeleteRelatedPublicationsByRelatedPublicationIdAsync(long relatedPublicationId)
	{
		var deletedObject = _repository.Where(x => x.RelatedPublicationId == relatedPublicationId);
		return await DeleteRecordsAsync(deletedObject);
	}

	public async Task<RelatedPublicationModel?> GetRelatedPublicationByIdAsync(long id)
	{
		return _repository.FirstOrDefault(x => x.Id == id);
	}

	public async Task<IEnumerable<RelatedPublicationModel>> GetRelatedPublicationsByPublicationIdAsync(long publicationId)
	{
		return _repository.Where(x => x.PublicationId == publicationId);
	}

	public async Task<IEnumerable<RelatedPublicationModel>> GetRelatedPublicationsByRelatedPublicationIdAsync(long relatedPublicationId)
	{
		return _repository.Where(x => x.RelatedPublicationId == relatedPublicationId);
	}

	public async Task<long> InsertRelatedPublicationAsync(RelatedPublicationModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdateRelatedPublicationAsync(RelatedPublicationModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
