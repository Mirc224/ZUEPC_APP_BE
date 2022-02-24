using ZUEPC.DataAccess.Models.RelatedPublication;

namespace ZUEPC.DataAccess.Data.RelatedPublications;

public interface IRelatedPublicationData
{
	Task<RelatedPublicationModel?> GetRelatedPublicationByIdAsync(long id);
	Task<IEnumerable<RelatedPublicationModel>> GetRelatedPublicationsByPublicationIdAsync(long publicationId);
	Task<IEnumerable<RelatedPublicationModel>> GetRelatedPublicationsByRelatedPublicationIdAsync(long relatedPublicationId);
	Task<long> InsertRelatedPublicationAsync(RelatedPublicationModel model);
	Task<int> UpdateRelatedPublicationAsync(RelatedPublicationModel model);
	Task<int> DeleteRelatedPublicationByIdAsync(long id);
	Task<int> DeleteRelatedPublicationsByPublicationIdAsync(long publicationId);
	Task<int> DeleteRelatedPublicationsByRelatedPublicationIdAsync(long relatedPublicationId);
}
