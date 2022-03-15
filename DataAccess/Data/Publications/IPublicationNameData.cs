using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications;

public interface IPublicationNameData : IRepositoryBase<PublicationNameModel>
{
	Task<IEnumerable<PublicationNameModel>> GetPublicationNamesByPublicationIdAsync(long publicationId);
	Task<int> DeletePublicationNamesByPublicationIdAsync(long publicationId);
}
