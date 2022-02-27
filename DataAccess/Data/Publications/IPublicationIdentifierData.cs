using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications;

public interface IPublicationIdentifierData
{
	Task<IEnumerable<PublicationIdentifierModel>> GetPublicationIdentifiersByIdentifierValueAsync(string identifierValue);
	Task<IEnumerable<PublicationIdentifierModel>> GetPublicationIdentifiersByPublicationIdAsync(long publicationId);
	Task<IEnumerable<PublicationIdentifierModel>> GetAllPublicationIdentifiersByIdentifierValueSetAsync(IEnumerable<string> identifierValues);
	Task<PublicationIdentifierModel?> GetPublicationIdentifierByIdAsync(long id);
	Task<long> InsertPublicationIdentifierAsync(PublicationIdentifierModel model);
	Task<int> DeletePublicationIdentifierByIdAsync(long id);
	Task<int> DeletePublicationIdentifiersByPublicationIdAsync(long publicationId);
	Task<int> UpdatePublicationIdentifierAsync(PublicationIdentifierModel model);
}
