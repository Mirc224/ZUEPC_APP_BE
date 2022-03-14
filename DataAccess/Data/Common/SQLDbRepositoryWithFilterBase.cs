using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Common;
using static Dapper.SqlBuilder;

namespace ZUEPC.DataAccess.Data.Common;

public abstract class SQLDbRepositoryWithFilterBase<TModel, TFilter> :
	SQLDbRepositoryBase<TModel>
	where TModel : ModelBase
	where TFilter : IQueryFilter
{
	protected SQLDbRepositoryWithFilterBase(ISqlDataAccess db, string baseTableName, string baseTableAlias)
		: base(db, baseTableName, baseTableAlias)
	{
	}

	public async Task<int> CountAsync(TFilter queryFilter)
	{
		SqlBuilder builder = new();
		dynamic parameters = BuildJoinExpression(builder);
		parameters = BuildWhereExpressionAndGetParameters(queryFilter, builder, parameters);
		parameters = BuildGroupByExpression(builder, parameters);
		Template builderTemplate = builder.AddTemplate($@"SELECT COUNT(*) FROM 
														(SELECT {baseTableAlias}.Id FROM {baseTableName} as {baseTableAlias}
														/**innerjoin**/
														/**where**/ /**groupby**/) tmp");
		return await db.ExecuteScalarAsync<int, dynamic>(builderTemplate.RawSql, parameters);
	}

	public async Task<IEnumerable<TModel>> GetAllAsync(TFilter queryFilter, PaginationFilter paginationFilter)
	{
		SqlBuilder builder = new();
		builder.Select(baseSelect);
		ExpandoObject parameters = BuildJoinExpression(builder);
		parameters = BuildWhereExpressionAndGetParameters(queryFilter, builder, parameters);
		parameters = BuildGroupByExpression(builder, parameters);
		return (await GetAllWithPaginationAsync(parameters, paginationFilter, builder));
	}

	protected virtual dynamic BuildWhereExpressionAndGetParameters(TFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null)
	{
		if (parameters is null)
		{
			parameters = new();
		}
		return parameters;
	}
	protected virtual dynamic BuildJoinExpression(SqlBuilder builder, ExpandoObject parameters = null)
	{
		if (parameters is null)
		{
			parameters = new ();
		}
		return parameters;
	}

	protected virtual dynamic BuildGroupByExpression(SqlBuilder builder, ExpandoObject parameters = null)
	{
		if (parameters is null)
		{
			parameters = new();
		}
		return parameters;
	}
}
