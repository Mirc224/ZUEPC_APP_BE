using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications.InDatabase;

public class SQLPublicationData :
	SQLDbRepositoryWithFilterBase<IPublicationData, PublicationModel, PublicationFilter>,
	IPublicationData
{
	public SQLPublicationData(ISqlDataAccess db) : 
		base(db, TableNameConstants.PUBLICATION_TABLE, TableAliasConstants.PUBLICATION_TABLE_ALIAS)
	{
	}

	protected override dynamic BuildJoinWithFilterExpression(PublicationFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null)
	{
		return parameters;
	}

	protected override dynamic BuildWhereWithFilterExpression(PublicationFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null)
	{
		return parameters;
	}
}
