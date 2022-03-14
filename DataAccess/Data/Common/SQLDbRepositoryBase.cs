using Dapper;
using DataAccess.DbAccess;
using System.Reflection;
using ZUEPC.DataAccess.Attributes.ModelAttributes;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Common;
using static Dapper.SqlBuilder;

namespace ZUEPC.DataAccess.Data.Common;

public abstract class SQLDbRepositoryBase<TModel>
	where TModel : ModelBase
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

	public async Task<int> DeleteModelByIdAsync(long id)
	{
		SqlBuilder builder = new();
		builder.Where("Id = @Id");
		return await DeleteModelAsync(new { Id = id }, builder);
	}

	protected async Task<IEnumerable<TModel>> GetModelsAsync(dynamic parameters, SqlBuilder builder)
	{
		Template builderTemplate = builder.AddTemplate(@$"SELECT /**select**/ FROM {baseTableName} AS {baseTableAlias}
														/**innerjoin**/ 
														/**where**/
														/**groupby**/");
		return await db.QueryAsync<TModel, dynamic>(builderTemplate.RawSql, parameters);
	}

	protected async Task<int> DeleteModelAsync(dynamic parameters, SqlBuilder builder)
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

	public async Task<TModel?> GetModelByIdAsync(long id)
	{
		SqlBuilder builder = new();
		builder.Select(baseSelect);
		builder.Where("Id = @Id");
		return (await GetModelsAsync(new { Id = id }, builder)).FirstOrDefault();
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
		int offset = (filter.PageNumber - 1) * filter.PageSize;
		Template builderTemplate = builder.AddTemplate(@$"
				SELECT /**select**/ FROM {baseTableName} AS {baseTableAlias}
				/**innerjoin**/
				/**where**/ 
				/**groupby**/
				ORDER BY [CreatedAt]
				OFFSET {offset} ROWS FETCH NEXT {filter.PageSize} ROWS ONLY");
		return await db.QueryAsync<TModel, dynamic>(builderTemplate.RawSql, parameters);
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
	protected void BuildBaseInsert()
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
						OUTPUT INSERTED.Id
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
		return await db.ExecuteScalarAsync<int, TModel>(baseInsertRawSql, model);
	}

	public async Task<int> UpdateModelAsync(TModel model)
	{
		string updateSql = $@"{baseUpdateRawSql} WHERE Id=@Id";
		return await db.ExecuteAsync(updateSql, model);
	}
}
