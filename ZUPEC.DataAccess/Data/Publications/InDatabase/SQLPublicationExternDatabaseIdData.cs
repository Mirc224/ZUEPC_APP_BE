using DataAccess.DbAccess;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications.InDatabase;

public class SQLPublicationExternDatabaseIdData :
	SQLDbEPCRepositoryBase<PublicationExternDatabaseIdModel>,
	IPublicationExternDatabaseIdData
{
	public SQLPublicationExternDatabaseIdData(ISqlDataAccess db) : 
		base(db, TableNameConstants.PUBLICATION_EXTERN_DATABASE_ID_TABLE, TableAliasConstants.PUBLICATION_EXTERN_DATABASE_ID_TABLE_ALIAS)
	{
	}

	public async Task<int> DeletePublicationExternDbIdsByPublicationIdAsync(long publicationId)
	{
		return await DeleteModelsWithColumnValueAsync(nameof(PublicationExternDatabaseIdModel.PublicationId), publicationId);
	}

	public async Task<IEnumerable<PublicationExternDatabaseIdModel>> GetAllPublicationExternDbIdsByIdentifierValueSetAsync(IEnumerable<string> identifierValues)
	{
		if (!identifierValues.Any())
		{
			return Enumerable.Empty<PublicationExternDatabaseIdModel>();
		}
		return await GetModelsWithColumnValueInSetAsync(nameof(PublicationExternDatabaseIdModel.ExternIdentifierValue), identifierValues);
	}

	public async Task<IEnumerable<PublicationExternDatabaseIdModel>> GetAllPublicationExternDbIdsByPublicationIdInSetAsync(IEnumerable<long> publicationIds)
	{
		if (!publicationIds.Any())
		{
			return Enumerable.Empty<PublicationExternDatabaseIdModel>();
		}
		return await GetModelsWithColumnValueInSetAsync(nameof(PublicationExternDatabaseIdModel.PublicationId), publicationIds);
	}

	public async Task<IEnumerable<PublicationExternDatabaseIdModel>> GetPublicationExternDbIdsByPublicationIdAsync(long publicationId)
	{
		return await GetModelsWithColumnValueAsync(nameof(PublicationExternDatabaseIdModel.PublicationId), publicationId);
	}
}
