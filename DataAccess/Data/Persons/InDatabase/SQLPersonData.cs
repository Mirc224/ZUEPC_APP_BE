using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Extensions;
using ZUEPC.DataAccess.Filters;
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
			queryFilter.FirstName != null ||
			queryFilter.LastName != null )
		{
			AddToLeftJoinExpression(
				builder,
				baseTableName,
				baseTableAlias,
				nameof(PersonModel.Id),
				TableNameConstants.PERSON_NAMES_TABLE,
				TableAliasConstants.PERSON_NAME_TABLE_ALIAS,
				nameof(PersonNameModel.PersonId));
		}

		if (queryFilter.ExternIdentifierValue != null)
		{
			AddToLeftJoinExpression(
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
		if (queryFilter.FirstName != null)
		{
			builder.WhereInArray(nameof(PersonNameModel.FirstName), queryFilter.FirstName, TableAliasConstants.PERSON_NAME_TABLE_ALIAS, parameters);
		}
		if (queryFilter.LastName != null)
		{
			builder.WhereInArray(nameof(PersonNameModel.LastName), queryFilter.LastName, TableAliasConstants.PERSON_NAME_TABLE_ALIAS, parameters);
		}
		if (queryFilter.NameType != null)
		{
			builder.WhereInArray(nameof(PersonNameModel.NameType), queryFilter.NameType, TableAliasConstants.PERSON_NAME_TABLE_ALIAS, parameters);
		}
		if (queryFilter.ExternIdentifierValue != null)
		{
			builder.WhereInArray(
				nameof(PersonExternDatabaseIdModel.ExternIdentifierValue),
				queryFilter.ExternIdentifierValue, 
				TableAliasConstants.PERSON_EXTERN_DATABASE_ID_TABLE_ALIAS, 
				parameters);
		}

		return parameters;
	}
}
