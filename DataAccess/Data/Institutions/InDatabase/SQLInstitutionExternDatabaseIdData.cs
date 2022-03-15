﻿using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Extensions;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.DataAccess.Data.Institutions.InDatabase;

public class SQLInstitutionExternDatabaseIdData :
	SQLDbRepositoryBase<InstitutionExternDatabaseIdModel>,
	IInstitutionExternDatabaseIdData
{
	public SQLInstitutionExternDatabaseIdData(ISqlDataAccess db) : 
		base(db, TableNameConstants.INSTITUTION_EXTERN_DATABASE_ID_TABLE, TableAliasConstants.INSTITUTION_EXTERN_DATABASE_ID_TABLE_ALIAS)
	{
	}

	public async Task<int> DeleteInstitutionExternDatabaseIdsByInstitutionIdAsync(long institutionId)
	{
		SqlBuilder builder = new();
		ExpandoObject parameters = new();
		builder.Select(baseSelect);
		AddToWhereExpression(nameof(InstitutionExternDatabaseIdModel.InstitutionId), institutionId, builder, parameters);
		return await DeleteModelAsync(parameters, builder);
	}

	public async Task<IEnumerable<InstitutionExternDatabaseIdModel>> GetAllInstitutionExternDbIdsByIdentifierValueSetAsync(IEnumerable<string> identifierValues)
	{
		SqlBuilder builder = new();
		builder.Select(baseSelect);
		ExpandoObject parameters = builder.WhereInArray(
			nameof(InstitutionExternDatabaseIdModel.ExternIdentifierValue),
			identifierValues,
			baseTableAlias);
		return await GetModelsAsync(parameters, builder);
	}

	public async Task<IEnumerable<InstitutionExternDatabaseIdModel>> GetInstitutionExternDatabaseIdsByInstitutionIdAsync(long institutionId)
	{
		SqlBuilder builder = new();
		ExpandoObject parameters = new();
		builder.Select(baseSelect);
		AddToWhereExpression(nameof(InstitutionExternDatabaseIdModel.InstitutionId), institutionId, builder, parameters);
		return await GetModelsAsync(parameters, builder);
	}
}
