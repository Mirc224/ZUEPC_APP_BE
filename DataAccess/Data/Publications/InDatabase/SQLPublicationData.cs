using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Extensions;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.DataAccess.Models.PublicationActivity;
using ZUEPC.DataAccess.Models.PublicationAuthor;

namespace ZUEPC.DataAccess.Data.Publications.InDatabase;

public class SQLPublicationData :
	SQLDbRepositoryWithFilterBase<IPublicationData, PublicationModel, PublicationFilter>,
	IPublicationData
{
	public SQLPublicationData(ISqlDataAccess db) : 
		base(db, TableNameConstants.PUBLICATION_TABLE, TableAliasConstants.PUBLICATION_TABLE_ALIAS)
	{
	}

	protected override dynamic BuildJoinWithFilterExpression(PublicationFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null)
	{
		if(queryFilter.Name != null ||
		   queryFilter.NameType != null)
		{
			AddToInnerJoinExpression(
				builder,
				baseTableName,
				baseTableAlias,
				nameof(PublicationModel.Id),
				TableNameConstants.PUBLICATION_NAMES_TABLE,
				TableAliasConstants.PUBLICATION_NAMES_TABLE_ALIAS,
				nameof(PublicationNameModel.PublicationId));
		}
		if (queryFilter.IdentifierValue != null ||
		   queryFilter.IdentifierName != null ||
		   queryFilter.ISForm != null)
		{
			AddToInnerJoinExpression(
				builder,
				baseTableName,
				baseTableAlias,
				nameof(PublicationModel.Id),
				TableNameConstants.PUBLICATION_IDENTIFIERS_TABLE,
				TableAliasConstants.PUBLICATION_IDENTIFIERS_TABLE_ALIAS,
				nameof(PublicationIdentifierModel.PublicationId));
		}
		if (queryFilter.ExternIdentifierValue != null)
		{
			AddToInnerJoinExpression(
				builder,
				baseTableName,
				baseTableAlias,
				nameof(PublicationModel.Id),
				TableNameConstants.PUBLICATION_EXTERN_DATABASE_ID_TABLE,
				TableAliasConstants.PUBLICATION_EXTERN_DATABASE_ID_TABLE_ALIAS,
				nameof(PublicationExternDatabaseIdModel.PublicationId));
		}
		if (queryFilter.ActivityCategory != null ||
			queryFilter.ActivityYear != null ||
			queryFilter.GovernmentGrant != null)
		{
			AddToInnerJoinExpression(
				builder,
				baseTableName,
				baseTableAlias,
				nameof(PublicationModel.Id),
				TableNameConstants.PUBLICATION_ACTIVITIES_TABLE,
				TableAliasConstants.PUBLICATION_ACTIVITIES_TABLE_ALIAS,
				nameof(PublicationActivityModel.PublicationId));
		}

		if (queryFilter.AuthorName != null ||
			queryFilter.InstitutionName != null)
		{
			AddToInnerJoinExpression(
				builder,
				baseTableName,
				baseTableAlias,
				nameof(PublicationModel.Id),
				TableNameConstants.PUBLICATION_AUTHORS_TABLE,
				TableAliasConstants.PUBLICATION_AUTHORS_TABLE_ALIAS,
				nameof(PublicationAuthorModel.PublicationId));
		}

		if (queryFilter.AuthorName != null)
		{
			AddToInnerJoinExpression(
				builder,
				TableNameConstants.PUBLICATION_AUTHORS_TABLE,
				TableAliasConstants.PUBLICATION_AUTHORS_TABLE_ALIAS,
				nameof(PublicationAuthorModel.PersonId),
				TableNameConstants.PERSON_NAMES_TABLE,
				TableAliasConstants.PERSON_NAMES_TABLE_ALIAS,
				nameof(PersonNameModel.PersonId));
		}
		
		if (queryFilter.InstitutionName != null)
		{
			AddToInnerJoinExpression(
				builder,
				TableNameConstants.PUBLICATION_AUTHORS_TABLE,
				TableAliasConstants.PUBLICATION_AUTHORS_TABLE_ALIAS,
				nameof(PublicationAuthorModel.InstitutionId),
				TableNameConstants.INSTITUTION_NAMES_TABLE,
				TableAliasConstants.INSTITUTION_NAMES_TABLE_ALIAS,
				nameof(InstitutionNameModel.InstitutionId));
		}
		return parameters;
	}

	protected override dynamic BuildWhereWithFilterExpression(PublicationFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null)
	{
		if(parameters is null)
		{
			parameters = new ExpandoObject();
		}
		if (queryFilter.DocumentType!= null)
		{
			builder.WhereInArray(nameof(PublicationModel.DocumentType), queryFilter.DocumentType, baseTableAlias, parameters);
		}
		if (queryFilter.PublishYear != null)
		{
			builder.WhereInArray(nameof(PublicationModel.PublishYear), queryFilter.PublishYear, baseTableAlias, parameters);
		}
		if (queryFilter.Name != null)
		{
			builder.WhereLikeInArray(
				nameof(PublicationNameModel.Name),
				queryFilter.Name, 
				TableAliasConstants.PUBLICATION_NAMES_TABLE_ALIAS, 
				parameters);
		}
		if (queryFilter.NameType != null)
		{
			builder.WhereInArray(
				nameof(PublicationNameModel.NameType), 
				queryFilter.NameType, 
				TableAliasConstants.PUBLICATION_NAMES_TABLE_ALIAS, 
				parameters);
		}
		if (queryFilter.IdentifierValue != null)
		{
			builder.WhereLikeInArray(
				nameof(PublicationIdentifierModel.IdentifierValue), 
				queryFilter.IdentifierValue, 
				TableAliasConstants.PUBLICATION_IDENTIFIERS_TABLE_ALIAS, 
				parameters);
		}
		if (queryFilter.IdentifierName != null)
		{
			builder.WhereInArray(
				nameof(PublicationIdentifierModel.IdentifierName),
				queryFilter.IdentifierName,
				TableAliasConstants.PUBLICATION_IDENTIFIERS_TABLE_ALIAS,
				parameters);
		}
		if (queryFilter.ISForm != null)
		{
			builder.WhereInArray(
				nameof(PublicationIdentifierModel.ISForm),
				queryFilter.ISForm,
				TableAliasConstants.PUBLICATION_IDENTIFIERS_TABLE_ALIAS,
				parameters);
		}
		if (queryFilter.ExternIdentifierValue != null)
		{
			builder.WhereInArray(
				nameof(PublicationExternDatabaseIdModel.ExternIdentifierValue),
				queryFilter.ExternIdentifierValue,
				TableAliasConstants.PUBLICATION_EXTERN_DATABASE_ID_TABLE_ALIAS,
				parameters);
		}
		if (queryFilter.InstitutionName != null)
		{
			builder.WhereLikeInArray(
				nameof(InstitutionNameModel.Name),
				queryFilter.InstitutionName,
				TableAliasConstants.INSTITUTION_NAMES_TABLE_ALIAS,
				parameters);
		}
		if (queryFilter.AuthorName != null)
		{
			string concatString = builder.GetConcatFunctionString(nameof(PersonNameModel.FirstName), nameof(PersonNameModel.LastName), TableAliasConstants.PERSON_NAMES_TABLE_ALIAS, parameters);
			string concatStringReverse = builder.GetConcatFunctionString(nameof(PersonNameModel.LastName), nameof(PersonNameModel.FirstName), TableAliasConstants.PERSON_NAMES_TABLE_ALIAS, parameters);

			string bindedSql = builder.WhereLikeInArrayBindedString(concatString, queryFilter.AuthorName, "", parameters);
			string bindedSqlReverse = builder.WhereLikeInArrayBindedString(concatStringReverse, queryFilter.AuthorName, "", parameters);

			builder.Where($"({bindedSql} OR {bindedSqlReverse})");
		}
		if (queryFilter.ActivityYear != null)
		{
			builder.WhereInArray(
				nameof(PublicationActivityModel.ActivityYear),
				queryFilter.ActivityYear,
				TableAliasConstants.PUBLICATION_ACTIVITIES_TABLE_ALIAS,
				parameters);
		}
		if (queryFilter.ActivityCategory != null)
		{
			builder.WhereInArray(
				nameof(PublicationActivityModel.Category),
				queryFilter.ActivityCategory,
				TableAliasConstants.PUBLICATION_ACTIVITIES_TABLE_ALIAS,
				parameters);
		}
		if (queryFilter.GovernmentGrant != null)
		{
			builder.WhereInArray(
				nameof(PublicationActivityModel.GovernmentGrant),
				queryFilter.GovernmentGrant,
				TableAliasConstants.PUBLICATION_ACTIVITIES_TABLE_ALIAS,
				parameters);
		}
		return parameters;
	}
}
