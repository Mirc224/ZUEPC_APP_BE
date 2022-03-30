using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Extensions;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons.InDatabase;

public class SQLPersonNameData :
	SQLDbRepositoryWithFilterBase<IPersonNameData, PersonNameModel, PersonNameFilter>,
	IPersonNameData
{
	public SQLPersonNameData(ISqlDataAccess db) : 
		base(db, TableNameConstants.PERSON_NAMES_TABLE, TableAliasConstants.PERSON_NAMES_TABLE_ALIAS)
	{
	}

	public async Task<int> DeletePersonNameByPersonIdAsync(long personId)
	{
		return await DeleteModelsWithColumnValueAsync(nameof(PersonNameModel.PersonId), personId);
	}

	public async Task<IEnumerable<PersonNameModel>> GetPersonNamesByPersonIdAsync(long personId)
	{
		return await GetModelsWithColumnValueAsync(nameof(PersonNameModel.PersonId), personId);
	}

	protected override dynamic BuildJoinWithFilterExpression(PersonNameFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null)
	{
		return parameters;
	}

	protected override dynamic BuildWhereWithFilterExpression(PersonNameFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null)
	{
		if (parameters is null)
		{
			parameters = new();
		}
		if (queryFilter.Name != null)
		{
			string concatString = builder.GetConcatFunctionString(nameof(PersonNameModel.FirstName), nameof(PersonNameModel.LastName), baseTableAlias, parameters);
			string concatStringReverse = builder.GetConcatFunctionString(nameof(PersonNameModel.LastName), nameof(PersonNameModel.FirstName), baseTableAlias, parameters);

			string bindedSql = builder.WhereLikeInArrayBindedString(concatString, queryFilter.Name, "", parameters);
			string bindedSqlReverse = builder.WhereLikeInArrayBindedString(concatStringReverse, queryFilter.Name, "", parameters);

			builder.Where($"({bindedSql} OR {bindedSqlReverse})");
		}
		return parameters;
	}
}
