using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.Base.ItemInterfaces;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Data.Common;

public abstract class SQLDbEPCRepositoryBase<TModel>:
	SQLDbRepositoryBase<TModel>
	where TModel : IItemWithID<long>
{

	public SQLDbEPCRepositoryBase(ISqlDataAccess db, string baseTableName, string baseTableAlias)
		: base(db, baseTableName, baseTableAlias) {}

	public async Task<int> DeleteModelByIdAsync(long id)
	{
		SqlBuilder builder = new();
		ExpandoObject parameters = new();
		AddToWhereExpression(nameof(IItemWithID<long>.Id), id, builder, parameters);
		return await DeleteModelsAsync(parameters, builder);
	}


	public async Task<TModel?> GetModelByIdAsync(long id)
	{
		return (await GetModelsWithColumnValueAsync(nameof(EPCModelBase.Id), id)).FirstOrDefault();
	}

	public async Task<int> UpdateModelAsync(TModel model)
	{
		SqlBuilder builder = new();
		AddToWhereExpression(nameof(IItemWithID<long>.Id), builder);
		string updateSql = builder.AddTemplate($"{baseUpdateRawSql} /**where**/").RawSql;
		return await db.ExecuteAsync(updateSql, model);
	}

	protected override void BuildBaseInsert()
	{
		Tuple<SqlBuilder, SqlBuilder> result = GetBaseInsertColumnAndInsertValuesSqlBuilderTuple();
		string insertColumns = result.Item1.AddTemplate("/**select**/").RawSql;
		string insertValues = result.Item2.AddTemplate("/**select**/").RawSql;
		baseInsertRawSql = $@"INSERT INTO {baseTableName} ({insertColumns})
						OUTPUT INSERTED.Id
						VALUES ({insertValues})";
	}
}
