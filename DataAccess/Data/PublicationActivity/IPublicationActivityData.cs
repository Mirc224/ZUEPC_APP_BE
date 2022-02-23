using ZUEPC.DataAccess.Models.PublicationActivity;

namespace ZUEPC.DataAccess.Data.PublicationActivity;

public interface IPublicationActivityData
{
	Task<PublicationActivityModel?> GetPublicationActivityByIdAsync(long id);
	Task<IEnumerable<PublicationActivityModel>> GetPublicationActivitiesByPublicationIdAsync(long publicationId);
	Task<long> InsertPublicationActivityAsync(PublicationActivityModel model);
	Task<int> UpdatePublicationActivityAsync(PublicationActivityModel model);
	Task<int> DeletePublicationActivityByIdAsync(long id);
	Task<int> DeletePublicationActivityByPublicationIdAsync(long publicationId);
}
