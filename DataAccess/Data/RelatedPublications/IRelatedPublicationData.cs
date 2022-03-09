using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.RelatedPublication;

namespace ZUEPC.DataAccess.Data.RelatedPublications;

public interface IRelatedPublicationData : IRepositoryBase<RelatedPublicationModel>
{
	Task<IEnumerable<RelatedPublicationModel>> GetRelatedPublicationsByPublicationIdAsync(long publicationId);
	Task<IEnumerable<RelatedPublicationModel>> GetRelatedPublicationsByRelatedPublicationIdAsync(long relatedPublicationId);
	Task<int> DeleteRelatedPublicationsByPublicationIdAsync(long publicationId);
	Task<int> DeleteRelatedPublicationsByRelatedPublicationIdAsync(long relatedPublicationId);
}
