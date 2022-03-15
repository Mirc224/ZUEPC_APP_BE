using DataAccess.DbAccess;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.RelatedPublication;

namespace ZUEPC.DataAccess.Data.RelatedPublications.InDatabase;

public class SQLRelatedPublicationData :
	SQLDbRepositoryBase<RelatedPublicationModel>,
	IRelatedPublicationData
{
	public SQLRelatedPublicationData(ISqlDataAccess db) : 
		base(db, TableNameConstants.RELATED_PUBLICATIONS_TABLE, TableAliasConstants.RELATED_PUBLICATIONS_TABLE_ALIAS)
	{
	}

	public async Task<int> DeleteRelatedPublicationsByPublicationIdAsync(long publicationId)
	{
		return await DeleteModelsWithColumnValue(nameof(RelatedPublicationModel.PublicationId), publicationId);
	}

	public async Task<int> DeleteRelatedPublicationsByRelatedPublicationIdAsync(long relatedPublicationId)
	{
		return await DeleteModelsWithColumnValue(nameof(RelatedPublicationModel.RelatedPublicationId), relatedPublicationId);
	}

	public async Task<IEnumerable<RelatedPublicationModel>> GetRelatedPublicationsByPublicationIdAsync(long publicationId)
	{
		return await GetModelsWithColumnValue(nameof(RelatedPublicationModel.PublicationId), publicationId);
	}

	public async Task<IEnumerable<RelatedPublicationModel>> GetRelatedPublicationsByRelatedPublicationIdAsync(long relatedPublicationId)
	{
		return await GetModelsWithColumnValue(nameof(RelatedPublicationModel.RelatedPublicationId), relatedPublicationId);
	}
}
