using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publication;

public interface IPublicationExternDatabaseIdData
{
	Task<IEnumerable<PublicationExternDatabaseIdModel>> GetAllPublicationExternDbIdsByPublicationExternDbIdAsync(string externDbId);
	Task<IEnumerable<PublicationExternDatabaseIdModel>> GetAllPublicationExternDbIdsByPublicationIdAsync(long publicationId);
	Task<PublicationExternDatabaseIdModel?> GetPublicationExternDbIdByIdAsync(long id);
	Task<long> InsertPublicationExternDbIdAsync(PublicationExternDatabaseIdModel model);
	Task<int> DeletePublicationExternDbIdByIdAsync(long id);
	Task<int> DeletePublicationExternDbIdsByPublicationIdAsync(long publicationId);
	Task<int> UpdatePublicationExternDbIdAsync(PublicationExternDatabaseIdModel model);
}
