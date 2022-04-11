using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Extensions;
using ZUEPC.Base.QueryFilters;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.DataAccess.Data.Persons.InDatabase;

public class SQLPersonData :
	SQLDbRepositoryWithFilterBase<IPersonData, PersonModel, PersonFilter>,
	IPersonData
{
	public SQLPersonData(ISqlDataAccess db) 
		: base(db, TableNameConstants.PERSONS_TABLE, TableAliasConstants.PERSON_TABLE_ALIAS)
	{
	}

	protected override dynamic BuildJoinWithFilterExpression(PersonFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null)
	{
		if (queryFilter.NameType != null ||
			queryFilter.Name != null)
		{
			AddToInnerJoinExpression(
				builder,
				baseTableName,
				baseTableAlias,
				nameof(PersonModel.Id),
				TableNameConstants.PERSON_NAMES_TABLE,
				TableAliasConstants.PERSON_NAMES_TABLE_ALIAS,
				nameof(PersonNameModel.PersonId));
		}

		if (queryFilter.ExternIdentifierValue != null)
		{
			AddToInnerJoinExpression(
				builder, 
				baseTableName, 
				baseTableAlias, 
				nameof(PersonModel.Id),
				TableNameConstants.PERSON_EXTERN_DATABASE_ID_TABLE, 
				TableAliasConstants.PERSON_EXTERN_DATABASE_ID_TABLE_ALIAS, 
				nameof(PersonExternDatabaseIdModel.PersonId));
		}
		return parameters;
	}

	protected override dynamic BuildWhereWithFilterExpression(PersonFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null)
	{
		if (parameters is null)
		{
			parameters = new();
		}
		if (queryFilter.BirthYear != null)
		{
			builder.WhereInArray(nameof(PersonModel.BirthYear), queryFilter.BirthYear, baseTableAlias, parameters);
		}
		if (queryFilter.DeathYear != null)
		{
			builder.WhereInArray(nameof(PersonModel.DeathYear), queryFilter.DeathYear, baseTableAlias, parameters);
		}
		if (queryFilter.Name != null)
		{
			string concatString = builder.GetConcatFunctionString(nameof(PersonNameModel.FirstName), nameof(PersonNameModel.LastName), TableAliasConstants.PERSON_NAMES_TABLE_ALIAS, parameters, "");
			string concatStringReverse = builder.GetConcatFunctionString(nameof(PersonNameModel.LastName), nameof(PersonNameModel.FirstName), TableAliasConstants.PERSON_NAMES_TABLE_ALIAS, parameters, "");

			IEnumerable<string> namesWithReplacedValues = queryFilter.Name.Select(x => x.Replace(' ', '%'));

			string bindedSql = builder.WhereLikeInArrayBindedString(concatString, namesWithReplacedValues, "", parameters);
			string bindedSqlReverse = builder.WhereLikeInArrayBindedString(concatStringReverse, namesWithReplacedValues, "", parameters);

			builder.Where($"({bindedSql} OR {bindedSqlReverse})");
		}
		if (queryFilter.NameType != null)
		{
			builder.WhereInArray(nameof(PersonNameModel.NameType), queryFilter.NameType, TableAliasConstants.PERSON_NAMES_TABLE_ALIAS, parameters);
		}
		if (queryFilter.ExternIdentifierValue != null)
		{
			builder.WhereLikeInArray(
				nameof(PersonExternDatabaseIdModel.ExternIdentifierValue),
				queryFilter.ExternIdentifierValue, 
				TableAliasConstants.PERSON_EXTERN_DATABASE_ID_TABLE_ALIAS, 
				parameters);
		}

		return parameters;
	}

}
