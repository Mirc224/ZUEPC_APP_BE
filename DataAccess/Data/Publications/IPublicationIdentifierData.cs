using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications;

public interface IPublicationIdentifierData
{
	Task<IEnumerable<PublicationIdentifierModel>> GetAllPublicationIdentifiersByIdentifierValueAsync(string identifierValue);
	Task<IEnumerable<PublicationIdentifierModel>> GetAllPublicationIdentifiersByIdentifierValueSetAsync(IEnumerable<string> identifierValues);
	Task<IEnumerable<PublicationIdentifierModel>> GetAllPublicationIdentifiersByPublicationIdAsync(long publicationId);
	Task<PublicationIdentifierModel?> GetPublicationIdentifierByIdAsync(long id);
	Task<long> InsertPublicationIdentifierAsync(PublicationIdentifierModel model);
	Task<int> DeletePublicationIdentifierByIdAsync(long id);
	Task<int> DeletePublicationIdentifiersByPublicationIdAsync(long publicationId);
	Task<int> UpdatePublicationIdentifierAsync(PublicationIdentifierModel model);
}
