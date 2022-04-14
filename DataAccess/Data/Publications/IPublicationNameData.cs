using ZUEPC.DataAccess.Data.Common;
using ZUEPC.Base.QueryFilters;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications;

public interface IPublicationNameData : 
	IRepositoryBase<PublicationNameModel>,
	IRepositoryWithSimpleIdBase<PublicationNameModel, long>,
	IRepositoryWithFilter<PublicationNameModel, PublicationNameFilter>
{
	Task<IEnumerable<PublicationNameModel>> GetPublicationNamesByPublicationIdAsync(long publicationId);
	Task<IEnumerable<PublicationNameModel>> GetAllPublicationNamesByPublicationIdInSetAsync(IEnumerable<long> publicationIds);
	Task<int> DeletePublicationNamesByPublicationIdAsync(long publicationId);
}
