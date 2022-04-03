using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Interfaces;
using static Dapper.SqlBuilder;

namespace ZUEPC.DataAccess.Data.Common;

public abstract class SQLDbRepositoryWithFilterBase<TRepository, TModel, TFilter> :
	SQLDbEPCWithPaginationRepositoryBase<TModel>
	where TModel : IItemWithID<long>
	where TFilter : IQueryFilter
	where TRepository: IRepositoryWithFilter<TModel, TFilter>
{
	protected SQLDbRepositoryWithFilterBase(ISqlDataAccess db, string baseTableName, string baseTableAlias)
		: base(db, baseTableName, baseTableAlias)
	{
	}

	public async Task<int> CountAsync(TFilter queryFilter)
	{
		SqlBuilder builder = new();
		dynamic parameters = BuildJoinWithFilterExpression(queryFilter, builder);
		parameters = BuildWhereWithFilterExpression(queryFilter, builder, parameters);
		parameters = BuildGroupByWithFilterExpression(builder, parameters);
		Template builderTemplate = builder.AddTemplate($@"SELECT COUNT(*) FROM 
														(SELECT {baseTableAlias}.Id FROM {baseTableName} as {baseTableAlias}
														/**innerjoin**/
														/**leftjoin**/
														/**where**/ /**groupby**/) tmp");
		return await db.ExecuteScalarAsync<int, dynamic>(builderTemplate.RawSql, parameters);
	}

	public async Task<IEnumerable<TModel>> GetAllAsync(TFilter queryFilter, PaginationFilter paginationFilter)
	{
		SqlBuilder builder = new();
		builder.Select(baseSelect);
		ExpandoObject parameters = BuildJoinWithFilterExpression(queryFilter, builder);
		parameters = BuildWhereWithFilterExpression(queryFilter, builder, parameters);
		parameters = BuildGroupByWithFilterExpression(builder, parameters);
		return (await GetAllWithPaginationAsync(parameters, paginationFilter, builder));
	}

	protected abstract dynamic BuildWhereWithFilterExpression(TFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null);
	protected abstract dynamic BuildJoinWithFilterExpression(TFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null);
	protected virtual dynamic BuildGroupByWithFilterExpression(SqlBuilder builder, ExpandoObject parameters = null) 
	{
		builder.GroupBy(baseSelect);
		return parameters;
	}

	protected void AddToInnerJoinExpression(
		SqlBuilder builder, 
		string firstTableName, 
		string firstTableAlias, 
		string firstTableColumn, 
		string secondTableName, 
		string secondTableAlias, 
		string secondTableColumn)
	{
		builder.InnerJoin($@"{secondTableName} as {secondTableAlias} ON 
							{firstTableAlias}.{firstTableColumn} = {secondTableAlias}.{secondTableColumn}");
	}
}
