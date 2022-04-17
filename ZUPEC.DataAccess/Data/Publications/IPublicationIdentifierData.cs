using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications;

public interface IPublicationIdentifierData : 
	IRepositoryBase<PublicationIdentifierModel>,
	IRepositoryWithSimpleIdBase<PublicationIdentifierModel, long>
{
	Task<IEnumerable<PublicationIdentifierModel>> GetPublicationIdentifiersByPublicationIdAsync(long publicationId);
	Task<IEnumerable<PublicationIdentifierModel>> GetAllPublicationIdentifierByPublicationIdInSetAsync(IEnumerable<long> publicationIds);
	Task<IEnumerable<PublicationIdentifierModel>> GetAllPublicationIdentifiersByIdentifierValueSetAsync(IEnumerable<string> identifierValues);
	Task<int> DeletePublicationIdentifiersByPublicationIdAsync(long publicationId);
}
