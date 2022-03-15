using DataAccess.DbAccess;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.PublicationActivity;

namespace ZUEPC.DataAccess.Data.PublicationActivities.InDatabase;

public class SQLPublicationActivityData :
	SQLDbRepositoryBase<PublicationActivityModel>,
	IPublicationActivityData
{
	public SQLPublicationActivityData(ISqlDataAccess db) : 
		base(db, TableNameConstants.PUBLICATION_ACTIVITIES_TABLE, TableAliasConstants.PUBLICATION_ACTIVITIES_TABLE_ALIAS)
	{
	}

	public async Task<int> DeletePublicationActivityByPublicationIdAsync(long publicationId)
	{
		return await DeleteModelsWithColumnValueAsync(nameof(PublicationActivityModel.PublicationId), publicationId);
	}

	public async Task<IEnumerable<PublicationActivityModel>> GetPublicationActivitiesByPublicationIdAsync(long publicationId)
	{
		return await GetModelsWithColumnValueAsync(nameof(PublicationActivityModel.PublicationId), publicationId);
	}
}
