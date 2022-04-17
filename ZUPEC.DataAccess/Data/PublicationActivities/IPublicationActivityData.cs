using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.PublicationActivity;

namespace ZUEPC.DataAccess.Data.PublicationActivities;

public interface IPublicationActivityData : 
	IRepositoryBase<PublicationActivityModel>,
	IRepositoryWithSimpleIdBase<PublicationActivityModel, long>
{
	Task<IEnumerable<PublicationActivityModel>> GetAllPublicationActivitiesByPublicationIdInSetAsync(IEnumerable<long> publicationIds);
	Task<IEnumerable<PublicationActivityModel>> GetPublicationActivitiesByPublicationIdAsync(long publicationId);
	Task<int> DeletePublicationActivityByPublicationIdAsync(long publicationId);
}
