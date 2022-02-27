using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications;

public interface IPublicationExternDatabaseIdData
{
	Task<IEnumerable<PublicationExternDatabaseIdModel>> GetPublicationExternDbIdsByPublicationExternDbIdAsync(string externDbId);
	Task<IEnumerable<PublicationExternDatabaseIdModel>> GetPublicationExternDbIdsByPublicationIdAsync(long publicationId);
	Task<IEnumerable<PublicationExternDatabaseIdModel>> GetAllPublicationExternDbIdsByIdentifierValueSetAsync(IEnumerable<string> identifierValues);
	Task<PublicationExternDatabaseIdModel?> GetPublicationExternDbIdByIdAsync(long id);
	Task<long> InsertPublicationExternDbIdAsync(PublicationExternDatabaseIdModel model);
	Task<int> DeletePublicationExternDbIdByIdAsync(long id);
	Task<int> DeletePublicationExternDbIdsByPublicationIdAsync(long publicationId);
	Task<int> UpdatePublicationExternDbIdAsync(PublicationExternDatabaseIdModel model);
}
