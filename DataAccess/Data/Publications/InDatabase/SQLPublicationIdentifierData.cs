using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Extensions;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications.InDatabase;

public class SQLPublicationIdentifierData :
	SQLDbRepositoryBase<PublicationIdentifierModel>,
	IPublicationIdentifierData
{
	public SQLPublicationIdentifierData(ISqlDataAccess db) : 
		base(db, TableNameConstants.PUBLICATION_IDENTIFIERS_TABLE, TableAliasConstants.PUBLICATION_IDENTIFIERS_TABLE_ALIAS)
	{
	}

	public async Task<int> DeletePublicationIdentifiersByPublicationIdAsync(long publicationId)
	{
		return await DeleteModelsWithColumnValue(nameof(PublicationIdentifierModel.PublicationId), publicationId);
	}

	public async Task<IEnumerable<PublicationIdentifierModel>> GetAllPublicationIdentifiersByIdentifierValueSetAsync(IEnumerable<string> identifierValues)
	{
		return await GetModelsWithColumnValueInSet(nameof(PublicationIdentifierModel.IdentifierValue), identifierValues);
	}

	public async Task<IEnumerable<PublicationIdentifierModel>> GetPublicationIdentifiersByPublicationIdAsync(long publicationId)
	{
		return await GetModelsWithColumnValue(nameof(PublicationIdentifierModel.PublicationId), publicationId);
	}
}
