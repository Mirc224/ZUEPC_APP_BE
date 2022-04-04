using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Extensions;
using ZUEPC.Base.QueryFilters;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications.InDatabase;

public class SQLPublicationNameData :
	SQLDbRepositoryWithFilterBase<IPublicationNameData, PublicationNameModel, PublicationNameFilter>,
	IPublicationNameData
{
	public SQLPublicationNameData(ISqlDataAccess db) : 
		base(db, TableNameConstants.PUBLICATION_NAMES_TABLE, TableAliasConstants.PUBLICATION_NAMES_TABLE_ALIAS)
	{
	}

	public async Task<int> DeletePublicationNamesByPublicationIdAsync(long publicationId)
	{
		return await DeleteModelsWithColumnValueAsync(nameof(PublicationNameModel.PublicationId), publicationId);
	}

	public async Task<IEnumerable<PublicationNameModel>> GetPublicationNamesByPublicationIdAsync(long publicationId)
	{
		return await GetModelsWithColumnValueAsync(nameof(PublicationNameModel.PublicationId), publicationId);
	}

	protected override dynamic BuildJoinWithFilterExpression(PublicationNameFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null)
	{
		return parameters;
	}

	protected override dynamic BuildWhereWithFilterExpression(PublicationNameFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null)
	{
		if (parameters is null)
		{
			parameters = new();
		}
		if (queryFilter.Name != null)
		{
			builder.WhereLikeInArray(
				nameof(PublicationNameModel.Name),
				queryFilter.Name,
				baseTableAlias,
				parameters);
		}
		return parameters;
	}
}
