using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons.InDatabase;

public class SQLPersonNameData :
	SQLDbRepositoryBase<PersonNameModel>,
	IPersonNameData
{
	public SQLPersonNameData(ISqlDataAccess db) : 
		base(db, TableNameConstants.PERSON_NAMES_TABLE, TableAliasConstants.PERSON_NAME_TABLE_ALIAS)
	{
	}

	public async Task<int> DeletePersonNameByPersonIdAsync(long personId)
	{
		SqlBuilder builder = new();
		ExpandoObject parameters = new();
		AddToWhereExpression(nameof(PersonNameModel.PersonId), personId, builder, parameters);
		return await DeleteModelAsync(parameters, builder);
	}

	public async Task<IEnumerable<PersonNameModel>> GetPersonNamesByPersonIdAsync(long personId)
	{
		SqlBuilder builder = new();
		ExpandoObject parameters = new();
		builder.Select(baseSelect);
		AddToWhereExpression(nameof(PersonNameModel.PersonId), personId, builder, parameters);
		return (await GetModelsAsync(parameters, builder));
	}
}
