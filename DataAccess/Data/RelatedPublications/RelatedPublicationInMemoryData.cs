using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.RelatedPublication;

namespace ZUEPC.DataAccess.Data.RelatedPublications;

public class RelatedPublicationInMemoryData : InMemoryBaseRepository<RelatedPublicationModel>, IRelatedPublicationData
{
	public async Task<int> DeleteModelByIdAsync(long id)
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

	public async Task<RelatedPublicationModel?> GetModelByIdAsync(long id)
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

	public async Task<long> InsertModelAsync(RelatedPublicationModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdateModelAsync(RelatedPublicationModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
