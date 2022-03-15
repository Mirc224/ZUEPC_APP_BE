using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Extensions;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons.InDatabase;

public class SQLPersonExternDatabaseIdData :
	SQLDbRepositoryBase<PersonExternDatabaseIdModel>,
	IPersonExternDatabaseIdData
{
	public SQLPersonExternDatabaseIdData(ISqlDataAccess db) :
		base(db, TableNameConstants.PERSON_EXTERN_DATABASE_ID_TABLE, TableAliasConstants.PERSON_EXTERN_DATABASE_ID_TABLE_ALIAS)
	{
	}

	public async Task<int> DeletePersonExternDatabaseIdsByPersonIdAsync(long personId)
	{
		SqlBuilder builder = new();
		ExpandoObject parameters = new();
		builder.Select(baseSelect);
		AddToWhereExpression(nameof(PersonExternDatabaseIdModel.PersonId), personId, builder, parameters);
		return await DeleteModelAsync(parameters, builder);
	}

	public async Task<IEnumerable<PersonExternDatabaseIdModel>> GetAllPersonExternDbIdsByIdentifierValueSetAsync(IEnumerable<string> identifierValues)
	{
		SqlBuilder builder = new();
		builder.Select(baseSelect);
		ExpandoObject parameters = builder.WhereInArray(
			nameof(PersonExternDatabaseIdModel.ExternIdentifierValue),
			identifierValues, 
			baseTableAlias);
		return await GetModelsAsync(parameters, builder);
	}

	public async Task<IEnumerable<PersonExternDatabaseIdModel>> GetPersonExternDatabaseIdsByPersonIdAsync(long personId)
	{
		SqlBuilder builder = new();
		ExpandoObject parameters = new();
		builder.Select(baseSelect);
		AddToWhereExpression(nameof(PersonExternDatabaseIdModel.PersonId), personId, builder, parameters);
		return await GetModelsAsync(parameters, builder);
	}
}
