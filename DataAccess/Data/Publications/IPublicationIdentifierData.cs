using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications;

public interface IPublicationIdentifierData : IRepositoryBase<PublicationIdentifierModel>
{
	Task<IEnumerable<PublicationIdentifierModel>> GetPublicationIdentifiersByIdentifierValueAsync(string identifierValue);
	Task<IEnumerable<PublicationIdentifierModel>> GetPublicationIdentifiersByPublicationIdAsync(long publicationId);
	Task<IEnumerable<PublicationIdentifierModel>> GetAllPublicationIdentifiersByIdentifierValueSetAsync(IEnumerable<string> identifierValues);
	Task<int> DeletePublicationIdentifiersByPublicationIdAsync(long publicationId);
}
