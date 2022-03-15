using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Extensions;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.DataAccess.Data.Institutions.InDatabase;

public class SQLInstitutionData :
	SQLDbRepositoryWithFilterBase<IInstitutionData, InstitutionModel, InstitutionFilter>,
	IInstitutionData
{
	public SQLInstitutionData(ISqlDataAccess db) 
		: base(db, TableNameConstants.INSTITUTION_TABLE, TableAliasConstants.INSTITUTION_TABLE_ALIAS)
	{
	}

	protected override dynamic BuildJoinWithFilterExpression(InstitutionFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null)
	{
		if (queryFilter.NameType != null ||
		    queryFilter.Name != null)
		{
			AddToInnerJoinExpression(
				builder,
				baseTableName,
				baseTableAlias,
				nameof(InstitutionModel.Id),
				TableNameConstants.INSTITUTION_NAMES_TABLE,
				TableAliasConstants.INSTITUTION_NAMES_TABLE_ALIAS,
				nameof(InstitutionNameModel.InstitutionId));
		}
		if (queryFilter.ExternIdentifierValue != null)
		{
			AddToInnerJoinExpression(
				builder,
				baseTableName,
				baseTableAlias,
				nameof(InstitutionModel.Id),
				TableNameConstants.INSTITUTION_EXTERN_DATABASE_ID_TABLE,
				TableAliasConstants.INSTITUTION_EXTERN_DATABASE_ID_TABLE_ALIAS,
				nameof(InstitutionExternDatabaseIdModel.InstitutionId));
		}
		return parameters;
	}

	protected override dynamic BuildWhereWithFilterExpression(InstitutionFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null)
	{
		if (parameters is null)
		{
			parameters = new();
		}
		if (queryFilter.Level != null)
		{
			builder.WhereInArray(nameof(InstitutionModel.Level), queryFilter.Level, baseTableAlias, parameters);
		}
		if (queryFilter.InstitutionType != null)
		{
			builder.WhereInArray(nameof(InstitutionModel.InstitutionType), queryFilter.InstitutionType, baseTableAlias, parameters);
		}
		if (queryFilter.NameType != null)
		{
			builder.WhereInArray(nameof(InstitutionNameModel.NameType), queryFilter.NameType, TableAliasConstants.INSTITUTION_NAMES_TABLE_ALIAS, parameters);
		}
		if (queryFilter.Name != null)
		{
			builder.WhereInArray(nameof(InstitutionNameModel.Name), queryFilter.Name, TableAliasConstants.INSTITUTION_NAMES_TABLE_ALIAS, parameters);
		}
		if (queryFilter.ExternIdentifierValue != null)
		{
			builder.WhereInArray(
				nameof(InstitutionExternDatabaseIdModel.ExternIdentifierValue),
				queryFilter.ExternIdentifierValue, 
				TableAliasConstants.INSTITUTION_EXTERN_DATABASE_ID_TABLE_ALIAS, 
				parameters);
		}
		return parameters;
	}
}
