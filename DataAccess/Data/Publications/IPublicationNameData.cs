using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications;

public interface IPublicationNameData : 
	IRepositoryBase<PublicationNameModel>,
	IRepositoryWithSimpleIdBase<PublicationNameModel, long>,
	IRepositoryWithFilter<PublicationNameModel, PublicationNameFilter>
{
	Task<IEnumerable<PublicationNameModel>> GetPublicationNamesByPublicationIdAsync(long publicationId);
	Task<int> DeletePublicationNamesByPublicationIdAsync(long publicationId);
}
