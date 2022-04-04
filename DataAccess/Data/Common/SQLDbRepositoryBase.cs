using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using System.Reflection;
using ZUEPC.DataAccess.Attributes.ModelAttributes;
using ZUEPC.DataAccess.Extensions;
using static Dapper.SqlBuilder;

namespace ZUEPC.DataAccess.Data.Common;

public abstract class SQLDbRepositoryBase<TModel>
{
	protected readonly ISqlDataAccess db;
	protected readonly string baseTableName;
	protected readonly string baseTableAlias;
	protected string baseSelect;
	protected string baseInsertRawSql;
	protected string baseUpdateRawSql;

	public SQLDbRepositoryBase(ISqlDataAccess db, string baseTableName, string baseTableAlias)
	{
		this.db = db;
		this.baseTableName = baseTableName;
		this.baseTableAlias = baseTableAlias;
		BuildBaseSelect();
		BuildBaseInsert();
		BuildBaseUpdate();
	}

	public async Task<int> CountAsync()
	{
		return await db.ExecuteScalarAsync<int, dynamic>(@$"SELECT COUNT(1) FROM {baseTableName} AS {baseTableAlias}", null);
	}

	protected async Task<IEnumerable<TModel>> GetModelsAsync(dynamic parameters, SqlBuilder builder)
	{
		Template builderTemplate = builder.AddTemplate(@$"SELECT /**select**/ FROM {baseTableName} AS {baseTableAlias}
														/**innerjoin**/ 
														/**where**/
														/**groupby**/");
		return await db.QueryAsync<TModel, dynamic>(builderTemplate.RawSql, parameters);
	}

	protected async Task<int> DeleteModelsAsync(dynamic parameters, SqlBuilder builder)
	{
		Template builderTemplate = builder.AddTemplate($"DELETE FROM {baseTableName} /**where**/");
		return await db.ExecuteAsync(builderTemplate.RawSql, parameters);
	}

	public async Task<IEnumerable<TModel>> GetAllAsync()
	{
		SqlBuilder builder = new();
		builder.Select(baseSelect);
		return await GetModelsAsync(null, builder);
	}

	protected void BuildBaseSelect()
	{
		IEnumerable<PropertyInfo> props = typeof(TModel)
			.GetProperties()
			.Where(prop => !Attribute.IsDefined(prop, typeof(ExcludeFromSelectAttribute)));
		SqlBuilder builder = new();
		foreach (PropertyInfo prop in props)
		{
			builder.Select($"{baseTableAlias}.{prop.Name}");
		}
		baseSelect = builder.AddTemplate("/**select**/").RawSql;
	}
	protected virtual void BuildBaseInsert()
	{
		IEnumerable<PropertyInfo> props = typeof(TModel)
			.GetProperties()
			.Where(prop => !Attribute.IsDefined(prop, typeof(ExcludeFromInsertAttribute)));
		SqlBuilder insertBuilder = new();
		SqlBuilder valuesBuilder = new();
		foreach (PropertyInfo prop in props)
		{
			insertBuilder.Select(prop.Name);
			valuesBuilder.Select("@" + prop.Name);
		}

		string insertColumns = insertBuilder.AddTemplate("/**select**/").RawSql;
		string insertValues = valuesBuilder.AddTemplate("/**select**/").RawSql;
		baseInsertRawSql = $@"INSERT INTO {baseTableName} ({insertColumns})
							  VALUES ({insertValues})";
	}

	protected void BuildBaseUpdate()
	{
		IEnumerable<PropertyInfo> props = typeof(TModel)
			.GetProperties()
			.Where(prop => !Attribute.IsDefined(prop, typeof(ExcludeFromUpdateAttribute)));
		SqlBuilder updateBuilder = new();
		foreach (PropertyInfo prop in props)
		{
			updateBuilder.Select($"{prop.Name} = @{prop.Name}");
		}

		string updateValues = updateBuilder.AddTemplate("/**select**/").RawSql;
		baseUpdateRawSql = $@"UPDATE {baseTableName} SET {updateValues}";
	}

	public async Task<long> InsertModelAsync(TModel model)
	{
		return await db.ExecuteScalarAsync<long, TModel>(baseInsertRawSql, model);
	}

	public void AddToWhereExpression<T>(string columnName, T value, SqlBuilder builder, ExpandoObject parameters, string op = "=")
	{
		builder.Where($"{columnName} {op} @{columnName}");
		parameters.TryAdd(columnName, value);
	}
	public void AddToWhereExpression(string columnName, SqlBuilder builder, string op = "=")
	{
		builder.Where($"{columnName} {op} @{columnName}");
	}

	protected async Task<int> DeleteModelsWithColumnValueAsync<T>(string columnName, T value)
	{
		SqlBuilder builder = new();
		ExpandoObject parameters = new();
		AddToWhereExpression(columnName, value, builder, parameters);
		return await DeleteModelsAsync(parameters, builder);
	}

	protected async Task<IEnumerable<TModel>> GetModelsWithColumnValueAsync<T>(string columnName, T value)
	{
		SqlBuilder builder = new();
		ExpandoObject parameters = new();
		builder.Select(baseSelect);
		AddToWhereExpression(columnName, value, builder, parameters);
		return await GetModelsAsync(parameters, builder);
	}

	protected async Task<IEnumerable<TModel>> GetModelsWithColumnValueInSetAsync<T>(string columnName, IEnumerable<T> values)
	{
		SqlBuilder builder = new();
		builder.Select(baseSelect);
		ExpandoObject parameters = builder.WhereInArray(
			columnName,
			values,
			baseTableAlias);
		return await GetModelsAsync(parameters, builder);
	}

	protected Tuple<SqlBuilder, SqlBuilder> GetBaseInsertColumnAndInsertValuesSqlBuilderTuple()
	{
		IEnumerable<PropertyInfo> props = typeof(TModel)
			.GetProperties()
			.Where(prop => !Attribute.IsDefined(prop, typeof(ExcludeFromInsertAttribute)));
		SqlBuilder insertBuilder = new();
		SqlBuilder valuesBuilder = new();
		foreach (PropertyInfo prop in props)
		{
			insertBuilder.Select(prop.Name);
			valuesBuilder.Select("@" + prop.Name);
		}

		return new Tuple<SqlBuilder, SqlBuilder>(insertBuilder, valuesBuilder);
	}
}
