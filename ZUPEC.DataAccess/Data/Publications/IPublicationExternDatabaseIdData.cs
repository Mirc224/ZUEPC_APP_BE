using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications;

public interface IPublicationExternDatabaseIdData : 
	IRepositoryBase<PublicationExternDatabaseIdModel>,
	IRepositoryWithSimpleIdBase<PublicationExternDatabaseIdModel, long>
{
	Task<IEnumerable<PublicationExternDatabaseIdModel>> GetPublicationExternDbIdsByPublicationIdAsync(long publicationId);
	Task<IEnumerable<PublicationExternDatabaseIdModel>> GetAllPublicationExternDbIdsByPublicationIdInSetAsync(IEnumerable<long> publicationIds);
	Task<IEnumerable<PublicationExternDatabaseIdModel>> GetAllPublicationExternDbIdsByIdentifierValueSetAsync(IEnumerable<string> identifierValues);
	Task<int> DeletePublicationExternDbIdsByPublicationIdAsync(long publicationId);
}
