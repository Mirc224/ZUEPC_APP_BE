using Dapper;
using DataAccess.DbAccess;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Interfaces;
using static Dapper.SqlBuilder;

namespace ZUEPC.DataAccess.Data.Common;

public abstract class SQLDbEPCWithPaginationRepositoryBase<TModel> : SQLDbEPCRepositoryBase<TModel>
	where TModel : IItemWithID<long>
{
	public SQLDbEPCWithPaginationRepositoryBase(ISqlDataAccess db, string baseTableName, string baseTableAlias) : base(db, baseTableName, baseTableAlias)
	{
	}

	public async Task<IEnumerable<TModel>> GetAllAsync(PaginationFilter filter)
	{
		SqlBuilder builder = new();
		builder.Select(baseSelect);
		return await GetAllWithPaginationAsync(null, filter, builder);
	}

	public async Task<IEnumerable<TModel>> GetAllWithPaginationAsync(
		dynamic parameters,
		PaginationFilter filter,
		SqlBuilder builder)
	{
		BuildOrderByExpression(builder, filter);
		int offset = (filter.PageNumber - 1) * filter.PageSize;
		Template builderTemplate = builder.AddTemplate(@$"
				SELECT /**select**/ FROM {baseTableName} AS {baseTableAlias}
				/**innerjoin**/
				/**leftjoin**/
				/**where**/ 
				/**groupby**/
				/**orderby**/ {filter.Order}
				OFFSET {offset} ROWS FETCH NEXT {filter.PageSize} ROWS ONLY");
		return await db.QueryAsync<TModel, dynamic>(builderTemplate.RawSql, parameters);
	}


	protected void BuildOrderByExpression(SqlBuilder builder, PaginationFilter filter)
	{
		builder.OrderBy(filter.OrderBy);
	}
}
