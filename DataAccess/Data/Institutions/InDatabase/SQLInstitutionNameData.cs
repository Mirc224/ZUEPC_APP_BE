using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Extensions;
using ZUEPC.Base.QueryFilters;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.DataAccess.Data.Institutions.InDatabase;

public class SQLInstitutionNameData :
	SQLDbRepositoryWithFilterBase<IInstitutionNameData, InstitutionNameModel, InstitutionNameFilter>,
	IInstitutionNameData
{
	public SQLInstitutionNameData(ISqlDataAccess db) : 
		base(db, TableNameConstants.INSTITUTION_NAMES_TABLE, TableAliasConstants.INSTITUTION_NAMES_TABLE_ALIAS)
	{
	}

	public async Task<int> DeleteInstitutionNamesByInstitutionIdAsync(long institutionId)
	{
		return await DeleteModelsWithColumnValueAsync(nameof(InstitutionNameModel.InstitutionId), institutionId);
	}

	public async Task<IEnumerable<InstitutionNameModel>> GetInstitutionNamesByInstitutionIdAsync(long institutionId)
	{
		return await GetModelsWithColumnValueAsync(nameof(InstitutionNameModel.InstitutionId), institutionId);
	}

	protected override dynamic BuildJoinWithFilterExpression(InstitutionNameFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null)
	{
		return parameters;
	}

	protected override dynamic BuildWhereWithFilterExpression(InstitutionNameFilter queryFilter, SqlBuilder builder, ExpandoObject parameters = null)
	{
		if (parameters is null)
		{
			parameters = new();
		}
		if (queryFilter.Name != null)
		{
			builder.WhereLikeInArray(nameof(InstitutionNameModel.Name), queryFilter.Name, baseTableAlias, parameters);
		}
		
		return parameters;
	}
}
